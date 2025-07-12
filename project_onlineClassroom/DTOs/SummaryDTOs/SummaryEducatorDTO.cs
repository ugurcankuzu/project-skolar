namespace project_onlineClassroom.DTOs.SummaryDTOs
{
    public class SummaryEducatorDTO
    {
        public int UserId { get; set; }
        public int TotalClassrooms { get; set; }
        public int OpenAssignments { get; set; }
        public int SubmittedAssignments { get; set; }

        public SummaryEducatorDTO() { }
        public SummaryEducatorDTO(int userId, int totalClassrooms, int openAssignments, int submittedAssignments)
        {
            UserId = userId;
            TotalClassrooms = totalClassrooms;
            OpenAssignments = openAssignments;
            SubmittedAssignments = submittedAssignments;
        }
    }
}
