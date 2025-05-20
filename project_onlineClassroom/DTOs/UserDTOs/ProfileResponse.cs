namespace project_onlineClassroom.DTOs.UserDTOs
{
    public class ProfileResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; } = null;
        public ProfileDTO? Data { get; set; } = null;


        public ProfileResponse(ProfileDTO profile)
        {
            Success = true;
            Data = profile;
        }
        public ProfileResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
