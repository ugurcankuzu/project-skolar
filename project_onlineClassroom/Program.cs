using Microsoft.EntityFrameworkCore;
using project_onlineClassroom.Models;
using project_onlineClassroom.Repositories;
/*using project_onlineClassroom.Application;
*/
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Problem occured while fetching connection string from appsettings.json");
builder.Services.AddDbContext<AppDbContext>(ctx => ctx.UseSqlServer(connectionString));
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
var app = builder.Build();
app.MapControllers();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    Console.WriteLine($"{builder.Configuration["AUTH_SECRET"]}");
}

app.Run();
