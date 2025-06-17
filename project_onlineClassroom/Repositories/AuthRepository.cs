using Google.Apis.Auth;
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
        /// <summary>
        /// Logs in a user by verifying their email and password.
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        /// <exception cref="InvalidPasswordException"></exception>
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
        /// <summary>
        /// Registers a new user by creating a new user profile in the database.
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        /// <exception cref="UserExistsException"></exception>

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
        /*public async Task<User> LoginWithGoogle(GoogleJsonWebSignature.Payload payload)
        {
            User? user = await _userRepository.GetByEmailAsync(payload.Email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    Email = payload.Email,
                    IsEducator = false, // Default value, can be changed later
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Password = Guid.NewGuid().ToString(), // Generate a random password
                    // Note: Password is not used for Google login, but required for User model
                    ProviderKey = payload.Subject,
                    AuthProvider = "Google"
                };
                await _userRepository.AddAsync(user);
            }
            return user;
        }*/
    }
}

