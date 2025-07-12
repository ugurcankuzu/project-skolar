using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs.UserDTOs;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;
using System;
using System.Threading.Tasks;

namespace project_onlineClassroom.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) => _userRepository = userRepository;

        /// <summary>
        /// Retrieves a user's profile by their unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the user to retrieve.</param>
        /// <returns>The found <see cref="User"/> entity.</returns>
        /// <exception cref="UserNotFoundException">Propagated from the repository if no user with the specified ID is found.</exception>
        public async Task<User> GetUserProfileByIdAsync(int id)
        {
            // The repository's GetByIdAsync handles the exception throwing.
            // The null-coalescing operator here is redundant and can be removed for clarity.
            return await _userRepository.GetByIdAsync(id);
        }

        /// <summary>
        /// Updates a user's profile information.
        /// </summary>
        /// <param name="id">The unique ID of the user to update.</param>
        /// <param name="profileDTO">A DTO containing the new profile information (FirstName, LastName, Email).</param>
        /// <returns>The updated <see cref="User"/> entity.</returns>
        /// <exception cref="UserNotFoundException">Propagated from the repository if the user to be updated does not exist.</exception>
        /// <exception cref="UserExistsException">Thrown if the new email address is already in use by another user.</exception>
        public async Task<User> UpdateUserProfileAsync(int id, ProfileDTO profileDTO)
        {
            User userToUpdate = await _userRepository.GetByIdAsync(id);

            // Business rule: Check if the new email is already taken by another user.
            if (!string.Equals(userToUpdate.Email, profileDTO.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existingUserWithNewEmail = await _userRepository.FindByEmailAsync(profileDTO.Email);
                if (existingUserWithNewEmail != null)
                {
                    throw new UserExistsException($"The email '{profileDTO.Email}' is already in use.");
                }
            }

            userToUpdate.FirstName = profileDTO.FirstName;
            userToUpdate.LastName = profileDTO.LastName;
            userToUpdate.Email = profileDTO.Email;
            userToUpdate.UpdatedAt = DateTime.UtcNow;

            await _userRepository.Update(userToUpdate);
            return userToUpdate;
        }

        /// <summary>
        /// Deletes a user from the database by their unique ID.
        /// </summary>
        /// <param name="id">The unique ID of the user to delete.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="UserNotFoundException">Propagated from the repository if the user to be deleted does not exist.</exception>
        public async Task DeleteUser(int id)
        {
            User userToDelete = await _userRepository.GetByIdAsync(id);
            await _userRepository.Delete(userToDelete);
        }

        /// <summary>
        /// Retrieves a user's profile by their external provider key (e.g., from Google).
        /// </summary>
        /// <param name="providerKey">The unique identifier from the external provider.</param>
        /// <param name="authProvider">The name of the authentication provider.</param>
        /// <returns>The found <see cref="User"/> entity.</returns>
        /// <exception cref="UserNotFoundException">Thrown if no user is linked to the specified provider key.</exception>
        public async Task<User> GetUserProfileByProviderKey(string providerKey, string authProvider = "Google")
        {
            return await _userRepository.FindByProviderKey(providerKey, authProvider)
                ?? throw new UserNotFoundException($"User with provider '{authProvider}' not found.");
        }

        /// <summary>
        /// Updates a user's status after their first login, setting their role and marking the first login as complete.
        /// </summary>
        /// <param name="id">The unique ID of the user to update.</param>
        /// <param name="isEducator">A boolean indicating whether the user has chosen the educator role.</param>
        /// <returns>The updated <see cref="User"/> entity.</returns>
        /// <exception cref="UserNotFoundException">Propagated from the repository if the user to be updated does not exist.</exception>
        public async Task<User> UpdateUserFirstLoginAsync(int id, bool isEducator)
        {
            User userToUpdate = await _userRepository.GetByIdAsync(id);

            userToUpdate.IsFirstLogin = false;
            userToUpdate.IsEducator = isEducator;
            userToUpdate.UpdatedAt = DateTime.UtcNow;

            await _userRepository.Update(userToUpdate);
            return userToUpdate;
        }
    }
}