using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
    public class Notification
    {
        [Key]
        public Guid NotificationId { get; set; }

        public string Message { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }
    }

}
