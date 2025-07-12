using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs.AuthDTOs;

namespace project_onlineClassroom.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Logs in a user by verifying their credentials and generating a JWT token.
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns><see cref="string"/></returns>
        Task<string> LoginAsync(LoginRequest loginRequest);
        /// <summary>
        /// Registers a new user by validating the input and creating a new user profile in the database.
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        /// <exception cref="PasswordMismatchException"></exception>
        /// <exception cref="InvalidEmailException"></exception>
        /// <exception cref="UserExistsException"></exception>
        Task RegisterAsync(RegisterRequest registerRequest);
        Task<string> LoginWithGoogle(string idToken);

    }
}
