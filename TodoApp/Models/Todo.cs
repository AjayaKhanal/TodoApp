using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
    public class Todo
    {
        [Key]
        public string TodoId { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        public TodoStatus Status { get; set; }

        // Priority levels
        public TodoPriority Priority { get; set; }

        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string? DeletedBy { get; set; }
        public int? IsDeleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime DueDate { get; set; }
    }
}
