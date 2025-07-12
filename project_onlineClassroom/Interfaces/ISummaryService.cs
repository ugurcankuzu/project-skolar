using project_onlineClassroom.DTOs.SummaryDTOs;

namespace project_onlineClassroom.Interfaces
{
    public interface ISummaryService
    {
        Task<SummaryEducatorDTO> GetDashboardSummaryAsync(int userId);
    }
}
