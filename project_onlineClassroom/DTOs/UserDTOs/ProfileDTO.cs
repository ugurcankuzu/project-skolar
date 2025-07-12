using project_onlineClassroom.Models;

namespace project_onlineClassroom.DTOs.UserDTOs
{
    public class ProfileDTO
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsEducator { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsFirstLogin { get; set; } = true;

        public ProfileDTO() { }
        public ProfileDTO(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            IsFirstLogin = user.IsFirstLogin;
            IsEducator = user.IsEducator;
            CreatedAt = user.CreatedAt;
            UpdatedAt = user.UpdatedAt;
        }
    }
}
