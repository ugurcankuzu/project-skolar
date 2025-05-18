using project_onlineClassroom.DTOs;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> LoginAsync(LoginRequest loginRequest);
        Task<bool> RegisterAsync(RegisterRequest registerRequest);
    }
}
