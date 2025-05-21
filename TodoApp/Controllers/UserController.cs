using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;
using TodoApp.ViewModel;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace TodoApp.Controllers
{
    public class UserController : Controller
    {
        private readonly TodoDbContext _context;

        public UserController(TodoDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid login data");
                TempData["Message"] = "Invalid login data";
                TempData["Info"] = "Danger";
                return View();
            }

            // Find user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid email or password");
                TempData["Message"] = "Invalid email or password";
                TempData["Info"] = "Danger";
                return View();
            }

            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            // Derive the hash from input password using stored salt
            string enteredPasswordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: model.Password,
                salt: user.Salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            // Compare hashes
            if (user.Password != enteredPasswordHash)
            {
                ModelState.AddModelError("", "Invalid Password");
                TempData["Message"] = "Invalid Password";
                TempData["Info"] = "Danger";
                return View();
            }

            HttpContext.Session.SetString("UserId", user.UserId);
            HttpContext.Session.SetString("Username", user.Username);
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel user)
        {
            var userForReg = new User();
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            if (ModelState.IsValid)
            {
                List<User> users = await _context.Users.ToListAsync();
                if (users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("", "Email already exists");
                    return View();
                }
                if (users.Any(u => u.PhoneNumber == user.PhoneNumber))
                {
                    ModelState.AddModelError("", "Phone number already exists");
                    return View();
                }
                if (user.Password != user.ConfirmPassword)
                {
                    ModelState.AddModelError("", "Password and Confirm Password do not match");
                    return View();
                }
                else
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: user.Password,
                        salt: salt,
                        prf: KeyDerivationPrf.HMACSHA256,
                        iterationCount: 100000,
                        numBytesRequested: 256 / 8));

                    userForReg = new User()
                    {
                        UserId = Guid.NewGuid().ToString(),
                        Username = user.Username,
                        Password = hashed,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        IsActive = true,
                        IsEmailVerified = false,
                        IsPhoneVerified = false,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user.Username,
                        IsDeleted = false,
                        Salt = salt
                    };
                }
                var userReg = await _context.Users.AddAsync(userForReg);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "User");
            }
            else
            {
                ModelState.AddModelError("", "Invalid data");
                return View();
            }
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(IFormFile Profile)
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login"); // or handle as needed
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            if (Profile != null && Profile.Length > 0)
            {
                using var ms = new MemoryStream();
                await Profile.CopyToAsync(ms);
                user.ProfilePicture = ms.ToArray();
                user.ProfilePictureMimeType = Profile.ContentType;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Settings");
        }


    }
}
