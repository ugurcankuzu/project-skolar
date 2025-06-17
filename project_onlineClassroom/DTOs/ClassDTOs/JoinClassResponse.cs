using project_onlineClassroom.Models;

namespace project_onlineClassroom.DTOs.ClassDTOs
{
    public class JoinClassResponse
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public DateTime JoinedAt { get; set; }
        public JoinClassResponse(Participant participant)
        {
            Id = participant.Id;
            ClassId = participant.ClassId;
            JoinedAt = participant.JoinedAt;
        }

    }
}
