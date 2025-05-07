using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApp.Models
{
    public class UserLoginHistory
    {
        [Key]
        public Guid UserHistoryId { get; set; }
        [Required]
        public string UserId { get; set; }

        // Navigation property
        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public DateTime LoginTime { get; set; }

        public string? LoginBrowser { get; set; }

        public string? LoginDevice { get; set; }

        public string? LoginIP { get; set; }

        public string? Location { get; set; } // Optional if you're using geolocation from IP

        public bool IsSuccessful { get; set; } = true;

        public string? FailureReason { get; set; } // If login fails
    }
}
