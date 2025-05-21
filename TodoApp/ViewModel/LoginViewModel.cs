using System.ComponentModel.DataAnnotations;

namespace TodoApp.ViewModel
{
    public class LoginViewModel
    {
        public string? UserId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
