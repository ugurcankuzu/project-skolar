using project_onlineClassroom.Models;

namespace project_onlineClassroom.DTOs.ClassDTOs
{
    public class UserClassSummaryDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte UserCount { get; set; }
        public byte UserLimit { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string? ImageUrl { get; set; } // Optional image URL for the class
        public string? ImagePublicId { get; set; } // Optional public ID for the image (e.g., from a cloud storage service)

        public UserClassSummaryDTO(Class @class)
        {
            Id = @class.Id;
            Title = @class.Title;
            UserCount = (byte)@class.Participants.Count;
            UserLimit = @class.UserLimit;
            CreatedAt = @class.CreatedAt;
            UpdatedAt = @class.UpdatedAt;
            ImagePublicId = @class.ImagePublicId;
            ImageUrl = @class.ImageUrl;
        }
    }
}
