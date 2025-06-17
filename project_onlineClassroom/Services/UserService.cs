using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs.UserDTOs;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository) => _userRepository = userRepository;

        /// <summary>
        /// Creates a new user profile and saves it to the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task<User> GetUserProfileByIdAsync(int id)
        {
            User user = await _userRepository.GetByIdAsync(id) ?? throw new UserNotFoundException();
            return user;
        }
        /// <summary>
        /// Updates an existing user profile with the provided details and saves it to the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="profileDTO"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task<User> UpdateUserProfileAsync(int id, ProfileDTO profileDTO)
        {
            User user = await _userRepository.GetByIdAsync(id) ?? throw new UserNotFoundException();
            user.FirstName = profileDTO.FirstName;
            user.LastName = profileDTO.LastName;
            user.Email = profileDTO.Email;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.Update(user);
            return user;
        }
        /// <summary>
        /// Deletes a user profile by its ID. If the user does not exist, throws a <see cref="UserNotFoundException"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        public async Task DeleteUser(int id)
        {
            User user = await _userRepository.GetByIdAsync(id) ?? throw new UserNotFoundException();
            await _userRepository.Delete(user);
        }
        public async Task<User> GetUserProfileByProviderKey(string providerKey, string authProvider = "Google")
        {
            User? user = await _userRepository.GetByProviderKey(providerKey, authProvider) ?? throw new UserNotFoundException();
            return user;
        }
    }
}
