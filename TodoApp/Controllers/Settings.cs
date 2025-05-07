using Microsoft.AspNetCore.Mvc;

namespace TodoApp.Controllers
{
    public class Settings : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
