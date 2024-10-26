using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using Project.DataAccess.Data;
using Project.Models;
using Project.ViewModels;
using System.Linq;
using System.Security.Claims;

namespace Project.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;


        public DashboardController(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;

        }

        public IActionResult Index()
        {
            ViewData["ActiveSection"] = "Index";
            var userId = _userService.GetUserId();
            var user = _context.Users.SingleOrDefault(u => u.UserID == userId);
            if (user == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var model = new DashboardViewModel
            {
                User = user
            };

            // Initialize recommended notes and notices as empty
            var recommendedNotes = new List<Note>();
            var recommendedNotices = new List<Notice>();

            // Get all form submissions for the user
            var eighthSemForm = _context.EighthSemExamForms.FirstOrDefault(f => f.UserId == userId);
            var firstSemForm = _context.FirstSemExamForms.FirstOrDefault(f => f.UserId == userId);
            var secondSemForm = _context.SecondSemExamForms.FirstOrDefault(f => f.UserId == userId);

            // Determine the latest form by comparing submission dates
            var latestForm = new[] { eighthSemForm?.SubmittedAt, firstSemForm?.SubmittedAt, secondSemForm?.SubmittedAt }
                .Where(d => d != null)
                .OrderByDescending(d => d)
                .FirstOrDefault();

            // If the latest form is the Eighth Sem form, recommend notes based on it
            if (eighthSemForm != null && eighthSemForm.SubmittedAt == latestForm)
            {
                var selectedSubjectIds = eighthSemForm.SelectedSubjects.Split(',').Select(int.Parse).ToList();
                recommendedNotes = _context.Notes.Where(n => selectedSubjectIds.Contains(n.SubjectId)).ToList();

                // Recommend notices for the 8th semester
                recommendedNotices = _context.Notice
                    .Where(n => n.SelectedSubjects == "Eight")
                    .ToList();
            }
            // If the latest form is the First Sem form, recommend notes based on it
            else if (firstSemForm != null && firstSemForm.SubmittedAt == latestForm)
            {
                var selectedSubjectIds = firstSemForm.SelectedSubjects.Split(',').Select(int.Parse).ToList();
                recommendedNotes = _context.Notes.Where(n => selectedSubjectIds.Contains(n.SubjectId)).ToList();

                // Recommend notices for the 1st semester
                recommendedNotices = _context.Notice
                    .Where(n => n.SelectedSubjects == "First")
                    .ToList();
            }
            // If the latest form is the Second Sem form, recommend notes based on it
            else if (secondSemForm != null && secondSemForm.SubmittedAt == latestForm)
            {
                var selectedSubjectIds = secondSemForm.SelectedSubjects.Split(',').Select(int.Parse).ToList();
                recommendedNotes = _context.Notes.Where(n => selectedSubjectIds.Contains(n.SubjectId)).ToList();

                // Recommend notices for the 2nd semester
                recommendedNotices = _context.Notice
                    .Where(n => n.SelectedSubjects == "second")
                    .ToList();
            }
            // If no specific form is filled, recommend Entrance notes and notices
            else if (string.IsNullOrEmpty(user.Collage))
            {
                model.RecommendedCampuses = _context.Campuses.Where(c => c.Province == user.Province).ToList();
                recommendedNotices = _context.Notice
                      .Where(n => n.SelectedSubjects == "Entrance")
                      .ToList();
            }
            else
            {
                var entranceSemester = _context.Semesters.Include(s => s.Subjects)
                                         .ThenInclude(sub => sub.Notes)
                                         .SingleOrDefault(s => s.Name == "Entrance");
                if (entranceSemester != null)
                {
                    recommendedNotes = entranceSemester.Subjects.SelectMany(sub => sub.Notes).ToList();
                    // Recommend entrance notices
                    recommendedNotices = _context.Notice
                        .Where(n => n.SelectedSubjects == "Entrance")
                        .ToList();
                }
            }

            // Assign the recommended notes and notices to the model
            model.RecommendedNotes = recommendedNotes;
            model.RecommendedNotices = recommendedNotices;

            return View(model);
        }

       




        public IActionResult Logout()
        {
            // Implement logout logic here (e.g., clearing cookies, sessions)
            return RedirectToAction("Login", "Home");
        }

        public IActionResult Profile()
        {
            ViewData["ActiveSection"] = "Profile";
            return View();
        }

        public IActionResult Forms()
        {
            ViewData["ActiveSection"] = "Forms";
            var userId = _userService.GetUserId();
            var user = _context.Users.SingleOrDefault(u => u.UserID == userId);

            if (user == null)
            {
                return RedirectToAction("Login", "Home");
            }

            // Check if any of the required details are missing
            var personalDetails = _context.PersonalDetails.FirstOrDefault(pd => pd.UserID == userId);
            var educationalDetails = _context.EducationalDetails.FirstOrDefault(ed => ed.UserID == userId);
            var contactDetails = _context.ContactDetails.FirstOrDefault(cd => cd.UserID == userId);

            if (personalDetails == null || educationalDetails == null || contactDetails == null ||
    !personalDetails.IsDeanApproved || !educationalDetails.IsDeanApproved || !contactDetails.IsDeanApproved)
            {
                // Set an error message in TempData
                TempData["ErrorMessage"] = "Please complete your personal, educational, and contact details and also Get Approved to fill any forms";

                // Redirect to the "Personal" page
                return RedirectToAction("Personal", "Profile");
            }

            var recommendedCampuses = _context.Campuses.ToList();
            var forms = _context.Forms.ToList(); // Fetch forms from the database

            var model = new DashboardViewModel
            {
                User = user,
                PersonalDetails = personalDetails,
                EducationalDetails = educationalDetails,
                ContactDetails = contactDetails,
                Forms = forms,
                RecommendedCampuses = recommendedCampuses
            };

            return View(model);
        }





        public IActionResult Notice(string searchTerm, string selectedSemester)
        {
            ViewData["ActiveSection"] = "Notices";
            var userId = _userService.GetUserId();
            var user = _context.Users.SingleOrDefault(u => u.UserID == userId);
            var noticesQuery = _context.Notice.Include(n => n.Semesters).AsQueryable();

            // Apply search filters
            if (!string.IsNullOrEmpty(searchTerm))
            {
                noticesQuery = noticesQuery.Where(n => n.Heading.Contains(searchTerm) || n.Message.Contains(searchTerm)|| n.SelectedSubjects.Contains(selectedSemester));
            }

           

            // Get notices ordered by latest
            var notices = noticesQuery.OrderByDescending(n => n.CreatedAt).ToList();

            // Create the view model
            var dashboardViewModel = new DashboardViewModel
            {
                User=user,
                Notices = notices, // Assuming Notices is a property in DashboardViewModel
                SearchTerm = searchTerm,
                SelectedSemester = selectedSemester,
                AllSemesters = _context.Semesters.ToList() // Pass all semesters for filtering
            };

            return View(dashboardViewModel);
        }


        public IActionResult Notes(string searchQuery)
        {
            ViewData["ActiveSection"] = "Notes";
            var userId = _userService.GetUserId();
            var user = _context.Users.SingleOrDefault(u => u.UserID == userId);
            // Fetch all semesters, subjects, and notes
            var semesters = _context.Semesters
                                    .Include(s => s.Subjects)
                                        .ThenInclude(sub => sub.Notes)
                                    .ToList();

            // If search query is provided, filter the notes based on the subject name or note title
            if (!string.IsNullOrEmpty(searchQuery))
            {
                foreach (var semester in semesters)
                {
                    semester.Subjects = semester.Subjects
                        .Where(sub => sub.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)
                                      || sub.Notes.Any(note => note.FileName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                }
            }

            // Pass the data to the view
            var viewModel = new DashboardViewModel
            {
                User = user,
                RecommendedNotes = semesters.SelectMany(s => s.Subjects.SelectMany(sub => sub.Notes)).ToList(), // All notes for search functionality
                EightSemesterSubjects = semesters.SelectMany(s => s.Subjects).ToList(), // You may change this based on your needs
            };

            return View(viewModel);
        }

        public IActionResult NoteDetails(int noteId)
        {
            ViewData["ActiveSection"] = "Notes";
            var userId = _userService.GetUserId();
            var user = _context.Users.SingleOrDefault(u => u.UserID == userId);
            // Fetch the selected note by its ID
            var selectedNote = _context.Notes
                .Include(n => n.Subject)
                .ThenInclude(s => s.Semester)
                .FirstOrDefault(n => n.Id == noteId);

            if (selectedNote == null)
            {
                return NotFound(); // Handle case when note is not found
            }

            // Get the SemesterId of the selected note's subject
            var semesterId = _context.Subjects
                .Where(s => s.Id == selectedNote.SubjectId)
                .Select(s => s.SemesterId)
                .FirstOrDefault();

            var semesterSub = _context.Subjects.Where(s =>s.SemesterId== s.SemesterId)
              .ToList();

            // Get the notes of the same semester but different from the selected note
            var sameSemesterNotes = _context.Notes
                .Where(n => n.Subject.SemesterId == semesterId && n.Id != noteId)
                .ToList();



            // Fetch related notes from the same subject (excluding the selected note)
            var relatedNotes = _context.Notes
                .Where(n => n.SubjectId == selectedNote.SubjectId && n.Id != noteId)
                .ToList();

            // Prepare the DashboardViewModel with note data
            var dashboardViewModel = new DashboardViewModel
            {
                User = user,
                Subject=semesterSub,
                SelectedNote = selectedNote,
                RelatedNotes = relatedNotes,
                RecommendedNotes=sameSemesterNotes
            };

            return View(dashboardViewModel);
        }



        public IActionResult Results()
        {
            ViewData["ActiveSection"] = "Results";
            return View();
        }

        public IActionResult PreviousYearPapers()
        {
            ViewData["ActiveSection"] = "PreviousYearPapers";
            return View();
        }

        public IActionResult MyApplications()
        {

            ViewData["ActiveSection"] = "My Applications";
            var userId = _userService.GetUserId();
          

            // Query all forms for the current user
            var entranceForms = _context.EntranceForms.Where(f => f.UserId == userId)
                                    .Select(f => new ApplicationViewModel
                                    {
                                        ApplyDate = f.SubmittedAt,
                                        Id = f.Id,

                                        College = f.Collage,
                                        Semester = 1,
                                        Status = f.CollageApproved
            ? (f.DeanApproved ? "Approved" : "Collage Approved")
            : "Pending"
                                    });

                var firstSemForms = _context.FirstSemExamForms.Where(f => f.UserId == userId)
                                    .Select(f => new ApplicationViewModel
                                    {
                                        ApplyDate = f.SubmittedAt,
                                        Id = f.Id,
                                        College = f.Collage,
                                        Semester = 1,
                                        Status = f.CollageApproved
            ? (f.DeanApproved ? "Approved" : "Collage Approved")
            : "Pending"

                                    });
            var secondSemForms = _context.SecondSemExamForms.Where(f => f.UserId == userId)
                                  .Select(f => new ApplicationViewModel
                                  {
                                      ApplyDate = f.SubmittedAt,
                                      Id = f.Id,
                                      College = f.Collage,
                                      Semester = 2,
                                      Status = f.CollageApproved
            ? (f.DeanApproved ? "Approved" : "Collage Approved")
            : "Pending"

                                  });
            var eightSemForms = _context.EighthSemExamForms.Where(f => f.UserId == userId)
                                .Select(f => new ApplicationViewModel
                                {
                                    ApplyDate = f.SubmittedAt,
                                    Id = f.Id,
                                    College = f.Collage,
                                    Semester = 8,
                                    Status = f.CollageApproved
            ? (f.DeanApproved ? "Approved" : "Collage Approved")
            : "Pending"
                                });


            // Similarly for SecondSemExamForms up to EightSemExamForms
            // Combine all forms into a single list
            var allForms = entranceForms
                                .Union(firstSemForms)
                                .Union(secondSemForms)  // Example for SecondSemExamForms
                                .Union(eightSemForms)    // Continue with other forms
                                .ToList();
            var viewModel = new DashboardViewModel
            {
                User = _context.Users.FirstOrDefault(u => u.UserID == userId),
                ApplicationViewModel = allForms ?? new List<ApplicationViewModel>() // Ensure it's not null
                
            };

          

            return View(viewModel);
        }
        public IActionResult ViewDetail(int id, int sem)
        {
            ViewData["ActiveSection"] = "My Applications";
            var userId = _userService.GetUserId();
            
            if (sem == 8)
            {
                var application = _context.EighthSemExamForms
                                          .Include(a => a.User)
                                          .FirstOrDefault(a => a.Id == id);

                var eighthSemester = _context.Semesters
                              .Include(s => s.Subjects)
                              .SingleOrDefault(s => s.Name == "Eight");
               
                var viewModel = new DashboardViewModel
                {
                    User = _context.Users.FirstOrDefault(u => u.UserID == userId),
                    Subject= _context.Subjects.ToList(),
                    ApplicationViewModel1 = new ApplicationViewModel
                    {
                        ApplyDate = application.SubmittedAt,
                       SelectedSubjects=application?.SelectedSubjects?.Split(',').Select(int.Parse).ToArray() ?? new int[0],
                        College = application.Collage,
                        PaymentImage = application?.PaymentImagePath ?? string.Empty,
                        Semester = 8,
                        Status = application.CollageApproved
            ? (application.DeanApproved ? "Approved" : "Collage Approved")
            : "Pending",
                    } // Ensure it's not null

                };



                if (application == null)
                {
                    return NotFound();
                }

                return View(viewModel);
            }
            else if (sem == 1)
            {
                var application = _context.FirstSemExamForms
                                          .Include(a => a.User)
                                          .FirstOrDefault(a => a.Id == id);
                var firstSemester = _context.Semesters
                                 .Include(s => s.Subjects)
                                 .SingleOrDefault(s => s.Name == "First");

                var viewModel = new DashboardViewModel
                {
                    Subject = _context.Subjects.ToList(),
                    User = _context.Users.FirstOrDefault(u => u.UserID == userId),
                    ApplicationViewModel1 = new ApplicationViewModel
                    {
                        ApplyDate = application.SubmittedAt,
                        SelectedSubjects = application?.SelectedSubjects?.Split(',').Select(int.Parse).ToArray() ?? new int[0],
                        College = application.Collage,
                        PaymentImage = application?.PaymentImagePath ?? string.Empty,
                        Semester = 1,
                        Status = application.CollageApproved
           ? (application.DeanApproved ? "Approved" : "Collage Approved")
           : "Pending",
                    } // Ensure it's not null

                };

                if (application == null)
                {
                    return NotFound();
                }

                return View(viewModel);
            }
            else
            {
                // Handle other semesters or return a default value
                return NotFound();
            }
        }






        public IActionResult Search()
        {
            return View();
        }
    }

   
}
