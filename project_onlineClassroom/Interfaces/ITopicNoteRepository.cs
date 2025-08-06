using project_onlineClassroom.Models;

namespace project_onlineClassroom.Interfaces
{
    public interface ITopicNoteRepository
    {
        Task<List<TopicNote>> GetTopicNotesByTopicIdAsync(int topicId);
        Task<TopicNote> GetTopicNoteByIdAsync(int topicNoteId);
        Task<TopicNote> AddTopicNoteAsync(TopicNote topicNote);
        Task UpdateTopicNoteAsync(TopicNote topicNote);
        Task DeleteTopicNoteAsync(int topicNoteId);
    }
}
