using project_onlineClassroom.DTOs.SummaryDTOs;

namespace project_onlineClassroom.Interfaces
{
    public interface ISummaryService
    {
        Task<SummaryEducatorDTO> GetDashboardSummaryEducatorAsync(int userId);
        Task<SummaryStudentDTO> GetDashboardSummaryStudentAsync(int userId);
    }
}
