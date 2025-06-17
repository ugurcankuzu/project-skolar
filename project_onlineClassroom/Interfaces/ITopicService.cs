using project_onlineClassroom.Models;

namespace project_onlineClassroom.Interfaces
{
    public interface ITopicService
    {
        Task<List<Topic>> GetAllTopicsInClassAsync(int classId);
        Task<Topic> GetTopicByIdAsync(int topicId, int classId);
        Task<Topic> AddTopic(Topic topic); // Replace Topic with TopicDTO when DTO is ready
        Task<Topic> UpdateTopicAsync(int topicId, Topic topic); // Replace Topic with TopicDTO when DTO is ready
        Task DeleteTopicAsync(int topicId, int classId);


    }
}
