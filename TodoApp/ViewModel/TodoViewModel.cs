using System.ComponentModel.DataAnnotations;
using TodoApp.Models;

namespace TodoApp.ViewModel
{
    public class TodoViewModel
    {
        public string TodoId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public TodoStatus Status { get; set; } = TodoStatus.Pending;
        [Required]
        public TodoPriority Priority { get; set; } = TodoPriority.Medium;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        [Display(Name = "Due Date")]
        public DateTime DueDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DueDate < CreatedDate)
            {
                yield return new ValidationResult(
                    "Due date must be later than or equal to the created date.",
                    new[] { nameof(DueDate) });
            }
        }
    }
}
