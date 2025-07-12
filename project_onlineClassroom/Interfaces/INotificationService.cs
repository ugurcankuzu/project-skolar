using project_onlineClassroom.Models;

namespace project_onlineClassroom.Interfaces
{
    public interface INotificationService
    {
        Task<Notification> CreateNotificationAsync(Notification notification);
        /// <summary>
        /// Retrieves notifications for the user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A list of notifications.</returns>
        Task<IEnumerable<Notification>> GetNotificationsAsync(int userId);
        /// <summary>
        /// Marks a notification as read.
        /// </summary>
        /// <param name="notificationId">The ID of the notification to mark as read.</param>
        /// <param name="currentUserId"> The ID of the current user.</param>
        /// <returns>A boolean indicating success or failure.</returns>
        Task MarkNotificationAsReadAsync(int notificationId, int currentUserId);
        /// <summary>
        /// Marks all notifications as read for the user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A boolean indicating success or failure.</returns>
        Task MarkAllNotificationsAsReadAsync(int userId);
    }
}
