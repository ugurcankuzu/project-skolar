namespace project_onlineClassroom.DTOs.ParticipantDTOs
{
    public class ParticipantDTO
    {
        public int Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public DateTime JoinedAt { get; set; }

        public ParticipantDTO() { }
        public ParticipantDTO(int id, string firstName, string lastName, string email, DateTime joinedAt)
        {
            Id = id;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            JoinedAt = joinedAt;
        }
    }
}
