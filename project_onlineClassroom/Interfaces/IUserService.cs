using project_onlineClassroom.DTOs.UserDTOs;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserProfileByIdAsync(int id);

    }
}
