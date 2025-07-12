using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using project_onlineClassroom.CustomError;
using project_onlineClassroom.DTOs;
using project_onlineClassroom.DTOs.NotificationDTOs;
using project_onlineClassroom.Interfaces;
using project_onlineClassroom.Models;

namespace project_onlineClassroom.Controllers
{
    public class NotificationController : ControllerBase
    {

        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService) => _notificationService = notificationService;

        [HttpGet("notifications")]
        [Authorize]
        [ProducesResponseType(typeof(GenericResponse<IEnumerable<NotificationDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNotifications()
        {
            try
            {
                string? userId = User.FindFirstValue("id");
                Console.WriteLine("User ID from token: " + userId);
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                {
                    throw new InvalidTokenException();
                }
                // Simulate fetching notifications from a service
                IEnumerable<Notification> notifications = await _notificationService.GetNotificationsAsync(id);
                //Convert to IEnumerable<NotificationDTO>
                IEnumerable<NotificationDTO> notificationDTOs = notifications.Select(n => new NotificationDTO
                {
                    Id = n.Id,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt,
                    IsRead = n.IsRead,
                    Link = n.Link
                });
                return Ok(new GenericResponse<IEnumerable<NotificationDTO>>(notificationDTOs));
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new GenericResponse<object>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse<object>(ex.Message));
            }
        }
        [HttpPost("notifications/mark-as-read/{notificationId}")]
        [Authorize]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MarkNotificationAsRead([FromRoute] int notificationId)
        {
            try
            {
                string? userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                {
                    throw new InvalidTokenException();
                }                // Simulate marking notification as read in a service
                await _notificationService.MarkNotificationAsReadAsync(notificationId, id);
                return Ok(new GenericResponse<object>(true, "Notification marked as read successfully."));
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new GenericResponse<object>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse<object>(ex.Message));
            }
        }
        [HttpPost("notifications/mark-as-read")]
        [Authorize]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(GenericResponse<object>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MarkAllNotificationsAsRead()
        {
            try
            {
                string? userId = User.FindFirstValue("id");
                if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int id))
                {
                    throw new InvalidTokenException();
                }                // Simulate marking all notifications as read in a service
                await _notificationService.MarkAllNotificationsAsReadAsync(id);
                return Ok(new GenericResponse<object>(true, "All notifications marked as read"));
            }
            catch (InvalidTokenException ex)
            {
                return Unauthorized(new GenericResponse<object>(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse<object>(ex.Message));
            }
        }
    }
}
