using System.ComponentModel.DataAnnotations;

namespace project_onlineClassroom.DTOs.AuthDTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public LoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
