using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IUserRepository _userRepository;
        public AuthRepository(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> LoginAsync(LoginRequest loginRequest)
        {
            // All checks done in service layer
            User? user = await _userRepository.GetByEmailAsync(loginRequest.Email) ?? throw new UserNotFoundException();
            if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            {
                throw new InvalidPasswordException(); // Invalid password
            }
            return user;

        }


        public async Task<bool> RegisterAsync(RegisterRequest registerRequest)
        {
            User? user = await _userRepository.GetByEmailAsync(registerRequest.Email);
            if (user != null)
            {
                throw new UserExistsException();
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
            await _userRepository.AddAsync(newUser);
            return true;
        }

    }
}

