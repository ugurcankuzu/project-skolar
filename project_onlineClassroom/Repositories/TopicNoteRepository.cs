using Microsoft.EntityFrameworkCore;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Repositories
{
    public class TopicNoteRepository : ITopicNoteRepository
    {
        private readonly AppDbContext _context;
        public TopicNoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TopicNote> AddTopicNoteAsync(TopicNote topicNote)
        {
            await _context.TopicNotes.AddAsync(topicNote);
            await _context.SaveChangesAsync();
            return topicNote;
        }

        public async Task DeleteTopicNoteAsync(int topicNoteId)
        {
            await _context.TopicNotes.Where(tn => tn.Id == topicNoteId).ExecuteDeleteAsync();
        }

        public async Task<TopicNote> GetTopicNoteByIdAsync(int topicNoteId)
        {
            return await _context.TopicNotes.FirstOrDefaultAsync(tn => tn.Id == topicNoteId) ?? throw new Exception($"TopicNote with ID {topicNoteId} not found.");
        }

        public Task<List<TopicNote>> GetTopicNotesByTopicIdAsync(int topicId)
        {
            return _context.TopicNotes.Where(tn => tn.TopicId == topicId)
                .ToListAsync();
        }

        public async Task UpdateTopicNoteAsync(TopicNote topicNote)
        {
            await _context.TopicNotes.Where(tn => tn.Id == topicNote.Id).ExecuteUpdateAsync(tn => tn.SetProperty(t => t.Title, topicNote.Title)
            .SetProperty(t => t.TopicId, topicNote.TopicId));

        }
    }
}
