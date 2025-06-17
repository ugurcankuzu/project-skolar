using project_onlineClassroom.CustomError;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IClassService _classService;
        public TopicService(ITopicRepository topicRepository, IClassService classService)
        {
            _topicRepository = topicRepository;
            _classService = classService;
        }
        private async Task<Class?> IsClassExist(int classId) => await _classService.GetClassByIdAsync(classId);
        public async Task<List<Topic>> GetAllTopicsInClassAsync(int classId)
        {
            Class @class = await IsClassExist(classId) ?? throw new ClassNotFoundException();
            return await _topicRepository.GetAllTopicsInClass(@class.Id);
        }
        public async Task<Topic> GetTopicByIdAsync(int topicId, int classId)
        {
            Class @class = await IsClassExist(classId) ?? throw new ClassNotFoundException();
            Topic topic = await _topicRepository.GetTopicByIdInClass(topicId, @class.Id) ?? throw new Exception("Topic Not Found By Given Id");
            return topic;
        }
        public async Task<Topic> AddTopic(Topic topic)
        {
            Class @class = await IsClassExist(topic.ClassId) ?? throw new ClassNotFoundException();
            return await _topicRepository.CreateTopic(topic);
        }
        public async Task<Topic> UpdateTopicAsync(int topicId, Topic topic)
        {
            Class @class = await IsClassExist(topic.ClassId) ?? throw new ClassNotFoundException();
            Topic updatedTopic = await _topicRepository.UpdateTopic(topicId, topic) ?? throw new Exception("Topic Not Found By Given Id");
            return updatedTopic;
        }
        public async Task DeleteTopicAsync(int topicId, int classId)
        {
            Class @class = await IsClassExist(classId) ?? throw new ClassNotFoundException();
            Topic? topic = await _topicRepository.GetTopicByIdInClass(topicId, @class.Id);
            if (topic == null) throw new Exception("Topic Not Found By Given Id");
            await _topicRepository.DeleteTopic(topic.Id);
        }

    }
}
