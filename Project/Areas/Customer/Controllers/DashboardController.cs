using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Data;
using Project.ViewModels;
using System.Linq;

namespace Project.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("Campuses", "Home");
            }

            var recommendedCampuses = _context.Campuses.Where(c => c.Province == user.Province).ToList();
            // Add logic for other recommendations as needed.

            var model = new DashboardViewModel
            {
                User = user,
                RecommendedCampuses = recommendedCampuses,
                // Add other recommendations to the model.
            };

            return View(model);
        }

        public IActionResult Logout()
        {
            // Implement logout logic here (e.g., clearing cookies, sessions)
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult Forms()
        {
            return View();
        }

        public IActionResult Notices()
        {
            return View();
        }

        public IActionResult Notes()
        {
            return View();
        }

        public IActionResult Results()
        {
            return View();
        }

        public IActionResult PreviousYearPapers()
        {
            return View();
        }

        public IActionResult MyApplications()
        {
            return View();
        }

        public IActionResult Search()
        {
            return View();
        }
    }
}
