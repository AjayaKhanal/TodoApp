using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
using TodoApp.ViewModel;

namespace TodoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateTodo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodo(TodoViewModel todoViewModel)
        {
            // Simulate a delay for the sake of the example
            await Task.Delay(1000);
            // Redirect to the Index action after creating a todo
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
