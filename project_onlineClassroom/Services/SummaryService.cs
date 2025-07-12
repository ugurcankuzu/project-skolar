using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs.SummaryDTOs;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Services
{
    public class SummaryService : ISummaryService
    {
        private readonly IClassService _classService;
        public SummaryService(IClassService classService)
        {
            _classService = classService;
        }

        /// <summary>
        /// Retrieves a summary of the educator's dashboard, including the number of classes, open assignments, and submitted assignments.
        /// </summary>
        /// <param name="userId"></param>
        /// <exception cref="UserNotFoundException"></exception>
        /// <exception cref="RoleMismatchForThisActionException"></exception>
        /// <returns></returns>
        public async Task<SummaryEducatorDTO> GetDashboardSummaryAsync(int userId)
        {
            //Get Number of Classes
            int numberOfClasses = await _classService.GetClassCountByEducatorIdAsync(userId);
            //Get Num of Open Assignments
            int numOfOpenAssignments = 0; // Placeholder, implement logic to get open assignments count
            //Num of Submitted Assignments
            int numOfSubmittedAssignments = 0; // Placeholder, implement logic to get submitted assignments count

            return new SummaryEducatorDTO
            {
                UserId = userId,
                TotalClassrooms = numberOfClasses,
                OpenAssignments = numOfOpenAssignments,
                SubmittedAssignments = numOfSubmittedAssignments,
            };
        }
    }
}
