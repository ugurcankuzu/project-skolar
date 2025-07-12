using Microsoft.EntityFrameworkCore;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project_onlineClassroom.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly AppDbContext _context;
        public ParticipantRepository(AppDbContext context) => _context = context;

        /// <summary>
        /// Adds a new participant record to the database and saves the changes.
        /// </summary>
        /// <param name="participant">The <see cref="Participant"/> entity to be created.</param>
        /// <returns>The created <see cref="Participant"/> entity with its new database-generated ID.</returns>
        public async Task<Participant> CreateParticipantAsync(Participant participant)
        {
            await _context.Participants.AddAsync(participant);
            await _context.SaveChangesAsync();
            return participant;
        }

        /// <summary>
        /// Finds a specific participant record based on the user ID and class ID.
        /// </summary>
        /// <param name="userId">The unique ID of the user.</param>
        /// <param name="classId">The unique ID of the class.</param>
        /// <returns>A <see cref="Task{Participant}"/> containing the found participant, or <c>null</c> if no matching record is found.</returns>
        public async Task<Participant?> FindParticipantAsync(int userId, int classId)
        {
            return await _context.Participants
                .Where(p => p.UserId == userId && p.ClassId == classId)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Retrieves a list of all participants in a specific class, including their related User data.
        /// Returns an empty list if the class has no participants.
        /// </summary>
        /// <param name="classId">The unique ID of the class.</param>
        /// <returns>A <see cref="Task{List{Participant}}"/> containing the participants of the class.</returns>
        public async Task<List<Participant>> GetParticipantsInClassAsync(int classId)
        {
            return await _context.Participants
                .Where(p => p.ClassId == classId)
                .Include(p => p.User)
                .ToListAsync();
        }

        /// <summary>
        /// Deletes a participant record from the database in a single operation without loading the entity.
        /// </summary>
        /// <param name="userId">The unique ID of the user to be removed.</param>
        /// <param name="classId">The unique ID of the class from which the user will be removed.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task DeleteParticipantAsync(int userId, int classId)
        {
            await _context.Participants
                .Where(p => p.UserId == userId && p.ClassId == classId)
                .ExecuteDeleteAsync();
        }
    }
}