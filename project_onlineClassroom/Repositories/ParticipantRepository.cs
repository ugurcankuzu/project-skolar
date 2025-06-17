using Microsoft.EntityFrameworkCore;
using project_onlineClassroom.DTOs.ParticipantDTOs;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly AppDbContext _context;
        public ParticipantRepository(AppDbContext context) => _context = context;

        public async Task<Participant> CreateParticipantAsync(Participant participant)
        {
            await _context.Participants.AddAsync(participant);
            await _context.SaveChangesAsync();
            return participant;
        }
        public async Task<Participant?> GetParticipantByUserIdAndClassIdAsync(int userId, int classId)
        {
            return await _context.Participants.Where(p => p.UserId == userId && p.ClassId == classId).FirstOrDefaultAsync();

        }
        public async Task<List<Participant>> GetParticipantsInClassAsync(int classId)
        {
            return await _context.Participants.Where(p => p.ClassId == classId).Include(p => p.User).ToListAsync();
        }
        public async Task DeleteParticipantAsync(int userId, int classId)
        {
            await _context.Participants.Where(p => p.UserId == userId && p.ClassId == classId)
                .ExecuteDeleteAsync();
            await _context.SaveChangesAsync();
        }
    }
}
