namespace project_onlineClassroom.DTOs
{
    public class LoginDTO
    {
        public string AuthToken { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public LoginDTO(string authToken)
        {
            AuthToken = authToken;
            Success = true;
        }
        public LoginDTO(bool success, string message)
        {
            Success = success;
            Message = message;
        }

    }
}
