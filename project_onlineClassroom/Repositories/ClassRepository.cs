using Microsoft.EntityFrameworkCore;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly AppDbContext _context;
        public ClassRepository(AppDbContext context) => _context = context;

        public async Task<Class?> GetClassByIdAsync(int id, bool includeParticipants = false, bool includeOwner = false)
        {
            IQueryable<Class> query = _context.Classes.AsQueryable();

            if (includeParticipants)
            {
                query = query.Include(c => c.Participants);
            }
            if (includeOwner)
            {
                query = query.Include(c => c.Owner);
            }
            Class? @class = await query.FirstOrDefaultAsync(c => c.Id == id);

            return @class;
        }
        public async Task<List<Class>> GetClassesByEducatorIdAsync(int educatorId)
        {
            return await _context.Classes
                .Where(c => c.OwnerId == educatorId)
                .Include(c => c.Participants)
                .ToListAsync();
        }
        public async Task<Class> CreateClassAsync(Class @class)
        {
            await _context.Classes.AddAsync(@class);
            await _context.SaveChangesAsync();
            return @class;
        }
    }
}
