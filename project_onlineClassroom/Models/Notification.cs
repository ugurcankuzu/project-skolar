using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_onlineClassroom.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; } // Unique identifier for the notification
        [ForeignKey("UserId")]
        public User User { get; set; } = null!; // Navigation property to the User who will receive the notification
        [Required]
        public int UserId { get; set; } // The ID of the user who will receive the notification
        public string Message { get; set; } // The content of the notification message
        public DateTime CreatedAt { get; set; } // The date and time when the notification was created
        public bool IsRead { get; set; } // Indicates whether the notification has been read by the user
        public string? Link { get; set; } // Optional link associated with the notification, can be null
    }
}
