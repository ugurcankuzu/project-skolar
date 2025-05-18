using System.Text.RegularExpressions;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;
using project_onlineClassroom.Util;

namespace project_onlineClassroom.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;
        public AuthRepository(AppDbContext context) => _context = context;
        public async Task<User> LoginAsync(LoginRequest loginRequest)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email)
                ?? throw new UserNotFoundException();

            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
                throw new InvalidPasswordException();

            return user;
        }


        public async Task<bool> RegisterAsync(RegisterRequest registerRequest)
        {

            // Check if user exists
            User? existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == registerRequest.Email);
            if (existingUser != null)
            {
                throw new UserExistsException(); // User already exists.
            }
            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                throw new PasswordMismatchException(); // Passwords do not match
            }
            // Check if the email is valid
            Regex emailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(registerRequest.Email))
            {
                throw new InvalidEmailException();
            }
            User newUser = new()
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                IsEducator = registerRequest.IsEducator,
                Password = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow

            };

            // Add user to the database
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return true; // Registration successful
        }

    }
}

