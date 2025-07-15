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
        public async Task<SummaryEducatorDTO> GetDashboardSummaryEducatorAsync(int userId)
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
        /// <summary>
        /// Retrieves a summary of the student's dashboard, including the number of classes they are enrolled in.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<SummaryStudentDTO> GetDashboardSummaryStudentAsync(int userId)
        {
            int numberOfClasses = await _classService.GetClassCountByStudentIdAsync(userId);
            int submittedAssignments = 0; // Placeholder, implement logic to get open assignments count
            int incompleteAssignments = 0; // Placeholder, implement logic to get submitted assignments count

            return new SummaryStudentDTO
            {
                IncompleteAssignments = incompleteAssignments,
                SubmittedAssignments = submittedAssignments,
                TotalCourses = numberOfClasses
            };
        }
    }
}
