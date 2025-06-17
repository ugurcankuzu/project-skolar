using project_onlineClassroom.Models;

namespace project_onlineClassroom.Interfaces
{
    public interface IClassRepository
    {
        Task<Class?> GetClassByIdAsync(int id, bool includeParticipants = false, bool includeOwner = false);
        Task<List<Class>> GetClassesByEducatorIdAsync(int educatorId);
        Task<Class> CreateClassAsync(Class @class);
    }
}

