using Microsoft.EntityFrameworkCore;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly AppDbContext _context;
        public TopicRepository(AppDbContext context) => _context = context;

        public async Task<List<Topic>> GetAllTopicsInClass(int classId)
        {
            return await _context.Topics
                .Where(t => t.ClassId == classId)
                .ToListAsync();
        }

        public async Task<Topic?> GetTopicByIdInClass(int topicId, int classId)
        {
            return await _context.Topics
                .Where(t => t.Id == topicId && t.ClassId == classId)
                .FirstOrDefaultAsync();
        }
        public async Task<Topic> CreateTopic(Topic topic)
        {
            _context.Topics.Add(topic);
            await _context.SaveChangesAsync();
            return topic;
        }
        public async Task<Topic?> UpdateTopic(int topicId, Topic topic)
        {
            var existingTopic = await _context.Topics.FindAsync(topicId);
            if (existingTopic == null) return null;
            existingTopic.Title = topic.Title;
            existingTopic.Description = topic.Description;
            existingTopic.ClassId = topic.ClassId;
            _context.Topics.Update(existingTopic);
            await _context.SaveChangesAsync();
            return existingTopic;
        }
        public async Task DeleteTopic(int topicId)
        {
            var topic = await _context.Topics.FindAsync(topicId);
            if (topic != null)
            {
                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync();
            }
        }
    }
}
