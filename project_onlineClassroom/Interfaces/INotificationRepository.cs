using project_onlineClassroom.Models;

namespace project_onlineClassroom.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetNotificationsAsync(int userId);
        Task<Notification> GetNotificationByIdAsync(int notificationId, int userId);
        Task<Notification> CreateNotificationAsync(Notification notification);
        Task MarkNotificationAsReadAsync(int notificationId, int userId);
        Task MarkAllNotificationsAsReadAsync(int userId);
    }
}
