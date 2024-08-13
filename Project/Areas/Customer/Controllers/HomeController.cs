using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Data;
using Project.Models;
using Project.ViewModels;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Project.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Campuses()
        {


            var campuses = _context.Campuses.ToList();
            return View(campuses);
        }

        public IActionResult RegistrationInstructions()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the username or email already exists
                if (_context.Users.Any(u => u.Username == model.Username || u.Email == model.Email))
                {
                    ModelState.AddModelError("Username", "Username or Email already exists.");
                    return View(model);
                }

                // Hash the password
                var passwordHash = HashPassword(model.Password);

                // Create a new user
                var user = new User
                {
                    Username = model.Username,
                    PasswordHash = passwordHash,
                    Email = model.Email,
                    Province = model.Province,
                    District = model.District,
                    Address = model.Address,
                    IsActive = true
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                // Redirect to a login page or home page
                return RedirectToAction("Login");
            }

            return View(model);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.SingleOrDefault(u => u.Username == model.Username);

                if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
                {
                    ModelState.AddModelError("Username", "Invalid login attempt.");
                    return View(model);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                     new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString())
                    // Add additional claims as needed
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                // Redirect to dashboard
                return RedirectToAction("Index", "Dashboard");
            }

            return View(model);
        }

        private string HashPassword(string password)
        {
            using (var sha = SHA256.Create())
            {
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}

