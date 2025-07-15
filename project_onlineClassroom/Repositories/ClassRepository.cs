using Microsoft.EntityFrameworkCore;
using project_onlineClassroom.CustomError;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly AppDbContext _context;
        public ClassRepository(AppDbContext context) => _context = context;

        /// <summary>
        /// Retrieves a specific class by its unique ID, with options to include related entities.
        /// </summary>
        /// <param name="id">The unique ID of the class to retrieve.</param>
        /// <param name="includeParticipants">A boolean indicating whether to include the list of participants in the result.</param>
        /// <param name="includeOwner">A boolean indicating whether to include the owner (educator) of the class in the result.</param>
        /// <returns>The found <see cref="Class"/> entity.</returns>
        /// <exception cref="ClassNotFoundException">Thrown if no class with the specified ID is found.</exception>
        public async Task<Class> GetClassByIdAsync(int id, bool includeParticipants = false, bool includeOwner = false)
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
            Class? @class = await query.FirstOrDefaultAsync(c => c.Id == id) ?? throw new ClassNotFoundException($"Class with ID {id} not found.");

            return @class;
        }

        /// <summary>
        /// Retrieves a list of all classes owned by a specific educator.
        /// Returns an empty list if the educator has no classes.
        /// </summary>
        /// <param name="educatorId">The unique ID of the educator.</param>
        /// <returns>A <see cref="Task{List{Class}}"/> containing the classes owned by the educator.</returns>
        public async Task<List<Class>> GetClassesByEducatorIdAsync(int educatorId)
        {
            return await _context.Classes
                .Where(c => c.OwnerId == educatorId)
                .Include(c => c.Participants) // Note: This includes participants by default.
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new class to the database and saves the changes.
        /// </summary>
        /// <param name="class">The <see cref="Class"/> entity to be created. The ID will be set by the database.</param>
        /// <returns>The created <see cref="Class"/> entity with its new database-generated ID.</returns>
        public async Task<Class> CreateClassAsync(Class @class)
        {
            await _context.Classes.AddAsync(@class);
            await _context.SaveChangesAsync();
            return @class;
        }
        /// <summary>
        /// Retrieves the total number of classes owned by a specific educator.
        /// </summary>
        /// <param name="educatorId"></param>
        /// <returns></returns>
        public Task<int> GetNumberOfClassesByEducatorIdAsync(int educatorId)
        {
            return _context.Classes
                .CountAsync(c => c.OwnerId == educatorId);
        }
        /// <summary>
        /// Retrieves the total number of classes that a specific student is enrolled in.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<int> GetNumberOfClassesByStudentIdAsync(int userId)
        {
            return _context.Classes.Where(c => c.Participants.Any(p => p.UserId == userId))
                .CountAsync();
        }

        public Task<List<Class>> GetClassesByStudentIdAsync(int id)
        {
            return _context.Classes.Where(c => c.Participants.Any(p => p.UserId == id)).ToListAsync();
        }
    }
}