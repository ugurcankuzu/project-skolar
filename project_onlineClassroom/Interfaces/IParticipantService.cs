using project_onlineClassroom.DTOs.ParticipantDTOs;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Interfaces
{
    public interface IParticipantService
    {
        /// <summary>
        /// Creates a new participant in a class. Participant existance in a class must be checked caller service.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="classId"></param>
        /// <returns><see cref="Task{Participant}"/></returns>
        Task<Participant> CreateParticipantAsync(int userId, int classId);
        /// <summary>
        /// Returns a participant in a class by user ID and class ID. If the participant does not exist, returns null.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="classId"></param>
        /// <returns><see cref="Task{Participant?}"/></returns>
        Task<Participant?> GetParticipantInClassByUserIdAsync(int userId, int classId);
        Task DeleteParticipantFromClassByUserIdAsync(int userId, int classId);
    }
}

//TODO
/*
    1-> DeleteParticipantAsync
 */