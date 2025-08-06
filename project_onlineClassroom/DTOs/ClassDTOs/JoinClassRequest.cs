using System.ComponentModel.DataAnnotations;

namespace project_onlineClassroom.DTOs.ClassDTOs
{
    public class JoinClassRequest
    {
        [Required]
        [StringLength(6, ErrorMessage = "Join code must be exactly 6 characters long.")]
        public string JoinCode { get; set; } = null!; // Unique code for joining the class
    }
}
