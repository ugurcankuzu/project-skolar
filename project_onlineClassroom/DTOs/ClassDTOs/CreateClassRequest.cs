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
        [Required]
        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        [Display(Name = "Class Description")]
        public string Description { get; set; }
        [Url(ErrorMessage = "Invalid URL format.")]
        [Display(Name = "Image URL")]
        
        public string? ImageUrl { get; set; }
        [Display(Name = "Image Public ID")]
        public string? ImagePublicId { get; set; }
        [Display(Name = "Image File")]
        public IFormFile? ImageFile { get; set; } // Optional file upload for the class image

    }
}
