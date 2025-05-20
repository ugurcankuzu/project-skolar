using System.ComponentModel.DataAnnotations;

namespace project_onlineClassroom.DTOs
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is Required.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is Required.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "First Name is Required.")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is Required.")]
        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters.")]
        public string LastName { get; set; }
        public bool IsEducator { get; set; } = false;
        public RegisterRequest(string email, string password, string confirmPassword, string firstName, string lastName, bool isEducator)
        {
            Email = email;
            Password = password;
            ConfirmPassword = confirmPassword;
            FirstName = firstName;
            LastName = lastName;
            IsEducator = isEducator;

        }
    }
}
