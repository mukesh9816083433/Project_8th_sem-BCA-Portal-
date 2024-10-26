using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DataAccess.Data;
using Microsoft.AspNetCore.Hosting; // Add this using directive

using Project.Models;
using Project.Models.ViewModel;

namespace Project.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class FormsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FormsController(ApplicationDbContext context, IUserService userService, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _userService = userService;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpPost]
        public IActionResult SetCollege(string college)
        {
            var userId = _userService.GetUserId();
            var user = _context.Users.SingleOrDefault(u => u.UserID == userId);

            if (user != null && !string.IsNullOrEmpty(college))
            {
                user.Collage = college;
                _context.SaveChanges();
            }

            return RedirectToAction("Forms", "Dashboard");
        }

        [Authorize]
        public IActionResult EntranceForm()
        {
            var model = CreateDashboardViewModel("Entrance Form");
            return View(model);
        }

        [Authorize]
        public IActionResult RegistrationForm()
        {
            var model = CreateDashboardViewModel("Registration Form");
            return View(model);
        }

        // Add similar methods for other forms...


        // Add methods for other forms similarly...
        [Authorize]
        [HttpGet]
        public IActionResult FirstSemExamForm()
        {

            var model = CreateDashboardViewModel("First Sem Exam Form");
            return View(model);

        }

        [Authorize]
        [HttpPost]
        public IActionResult FirstSemExamForm(DashboardViewModel model)
        {
            var userId = _userService.GetUserId();
            var user = _context.Users.FirstOrDefault(u => u.UserID == userId);

            if (user == null)
            {
                return RedirectToAction("Login", "Home");
            }

            // Check if the EightSemExamForm is null
            if (model.FirstSemExamForm == null)
            {
                ModelState.AddModelError(string.Empty, "Form data is missing.");
                return View(model); // Or handle it according to your application's requirements
            }

            // Check if a form already exists for this user
            var form = _context.FirstSemExamForms.FirstOrDefault(f => f.UserId == user.UserID);

            if (form == null)
            {
                // If no form exists, create a new one
                form = new FirstSemExamForm
                {
                    UserId = user.UserID,
                    SubmittedAt = DateTime.Now,
                    CollageApproved = false,
                    DeanApproved = false
                };
                _context.FirstSemExamForms.Add(form);
            }

            // Update form fields
            form.SelectedSubjects = string.Join(",", model.FirstSemExamForm.SelectedSubjects ?? new int[0]); // Default to empty array if null

            // Handle file upload for the payment image
            if (model.FirstSemExamForm.PaymentImage != null)
            {
                var fileName = Path.GetFileName(model.FirstSemExamForm.PaymentImage.FileName);
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.FirstSemExamForm.PaymentImage.CopyTo(fileStream);
                }
                form.PaymentImagePath = "/uploads/" + fileName;
            }

            form.Collage = user.Collage;

            _context.SaveChanges();

            // Fetch the notes based on selected subjects
            var selectedSubjectIds = model.FirstSemExamForm.SelectedSubjects ?? new int[0];
            var recommendedNotes = _context.Notes
                                            .Where(n => selectedSubjectIds.Contains(n.SubjectId))
                                            .ToList();

            // Create a new ViewModel to pass the notes to the Index view
            var dashboardViewModel = new DashboardViewModel
            {
                User = user,
                RecommendedNotes = recommendedNotes,
                PersonalDetails = _context.PersonalDetails.FirstOrDefault(pd => pd.UserID == user.UserID),
                EducationalDetails = _context.EducationalDetails.FirstOrDefault(ed => ed.UserID == user.UserID),
                ContactDetails = _context.ContactDetails.FirstOrDefault(cd => cd.UserID == user.UserID),
                Forms = _context.Forms.ToList(),
                RecommendedCampuses = _context.Campuses.ToList()
            };

            return RedirectToAction("Index", "Dashboard", dashboardViewModel);
        }






        [Authorize]
        [HttpGet]
        public IActionResult EighthSemExamForm()
        {
   
                var model = CreateDashboardViewModel("Eight Sem Exam Form");
                return View(model);
            
        }
        [Authorize]
        [HttpPost]
        public IActionResult EighthSemExamForm(DashboardViewModel model)
        {
            var userId = _userService.GetUserId();
            var user = _context.Users.FirstOrDefault(u => u.UserID == userId);

            if (user == null)
            {
                return RedirectToAction("Login", "Home");
            }

            // Check if the EightSemExamForm is null
            if (model.EightSemExamForm == null)
            {
                ModelState.AddModelError(string.Empty, "Form data is missing.");
                return View(model); // Or handle it according to your application's requirements
            }

            // Check if a form already exists for this user
            var form = _context.EighthSemExamForms.FirstOrDefault(f => f.UserId == user.UserID);

            if (form == null)
            {
                // If no form exists, create a new one
                form = new EightSemExamForm
                {
                    UserId = user.UserID,
                    SubmittedAt = DateTime.Now,
                    CollageApproved = false,
                    DeanApproved = false
                };
                _context.EighthSemExamForms.Add(form);
            }

            // Update form fields
            form.SelectedSubjects = string.Join(",", model.EightSemExamForm.SelectedSubjects ?? new int[0]); // Default to empty array if null

            // Handle file upload for the payment image
            if (model.EightSemExamForm.PaymentImage != null)
            {
                var fileName = Path.GetFileName(model.EightSemExamForm.PaymentImage.FileName);
                var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads", fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.EightSemExamForm.PaymentImage.CopyTo(fileStream);
                }
                form.PaymentImagePath = "/uploads/" + fileName;
            }

            form.Collage = model.EightSemExamForm.Collage;

            _context.SaveChanges();

            // Fetch the notes based on selected subjects
            var selectedSubjectIds = model.EightSemExamForm.SelectedSubjects ?? new int[0];
            var recommendedNotes = _context.Notes
                                            .Where(n => selectedSubjectIds.Contains(n.SubjectId))
                                            .ToList();

            // Create a new ViewModel to pass the notes to the Index view
            var dashboardViewModel = new DashboardViewModel
            {
                User = user,
                RecommendedNotes = recommendedNotes,
                PersonalDetails = _context.PersonalDetails.FirstOrDefault(pd => pd.UserID == user.UserID),
                EducationalDetails = _context.EducationalDetails.FirstOrDefault(ed => ed.UserID == user.UserID),
                ContactDetails = _context.ContactDetails.FirstOrDefault(cd => cd.UserID == user.UserID),
                Forms = _context.Forms.ToList(),
                RecommendedCampuses = _context.Campuses.ToList()
            };

            return RedirectToAction("Index", "Dashboard", dashboardViewModel);
        }
    


private string SavePaymentImage(IFormFile paymentImage)
        {
            if (paymentImage == null || paymentImage.Length == 0)
            {
                throw new ArgumentException("Invalid payment image.");
            }

            // Generate a unique file name to prevent collisions
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(paymentImage.FileName);

            // Specify the directory to save the uploaded file
            var uploadsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "paymentImages");

            // Create the directory if it doesn't exist
            if (!Directory.Exists(uploadsDirectory))
            {
                Directory.CreateDirectory(uploadsDirectory);
            }

            // Combine the directory and the file name to get the full file path
            var filePath = Path.Combine(uploadsDirectory, uniqueFileName);

            // Save the file to the specified path
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                paymentImage.CopyTo(fileStream);
            }

            // Return the relative path to be stored in the database
            return Path.Combine("uploads", "paymentImages", uniqueFileName).Replace("\\", "/");
        }

        private DashboardViewModel CreateDashboardViewModel(string formName)
        {
            var userId = _userService.GetUserId();
            var user = _context.Users.FirstOrDefault(u => u.UserID == userId);
            var forms = _context.Forms.ToList();
            var recommendedCampuses = _context.Campuses.ToList();



            var eighthSemester = _context.Semesters
                              .Include(s => s.Subjects)
                              .SingleOrDefault(s => s.Name == "Eight");
            var firstSemester = _context.Semesters
                              .Include(s => s.Subjects)
                              .SingleOrDefault(s => s.Name == "First");

            var form8 = _context.EighthSemExamForms
                              .FirstOrDefault(f => f.UserId == userId);
            var form1 = _context.FirstSemExamForms
                             .FirstOrDefault(f => f.UserId == userId);

            // If eighthSemester or form is null, handle it appropriately
            var model = new DashboardViewModel
            {
                User = user,
                PersonalDetails = _context.PersonalDetails.FirstOrDefault(pd => pd.UserID == userId),
                EducationalDetails = _context.EducationalDetails.FirstOrDefault(ed => ed.UserID == userId),
                ContactDetails = _context.ContactDetails.FirstOrDefault(cd => cd.UserID == userId),
                Forms = forms,
                RecommendedCampuses = recommendedCampuses,
                EightSemExamForm = new EightSemExamFormViewModel
                {
                    EightSemesterSubjects = eighthSemester?.Subjects?.ToList() ?? new List<Subject>(),
                    Collage = user.Collage,
                    PaymentImagePath = form8?.PaymentImagePath ?? string.Empty,
                    CollageApproved = form8?.CollageApproved ?? false,
                    DeanApproved = form8?.DeanApproved ?? false,
                    SelectedSubjects = form8?.SelectedSubjects?.Split(',').Select(int.Parse).ToArray() ?? new int[0]
                },
                 FirstSemExamForm = new FirstSemExamFormViewModel
                 {
                     FirstSemesterSubjects = firstSemester?.Subjects?.ToList() ?? new List<Subject>(),
                     Collagee = user.Collage,
                     PaymentImagePath = form1?.PaymentImagePath ?? string.Empty,
                     CollageApproved = form1?.CollageApproved ?? false,
                     DeanApproved = form1?.DeanApproved ?? false,
                     SelectedSubjects = form1?.SelectedSubjects?.Split(',').Select(int.Parse).ToArray() ?? new int[0]
                 }
            };

            return model;
        }

    }
}
