using System.ComponentModel.DataAnnotations;

namespace project_onlineClassroom.DTOs.ClassDTOs
{
    public class CreateClassRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        [Display(Name = "Class Title")]
        public string Title { get; set; }
        [Required]
        [Range(1, byte.MaxValue, ErrorMessage = "User limit must be between 1 and 255.")]
        [Display(Name = "User Limit")]
        public byte UserLimit { get; set; }

    }
}
