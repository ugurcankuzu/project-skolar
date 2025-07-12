namespace project_onlineClassroom.DTOs.NotificationDTOs
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string? Link { get; set; } = null;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public NotificationDTO() { }
        public NotificationDTO(int id, string title, string message, DateTime createdAt, bool isRead, string? link)
        {
            Id = id;
            Title = title;
            Message = message;
            CreatedAt = createdAt;
            IsRead = isRead;
            Link = link;
        }
    }
}
