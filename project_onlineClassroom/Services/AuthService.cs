using System.Text.RegularExpressions;
using Google.Apis.Auth;
using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs.AuthDTOs;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;
using project_onlineClassroom.Util;
using System;
using System.Threading.Tasks;
using System.Security.Authentication; // Added for InvalidCredentialException

namespace project_onlineClassroom.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JWTHelper _jwtHelper;

        public AuthService(IUserRepository userRepository, JWTHelper jwtHelper)
        {
            _userRepository = userRepository;
            _jwtHelper = jwtHelper;
        }

        /// <summary>
        /// Authenticates a user based on their email and password.
        /// </summary>
        /// <param name="request">The login request DTO containing the user's credentials.</param>
        /// <returns>A JWT string upon successful authentication.</returns>
        /// <exception cref="InvalidCredentialException">Thrown if the email does not exist or the password is incorrect. This is a deliberate, generic exception to prevent username enumeration attacks.</exception>
        public async Task<string> LoginAsync(LoginRequest request)
        {
            User? user = await _userRepository.FindByEmailAsync(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                // Throwing a generic exception to prevent username enumeration attacks.
                throw new InvalidCredentialException("Invalid username or password.");
            }

            return _jwtHelper.GenerateToken(user);
        }

        /// <summary>
        /// Registers a new user after validating the provided information.
        /// </summary>
        /// <param name="request">The registration request DTO containing the new user's details.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous registration operation.</returns>
        /// <exception cref="PasswordMismatchException">Thrown if the password and confirm password fields do not match.</exception>
        /// <exception cref="InvalidEmailException">Thrown if the provided email has an invalid format.</exception>
        /// <exception cref="UserExistsException">Thrown if a user with the provided email already exists.</exception>
        public async Task RegisterAsync(RegisterRequest request)
        {
            if (request.Password != request.ConfirmPassword)
                throw new PasswordMismatchException("Passwords do not match.");

            Regex emailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(request.Email))
                throw new InvalidEmailException("Invalid email format.");

            if (await _userRepository.FindByEmailAsync(request.Email) != null)
                throw new UserExistsException("A user with this email already exists.");

            User newUser = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                IsEducator = false, // Default role
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsFirstLogin = true,
                AuthProvider = null,
                ProviderKey = null,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            await _userRepository.AddAsync(newUser);
        }

        /// <summary>
        /// Authenticates a user via their Google ID token. If the user does not exist, a new user profile is created. 
        /// If a user with the same email exists, the Google account is linked to the existing profile.
        /// </summary>
        /// <param name="idToken">The ID token provided by Google's sign-in service.</param>
        /// <returns>A JWT string for the authenticated or newly created user.</returns>
        /// <exception cref="ArgumentException">Thrown if the idToken is null or empty.</exception>
        /// <exception cref="InvalidCredentialException">Thrown if the Google ID token is invalid or cannot be validated.</exception>
        public async Task<string> LoginWithGoogle(string idToken)
        {
            if (string.IsNullOrEmpty(idToken))
                throw new ArgumentException("Google ID token is required.", nameof(idToken));

            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            }
            catch (InvalidJwtException ex)
            {
                throw new InvalidCredentialException("Invalid Google ID token.", ex);
            }

            User? user = await _userRepository.FindByProviderKey(payload.Subject, "Google");

            if (user == null)
            {
                // Optional: Check if a user with this email already exists to link accounts.
                user = await _userRepository.FindByEmailAsync(payload.Email);
                if (user != null)
                {
                    // User exists, link the Google account to them.
                    user.ProviderKey = payload.Subject;
                    user.AuthProvider = "Google";
                    await _userRepository.Update(user);
                }
                else
                {
                    // User does not exist at all, create a new one.
                    user = new User()
                    {
                        FirstName = payload.GivenName,
                        LastName = payload.FamilyName,
                        Email = payload.Email,
                        IsEducator = false,
                        AuthProvider = "Google",
                        ProviderKey = payload.Subject,
                        // Hash a random password for consistency, as it won't be used for login.
                        Password = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };
                    await _userRepository.AddAsync(user);
                }
            }

            return _jwtHelper.GenerateToken(user);
        }
    }
}