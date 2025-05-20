
using project_onlineClassroom.DTOs;

namespace project_onlineClassroom.Interfaces
{
    public interface IAuthService
    {
        Task<string> LoginAsync(LoginRequest loginRequest);
        Task RegisterAsync(RegisterRequest registerRequest);
    }
}
