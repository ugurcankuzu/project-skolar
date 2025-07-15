using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using project_onlineClassroom.DTOs;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;
using project_onlineClassroom.Repositories;
using project_onlineClassroom.Services;
using project_onlineClassroom.Util;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Problem occured while fetching connection string from appsettings.json");


builder.Services.AddDbContext<AppDbContext>(context => context.UseSqlServer(connectionString));
builder.Services.AddScoped<ISummaryService, SummaryService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<IParticipantService, ParticipantService>();
builder.Services.AddScoped<ITopicRepository, TopicRepository>();
builder.Services.AddScoped<ITopicService, TopicService>();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(name: "AllowedDomains", policy =>
    {
        policy.WithOrigins("https://localhost:3000").AllowAnyHeader().AllowAnyMethod();
    });
});
builder.Services.AddScoped<JWTHelper>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    setup.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme

    });

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new List<string>()
        }
    });
    setup.CustomSchemaIds(type =>
    {
        // Check if the type is GenericResponse<T>
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(GenericResponse<>))
        {
            // Get the inner type (e.g., ClassDTO or List<ClassDTO>)
            var innerType = type.GetGenericArguments()[0];

            // Check if the inner type is itself a Generic List, Collection, or IEnumerable
            if (innerType.IsGenericType &&
                (innerType.GetGenericTypeDefinition() == typeof(List<>) ||
                 innerType.GetGenericTypeDefinition() == typeof(ICollection<>) ||
                 innerType.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                // Get the actual type inside the list (e.g., ClassDTO)
                var listItemType = innerType.GetGenericArguments()[0];
                // "ListOf" + "ClassDTO" + "Response" -> "ListOfClassDTOResponse"
                return $"ListOf{listItemType.Name}Response";
            }
            else
            {
                // If T is a simple type (e.g., ClassDTO), use the old logic
                // "ClassDTO" + "Response" -> "ClassDTOResponse"
                return $"{innerType.Name}Response";
            }
        }

        // Use the default name for all other types
        return type.Name;
    });

});

builder.Services.AddControllers();
//JWT Bearer Middleware integration
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt =>
{
    opt.SaveToken = true;
    opt.RequireHttpsMetadata = false; // For Development purposes only
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false, //Dev only
        ValidateAudience = false, //Dev only
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(DotNetEnv.Env.GetString("AUTH_SECRET")))
    };

});

var app = builder.Build();
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine("=== GLOBAL EXCEPTION HANDLER ===");
        Console.WriteLine($"Path: {context.Request.Path}");
        Console.WriteLine($"Method: {context.Request.Method}");
        Console.WriteLine($"Exception: {ex.Message}");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");
        Console.WriteLine("================================");
        throw; // Re-throw to let other handlers deal with it
    }
});
app.UseHttpsRedirection();
app.UseCors("AllowedDomains");
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();



if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
