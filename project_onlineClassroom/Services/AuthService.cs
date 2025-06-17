using System.Text.RegularExpressions;
using Google.Apis.Auth;
using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;
using project_onlineClassroom.Util;

namespace project_onlineClassroom.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly JWTHelper _jwtHelper;

        public AuthService(IAuthRepository authRepository, IUserRepository userRepository, JWTHelper jwtHelper)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
        }
        /// <summary>
        /// Logs in a user by verifying their credentials and generating a JWT token.
        /// </summary>
        /// <param name="request"></param>
        /// <returns><see cref="string"/></returns>
        public async Task<string> LoginAsync(LoginRequest request)
        {
            var user = await _authRepository.LoginAsync(request);
            return _jwtHelper.GenerateToken(user);

        }
        /// <summary>
        /// Registers a new user by validating the input and creating a new user profile in the database.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="PasswordMismatchException"></exception>
        /// <exception cref="InvalidEmailException"></exception>
        /// <exception cref="UserExistsException"></exception>
        public async Task RegisterAsync(RegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword) throw new PasswordMismatchException();
            Regex emailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(request.Email)) throw new InvalidEmailException();
            if (await _userRepository.GetByEmailAsync(request.Email) != null) throw new UserExistsException();
            await _authRepository.RegisterAsync(request);
        }
        public async Task<string> LoginWithGoogle(string idToken)
        {
            if (string.IsNullOrEmpty(idToken)) throw new Exception("Google ID token is required.");
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            User? user = await _userRepository.GetByProviderKey(payload.Subject, "Google");
            if (user != null)
            {
                return _jwtHelper.GenerateToken(user);
            }
            User newUser = new User()
            {
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                Email = payload.Email,
                IsEducator = false, // Default to student, can be changed later
                AuthProvider = "Google",
                Password = Guid.NewGuid().ToString(), // Generate a random password since Google login doesn't use one
                ProviderKey = payload.Subject,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _userRepository.AddAsync(newUser);
            return _jwtHelper.GenerateToken(newUser);
        }
    }
}