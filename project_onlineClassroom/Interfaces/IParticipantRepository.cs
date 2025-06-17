using project_onlineClassroom.Models;

namespace project_onlineClassroom.Interfaces
{
    public interface IParticipantRepository
    {
        Task<Participant> CreateParticipantAsync(Participant participant);
        Task<Participant?> GetParticipantByUserIdAndClassIdAsync(int userId, int classId);
        Task<List<Participant>> GetParticipantsInClassAsync(int classId);
        Task DeleteParticipantAsync(int userId, int classId);
    }
}
