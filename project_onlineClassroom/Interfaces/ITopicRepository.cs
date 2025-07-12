using project_onlineClassroom.Models;

namespace project_onlineClassroom.Interfaces
{
    public interface ITopicRepository
    {
        Task<List<Topic>> GetAllTopicsInClass(int classId);
        Task<Topic> GetTopicByIdInClass(int topicId,int classId);
        Task<Topic> CreateTopic(Topic topic); // Replace TopicDTO with Topic when DTO Ready
        Task UpdateTopic(Topic topic); // Replace TopicDTO with Topic when DTO Ready
        Task DeleteTopic(int topicId,int classId);
    }
}
