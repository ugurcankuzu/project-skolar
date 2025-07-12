namespace project_onlineClassroom.DTOs
{
    public class GenericResponse<T> where T : class
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public GenericResponse(bool success, string? message = null, T? data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }
        public GenericResponse(T data) : this(true, null, data) { }
        public GenericResponse(bool success, string message) : this(success, message, null) { }
        public GenericResponse(string message) : this(false, message, null) { }
    }
}
