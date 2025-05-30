using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using TodoApp.Migrations;
using TodoApp.Models;
using TodoApp.ViewModel;

namespace TodoApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TodoDbContext _context;

        public HomeController(ILogger<HomeController> logger, TodoDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(string? searchInput, string? statusInput, string? priorityInput)
        {
            var todos = await _context.Todos.Where(t => t.IsDeleted == 0).ToListAsync();
            var todoViewList = new List<TodoViewModel>();

            if (searchInput != null)
            {
                ViewData["searchInput"] = searchInput;
                todos = todos.Where(t => t.Title.Contains(searchInput, StringComparison.OrdinalIgnoreCase)).ToList();
                todoViewList = todos.Select(todo => new TodoViewModel {
                    TodoId = todo.TodoId.ToString(),
                    Title = todo.Title,
                    Status = todo.Status,
                    Priority = todo.Priority,
                    DueDate = todo.DueDate,
                    RemainingTime = todo.DueDate - DateTime.Now
                }).ToList();
                return View(todoViewList);
            }

            if (statusInput != null && statusInput!="All")
            {
                ViewData["statusInput"] = statusInput;
                todos = todos.Where(t => t.Status.ToString() == statusInput).ToList();
                todoViewList = todos.Select(todo => new TodoViewModel
                {
                    TodoId = todo.TodoId.ToString(),
                    Title = todo.Title,
                    Status = todo.Status,
                    Priority = todo.Priority,
                    DueDate = todo.DueDate,
                    RemainingTime = todo.DueDate - DateTime.Now
                }).ToList();
                return View(todoViewList);
            }

            if (priorityInput != null && priorityInput!="All")
            {
                ViewData["priorityInput"] = priorityInput;
                todos = todos.Where(t => t.Priority.ToString() == priorityInput).ToList();
                todoViewList = todos.Select(todo => new TodoViewModel
                {
                    TodoId = todo.TodoId.ToString(),
                    Title = todo.Title,
                    Status = todo.Status,
                    Priority = todo.Priority,
                    DueDate = todo.DueDate,
                    RemainingTime = todo.DueDate - DateTime.Now
                }).ToList();
                return View(todoViewList);
            }

            if (todos != null)
            {
                todoViewList = todos.Select(todo => new TodoViewModel
                {
                    TodoId = todo.TodoId.ToString(),
                    Title = todo.Title,
                    Status = todo.Status,
                    Priority = todo.Priority,
                    DueDate = todo.DueDate,
                    RemainingTime = todo.DueDate - DateTime.Now
                }).ToList();
            }

            return View(todoViewList);
        }

        public IActionResult CreateTodo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodo(TodoViewModel todoViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Invalid Action";
                TempData["Info"] = "Danger";
                return RedirectToAction("Index", "Home");
            }
            var todos = await _context.Todos.ToListAsync();
            if (todos.Any(t => t.TodoId == todoViewModel.TodoId))
            {
                ModelState.AddModelError("Title", "Todo already exists.");
                TempData["Message"] = "Todo already exists";
                TempData["Info"] = "Danger";
                return View(todoViewModel);
            }
            try
            {
                var todo = new Todo
                {
                    TodoId = Guid.NewGuid().ToString(),
                    Title = todoViewModel.Title,
                    Description = todoViewModel.Description,
                    Status = todoViewModel.Status,
                    Priority = todoViewModel.Priority,
                    CreatedAt = todoViewModel.CreatedDate,
                    DueDate = todoViewModel.DueDate,
                    IsDeleted = 0
                };

                await _context.AddAsync(todo);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Todo Saved Successfully";
                TempData["Info"] = "Success";
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
            // Simulate a delay for the sake of the example
            //await Task.Delay(1000);
            // Redirect to the Index action after creating a todo
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTodo(int id, TodoViewModel todoViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Invalid Action";
                TempData["Info"] = "Delete";
                return View(todoViewModel);
            }
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                TempData["Message"] = "Todo Not Found";
                TempData["Info"] = "Danger";
                return NotFound();
            }

            try
            {
                todo.Title = todoViewModel.Title;
                todo.Description = todoViewModel.Description;
                todo.Status = todoViewModel.Status;
                todo.Priority = todoViewModel.Priority;
                todo.DueDate = todoViewModel.DueDate;
                _context.Update(todo);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Todo Updated Successfully";
                TempData["Info"] = "Success";
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(string TodoId, string Status){
            var todo = await _context.Todos.FindAsync(TodoId);
            if (todo == null)
            {
                TempData["Message"] = "Todo Not Found";
                TempData["Info"] = "Danger";
                return NotFound();
            }

            try
            {
                Enum.TryParse(typeof(TodoStatus), Status, true, out var result);
                todo.Status = (TodoStatus)result;
                _context.Update(todo);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Todo Updated Successfully";
                TempData["Info"] = "Success";
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTodo(string TodoId)
        {
            var todo = await _context.Todos.FindAsync(TodoId);
            if (todo == null)
            {
                TempData["Message"] = "Todo Not Fund";
                TempData["Info"] = "Danger";
                return NotFound();
            }

            try
            {
                todo.IsDeleted = 1;
                todo.DeletedAt = DateTime.UtcNow;
                todo.DeletedBy = HttpContext.Session.GetString("UserId"); ;

                _context.Todos.Update(todo);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Todo Deleted Successfully";
                TempData["Info"] = "Success";
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
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
