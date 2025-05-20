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

        public async Task<User> GetUserProfileByIdAsync(int id)
        {
            User user = await _userRepository.GetByIdAsync(id) ?? throw new UserNotFoundException();
            return user;
        }
    }
}
