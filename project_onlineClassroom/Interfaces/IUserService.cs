using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs.UserDTOs;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Creates a new user profile and saves it to the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        Task<User> GetUserProfileByIdAsync(int id);
        /// <summary>
        /// Updates an existing user profile with the provided details and saves it to the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="profileDTO"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        Task<User> UpdateUserProfileAsync(int id, ProfileDTO profileDTO);
        /// <summary>
        /// Deletes a user profile by its ID. If the user does not exist, throws a <see cref="UserNotFoundException"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        Task DeleteUser(int id);
        Task<User> GetUserProfileByProviderKey(string providerKey, string authProvider = "Google");
    }
}
