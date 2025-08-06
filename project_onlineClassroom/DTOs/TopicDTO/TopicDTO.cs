using project_onlineClassroom.Models;

namespace project_onlineClassroom.DTOs.TopicDTO
{
    public class TopicDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IEnumerable<TopicNoteDTO> topicNotes { get; set; } = new List<TopicNoteDTO>();
        public TopicDTO() { }
        public TopicDTO(Topic topic)
        {
            Id = topic.Id;
            Title = topic.Title;
            Description = topic.Description;
            CreatedAt = topic.CreatedAt;
            UpdatedAt = topic.UpdatedAt;
            topicNotes = topic.TopicNotes.Select(note => new TopicNoteDTO
            {
                Id = note.Id,
                TopicId = note.TopicId,
                title = note.Title,
                createdAt = note.CreatedAt,
                updatedAt = note.UpdatedAt
            });
        }
    }
}
