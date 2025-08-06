namespace project_onlineClassroom.DTOs.TopicDTO
{
    public class TopicNoteDTO
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string title { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public TopicNoteDTO() { }
        public TopicNoteDTO(int id, int topicId, string title, DateTime createdAt, DateTime updatedAt)
        {
            Id = id;
            TopicId = topicId;
            this.title = title;
            this.createdAt = createdAt;
            this.updatedAt = updatedAt;
        }

    }

}
