using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
using TodoApp.ViewModel;

namespace TodoApp.Controllers
{
    public class SettingsController : Controller
    {
        private readonly TodoDbContext _context;
        public SettingsController(TodoDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                return RedirectToAction("Login", "User");
            }
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var userViewModel = new UserViewModel
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture
            };
            return View(userViewModel);
        }
    }
}
