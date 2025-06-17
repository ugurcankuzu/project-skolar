using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs.ParticipantDTOs;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;

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
        /// Creates a new participant in a class. Participant existance in a class must be checked caller service.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="classId"></param>
        /// <returns><see cref="Task{Participant}"/></returns>
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
        /// Returns a participant in a class by user ID and class ID. If the participant does not exist, returns null.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="classId"></param>
        /// <returns><see cref="Task{Participant?}"/></returns>
        public async Task<Participant?> GetParticipantInClassByUserIdAsync(int userId, int classId)
        {
            Participant? participant = await _participantRepository.GetParticipantByUserIdAndClassIdAsync(userId, classId);
            return participant;
        }
        public async Task DeleteParticipantFromClassByUserIdAsync(int userId, int classId)
        {
            await _participantRepository.DeleteParticipantAsync(userId, classId);
        }
    }
}
