using Microsoft.AspNetCore.Mvc;

using Project.DataAccess.Data;
using Project.Models.ViewModel;
using Project.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;

namespace Project.Areas.Collage.Controllers
{
    [Area("Collage")]
    public class NoticeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public NoticeController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _hostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Notice()
        {
            ViewData["ActiveSection"] = "Notices";
            var viewModel = new NoticeViewModel
            {
                AllSemesters = _context.Semesters.ToList(), // Fetch available semesters
                AllNotices = _context.Notice.Include(n => n.Semesters).OrderByDescending(n => n.CreatedAt).ToList(), // Fetch all notices
                Heading = "Holiday", // Default value
            };
          
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddNotice(NoticeViewModel model)
        {
            
                // Handle image upload (if any)
                string imagePath = null;
                if (model.NoticeImage != null)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.NoticeImage.FileName;
                    var uploads = Path.Combine(_hostEnvironment.WebRootPath, "images/notices");
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.NoticeImage.CopyTo(fileStream);
                    }
                    imagePath = "/images/notices/" + uniqueFileName;
                }

                // Save notice to the database
                var notice = new Notice
                {
                    Heading = model.Heading,
                    Message = model.Message,
                    ImagePath = imagePath,
                    SelectedSubjects = model.SelectedSubjects,
                    IsAllSemesters = model.IsAllSemesters,
                 
                    NoticeBy = "By Collage", // For example, this could be set based on who adds the notice
                    CreatedAt = DateTime.Now
                };
        
            _context.Notice.Add(notice);
            
                _context.SaveChanges();

                return RedirectToAction("Notice"); // Redirect to the notices listing page
            

            // If model state is invalid, return the form with validation messages
           /* model.SelectedSemesters = _context.Semesters.ToList(); // Reload semesters list in case of validation failure
            model.AllNotices = _context.Notice.Include(n => n.Semesters).OrderByDescending(n => n.CreatedAt).ToList(); // Reload notices list
            return View("Notice", model);*/
        }

        // GET: EditNotice
        public IActionResult EditNotice(int id)
        {
            ViewData["ActiveSection"] = "Notices";
            var notice = _context.Notice.Include(n => n.Semesters).FirstOrDefault(n => n.Id == id);
            if (notice == null)
            {
                return NotFound();
            }

            var viewModel = new NoticeViewModel
            {
                Id = notice.Id,
                Heading = notice.Heading,
                Message = notice.Message,
                IsAllSemesters = notice.IsAllSemesters,
                SelectedSubjects= notice.SelectedSubjects,
                AllSemesters = _context.Semesters.ToList(), // Fetch all semesters
                NoticeBy = notice.NoticeBy,
                ImagePath = notice.ImagePath,
                AllNotices = _context.Notice.OrderByDescending(n => n.CreatedAt).ToList() // For display if necessary
            };

            return View(viewModel);
        }

        // POST: EditNotice
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditNotice(NoticeViewModel model)
        {
           
                var notice = _context.Notice.Include(n => n.Semesters).FirstOrDefault(n => n.Id == model.Id);
                if (notice == null)
                {
                    return NotFound();
                }

                // Handle image upload (if any)
                if (model.NoticeImage != null)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.NoticeImage.FileName;
                    var uploads = Path.Combine(_hostEnvironment.WebRootPath, "images/notices");
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        model.NoticeImage.CopyTo(fileStream);
                    }
                    notice.ImagePath = "/images/notices/" + uniqueFileName;
                }

                // Update notice details
                notice.Heading = model.Heading;
                notice.Message = model.Message;
                notice.IsAllSemesters = model.IsAllSemesters;
                notice.SelectedSubjects =  model.SelectedSubjects;

                _context.SaveChanges();
                return RedirectToAction("Notice");
           
        }

        // POST: DeleteNotice
        [HttpPost]
        public IActionResult DeleteNotice(int id)
        {
            var notice = _context.Notice.FirstOrDefault(n => n.Id == id);
            if (notice == null)
            {
                return NotFound();
            }

            _context.Notice.Remove(notice);
            _context.SaveChanges();
            return RedirectToAction("Notice");
        }
    }
}

