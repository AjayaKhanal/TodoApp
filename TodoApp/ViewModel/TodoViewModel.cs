using System.ComponentModel.DataAnnotations;
using TodoApp.Models;

namespace TodoApp.ViewModel
{
    public class TodoViewModel
    {
        public string TodoId { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        public TodoStatus Status { get; set; }
        public TodoPriority Priority { get; set; } = TodoPriority.Medium;
    }
}
