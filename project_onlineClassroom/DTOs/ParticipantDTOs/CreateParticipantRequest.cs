namespace project_onlineClassroom.DTOs.ParticipantDTOs
{
    public class CreateParticipantRequest
    {
        public int UserId { get; set; }
        public int ClassId { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
