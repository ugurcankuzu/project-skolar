using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Interfaces
{
    public interface IAuthRepository
    {
        /// <summary>
        /// Logs in a user by verifying their email and password.
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        /// <exception cref="InvalidPasswordException"></exception>
        Task<User> LoginAsync(LoginRequest loginRequest);
        /// <summary>
        /// Registers a new user by creating a new user profile in the database.
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>
        /// <exception cref="UserExistsException"></exception>
        Task<bool> RegisterAsync(RegisterRequest registerRequest);
        /*        Task<User> LoginWithGoogle(GoogleJsonWebSignature.Payload payload);
        */
    }
}
