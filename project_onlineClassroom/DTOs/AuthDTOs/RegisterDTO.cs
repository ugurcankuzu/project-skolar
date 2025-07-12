namespace project_onlineClassroom.DTOs.AuthDTOs
{
    public class RegisterDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public RegisterDTO(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
