namespace project_onlineClassroom.DTOs.ClassDTOs
{
    public class ClassResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; } = null;
        public T? Data { get; set; }
        public ClassResponse() { }
        public ClassResponse(T data)
        {
            Success = true;
            Data = data;
        }
        public ClassResponse(bool success, string message)
        {
            Success = success; Message = message;
        }
    }
}
