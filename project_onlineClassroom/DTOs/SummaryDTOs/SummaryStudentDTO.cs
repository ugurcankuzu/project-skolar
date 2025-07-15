namespace project_onlineClassroom.DTOs.SummaryDTOs
{
    public class SummaryStudentDTO
    {
        public int TotalCourses { get; set; }
        public int SubmittedAssignments { get; set; }
        public int IncompleteAssignments { get; set; }

        public SummaryStudentDTO() { }
        public SummaryStudentDTO(int totalCourses, int submittedAssignments, int assignmentsNotSubmitted)
        {
            TotalCourses = totalCourses;
            SubmittedAssignments = submittedAssignments;
            IncompleteAssignments = assignmentsNotSubmitted;
        }
    }
}
