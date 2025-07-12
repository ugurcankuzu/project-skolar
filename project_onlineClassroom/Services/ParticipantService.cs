using project_onlineClassroom.CustomError;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;
using System;
using System.Threading.Tasks;

namespace project_onlineClassroom.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;
        public ParticipantService(IParticipantRepository participantRepository)
        {
            _participantRepository = participantRepository;
        }

        /// <summary>
        /// Creates a new participant record, effectively adding a user to a class.
        /// This method does not perform any business logic checks (e.g., if the user is already a participant);
        /// such checks are the responsibility of the calling service.
        /// </summary>
        /// <param name="userId">The unique ID of the user to be added as a participant.</param>
        /// <param name="classId">The unique ID of the class the user is joining.</param>
        /// <returns>The newly created <see cref="Participant"/> entity.</returns>
        public async Task<Participant> CreateParticipantAsync(int userId, int classId)
        {
            Participant participant = new Participant
            {
                UserId = userId,
                ClassId = classId,
                JoinedAt = DateTime.UtcNow
            };
            Participant createdParticipant = await _participantRepository.CreateParticipantAsync(participant);
            return createdParticipant;
        }

        /// <summary>
        /// Finds a participant record by user ID and class ID.
        /// This method is designed to be called by other services to check for a user's membership in a class.
        /// </summary>
        /// <param name="userId">The unique ID of the user.</param>
        /// <param name="classId">The unique ID of the class.</param>
        /// <returns>A <see cref="Task{Participant}"/> containing the found participant, or <c>null</c> if the user is not a participant in the class.</returns>
        public async Task<Participant?> FindParticipantByUserIdAsync(int userId, int classId)
        {
            // This service method acts as a pass-through to the repository's 'Find' method.
            // It intentionally returns null if not found, allowing the calling service to handle the logic.
            return await _participantRepository.FindParticipantAsync(userId, classId);
        }

        /// <summary>
        /// Deletes a participant from a class.
        /// This method does not verify the existence of the participant before attempting deletion.
        /// It is expected that the calling service (e.g., ClassService) performs this validation.
        /// </summary>
        /// <param name="userId">The unique ID of the user to be removed.</param>
        /// <param name="classId">The unique ID of the class from which the user will be removed.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task DeleteParticipantFromClassByUserIdAsync(int userId, int classId)
        {
            // This is a "fire-and-forget" style command. The repository's ExecuteDeleteAsync is efficient
            // and does not throw an error if the record does not exist.
            await _participantRepository.DeleteParticipantAsync(userId, classId);
        }
    }
}