using project_onlineClassroom.CustomError;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project_onlineClassroom.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserService _userService;

        public NotificationService(INotificationRepository notificationRepository, IUserService userService)
        {
            _notificationRepository = notificationRepository;
            _userService = userService;
        }

        /// <summary>
        /// Retrieves all unread notifications for a specific user after verifying the user's existence.
        /// </summary>
        /// <param name="userId">The unique ID of the user whose notifications are to be retrieved.</param>
        /// <returns>A <see cref="Task{IEnumerable{Notification}}"/> containing the user's unread notifications. Returns an empty collection if none are found.</returns>
        /// <exception cref="UserNotFoundException">Propagated from the user service if the specified user does not exist.</exception>
        public async Task<IEnumerable<Notification>> GetNotificationsAsync(int userId)
        {
            // First, validate that the user exists.
            await _userService.GetUserProfileByIdAsync(userId);
            return await _notificationRepository.GetNotificationsAsync(userId);
        }

        /// <summary>
        /// Marks all unread notifications for a specific user as read after verifying the user's existence.
        /// </summary>
        /// <param name="userId">The unique ID of the user whose notifications will be marked as read.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="UserNotFoundException">Propagated from the user service if the specified user does not exist.</exception>
        public async Task MarkAllNotificationsAsReadAsync(int userId)
        {
            // First, validate that the user exists.
            await _userService.GetUserProfileByIdAsync(userId);
            await _notificationRepository.MarkAllNotificationsAsReadAsync(userId);
        }

        /// <summary>
        /// Marks a single notification as read after verifying its existence and ownership.
        /// Does nothing if the notification is already read.
        /// </summary>
        /// <param name="notificationId">The unique ID of the notification to mark as read.</param>
        /// <param name="currentUserId">The unique ID of the user performing the action, to verify ownership.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="NotFoundException">Propagated from the repository if no matching notification is found for the user.</exception>
        public async Task MarkNotificationAsReadAsync(int notificationId, int currentUserId)
        {
            // "Get-Then-Update" pattern: First, get the notification to ensure it exists and belongs to the current user.
            Notification notification = await _notificationRepository.GetNotificationByIdAsync(notificationId, currentUserId);

            // If already read, no action is needed.
            if (notification.IsRead) return;

            // This is slightly less efficient than a direct ExecuteUpdate but works.
            // A direct call to _notificationRepository.MarkNotificationAsReadAsync would be more performant.
            await _notificationRepository.MarkNotificationAsReadAsync(notification.Id, currentUserId);
        }

        /// <summary>
        /// Creates a new notification record in the database.
        /// </summary>
        /// <param name="notification">The <see cref="Notification"/> entity to be created. Its UserId must be valid.</param>
        /// <returns>The newly created <see cref="Notification"/> entity.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the provided notification object is null.</exception>
        /// <exception cref="UserNotFoundException">Propagated from the user service if the user specified in the notification's UserId does not exist.</exception>
        async Task<Notification> INotificationService.CreateNotificationAsync(Notification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification), "Notification object cannot be null");
            }
            // Validate that the recipient user exists.
            await _userService.GetUserProfileByIdAsync(notification.UserId);

            return await _notificationRepository.CreateNotificationAsync(notification);
        }
    }
}