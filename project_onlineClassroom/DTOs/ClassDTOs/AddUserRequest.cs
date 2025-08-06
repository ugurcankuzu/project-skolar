using System.ComponentModel.DataAnnotations;

namespace project_onlineClassroom.DTOs.ClassDTOs
{
    public class AddUserRequest
    {
                [Required]
        [EmailAddress]
        [StringLength(100, ErrorMessage = "Email length can't be more than 100 characters.")]
        [Display(Name = "Student Email")]
        public string email { get; set; } = "user@example.com";
    }
}
