namespace TodoApp.Models
{
    public class Reminder
    {
        public Guid Id { get; set; }
        public string TodoId { get; set; }
        public string ReminderDescription { get; set; }
        public DateTime? ReminderTime { get; set; }
        public string? NotifiedTo { get; set; }
        public bool? IsNotified { get; set; }
    }

}
