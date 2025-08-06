using project_onlineClassroom.Migrations;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.DTOs.ClassDTOs
{
    public class ClassDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte UserLimit { get; set; }
        public byte UserCount { get; set; }
        public string? Owner { get; set; } = null;
        public string JoinCode { get; set; } = null!; // Unique code for joining the class
        //Participant entity model will be replaced by ParticipantDTO when it's ready.
        public List<Participant>? Participants { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? imageUrl { get; set; } = null;
        public string? imagePublicId { get; set; } = null;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ClassDTO(Class @class)
        {
            Id = @class.Id;
            Title = @class.Title;
            Description = @class.Description;
            UserLimit = @class.UserLimit;
            JoinCode = @class.JoinCode;
            UserCount = (byte)@class.Participants.Count;
            CreatedAt = @class.CreatedAt;
            UpdatedAt = @class.UpdatedAt;
            if (@class.Owner != null) Owner = $"{@class.Owner.FirstName} {@class.Owner.LastName}";
            if (@class.Participants != null)
            {
                Participants = [..@class.Participants.Select(p => new Participant
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    ClassId = p.ClassId,
                    JoinedAt = p.JoinedAt,
                })];
            }
            if (@class.ImagePublicId != null)
            {
                imagePublicId = @class.ImagePublicId;
            }
            if (@class.ImageUrl != null)
            {
                imageUrl = @class.ImageUrl;
            }
        }
    }

}
