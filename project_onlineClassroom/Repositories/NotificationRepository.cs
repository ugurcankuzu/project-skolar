using Microsoft.EntityFrameworkCore;
using project_onlineClassroom.CustomError; // Assuming NotFoundException is in this namespace
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project_onlineClassroom.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;
        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds a new notification to the database and saves the changes.
        /// </summary>
        /// <param name="notification">The <see cref="Notification"/> entity to be created.</param>
        /// <returns>The created <see cref="Notification"/> entity with its new database-generated ID.</returns>
        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        /// <summary>
        /// Retrieves a list of unread notifications for a specific user, ordered by most recent.
        /// Returns an empty collection if the user has no unread notifications.
        /// </summary>
        /// <param name="userId">The unique ID of the user whose notifications are to be retrieved.</param>
        /// <returns>A <see cref="Task{IEnumerable{Notification}}"/> containing the unread notifications for the user.</returns>
        public async Task<IEnumerable<Notification>> GetNotificationsAsync(int userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.UserId == userId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return notifications;
        }

        /// <summary>
        /// Marks all unread notifications for a specific user as read in a single database operation.
        /// </summary>
        /// <param name="userId">The unique ID of the user whose notifications will be marked as read.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task MarkAllNotificationsAsReadAsync(int userId)
        {
            await _context.Notifications.Where(n => n.UserId == userId && !n.IsRead)
                .ExecuteUpdateAsync(notification => notification.SetProperty(n => n.IsRead, true));
        }

        /// <summary>
        /// Marks a single notification as read for a specific user in a single database operation.
        /// The operation will only succeed if the notification belongs to the specified user.
        /// </summary>
        /// <param name="notificationId">The unique ID of the notification to be marked as read.</param>
        /// <param name="userId">The unique ID of the user who owns the notification.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task MarkNotificationAsReadAsync(int notificationId, int userId)
        {
            await _context.Notifications.Where(n => n.Id == notificationId && n.UserId == userId)
                .ExecuteUpdateAsync(notification => notification.SetProperty(n => n.IsRead, true));
        }

        /// <summary>
        /// Retrieves a specific notification by its ID, but only if it belongs to the specified user.
        /// </summary>
        /// <param name="notificationId">The unique ID of the notification to retrieve.</param>
        /// <param name="userId">The unique ID of the user who is expected to own the notification.</param>
        /// <returns>The found <see cref="Notification"/> entity.</returns>
        /// <exception cref="Exception">Thrown if no notification with the specified ID is found for the given user.</exception>
        public async Task<Notification> GetNotificationByIdAsync(int notificationId, int userId)
        {
            // Using a more specific exception is recommended.
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId)
                ?? throw new Exception($"Notification with ID {notificationId} not found for user with ID {userId}.");
            return notification;
        }
    }
}