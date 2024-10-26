using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DataAccess.Data;
using Project.Models;

using Microsoft.AspNetCore.Hosting;
using Project.Models.ViewModel;
using Microsoft.SqlServer.Server;
using System.Net.Mail;
using System.Net;
using static System.Net.Mime.MediaTypeNames;

namespace Project.Areas.Collage.Controllers
{
    [Area("Collage")]
    public class CollageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public CollageController(ApplicationDbContext context, IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userService = userService;
            _hostEnvironment = webHostEnvironment;
        }
        public IActionResult Home()
        {
            ViewData["ActiveSection"] = "Home";

            var campuses = _context.Campuses.ToList();  // Fetch campuses from the database
            /* var model = new CollageHomeViewModel
             {
                 Campuses = campuses
             };*/

            return View(campuses);
        }
        [HttpPost]
        public IActionResult ValidateLoginCode(int College, string LoginCode)
        {
            var campus = _context.Campuses.FirstOrDefault(c => c.CampusID == College && c.LoginCode == int.Parse(LoginCode));

            if (campus == null)
            {
                TempData["Error"] = "Invalid college or login code.";
                return RedirectToAction("Home");
            }
            HttpContext.Session.SetString("SelectedCollege", campus.CampusName);
            // Proceed with the login process, set session or auth cookies
            return RedirectToAction("Users");
        }


        public IActionResult Report()
        {
            // Get user statistics
            ViewData["ActiveSection"] = "Dashboard";
            var totalUsers = _context.Users.Count();
            var activeUsers = _context.Users.Count(u => u.IsActive);
            var inactiveUsers = totalUsers - activeUsers;

            // Get form statistics (e.g., Entrance forms, Exam forms)
            var entranceFormsCount = _context.EighthSemExamForms.Count();
            var firstSemExamFormsCount = _context.FirstSemExamForms.Count();
            var totalForms = entranceFormsCount + firstSemExamFormsCount;

            // Get notes statistics
            var totalNotes = _context.Notes.Count();

            // Get notices count
            var totalNotices = _context.Notice.Count();

            // Create a view model with summary data
            var reportViewModel = new ReportViewModel
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                InactiveUsers = inactiveUsers,
                TotalForms = totalForms,
                EntranceFormsCount = entranceFormsCount,
                FirstSemExamFormsCount = firstSemExamFormsCount,
                TotalNotes = totalNotes,
                TotalNotices = totalNotices
            };

            return View(reportViewModel);
        }
        public IActionResult Applications()
        {
            ViewData["ActiveSection"] = "Applications";
            var forms = _context.Forms.ToList();
            return View(forms);
        }

        public IActionResult FormApplications(int formId)
        {

            var form = _context.Forms.FirstOrDefault(f => f.Id == formId);

            // Depending on the form, query the correct table
            var formApplications = new List<ApplicationViewModel>();

            if (form.Name == "Entrance Form")
            {
                formApplications = _context.EntranceForms
                    .Include(e => e.User)
                    .Select(e => new ApplicationViewModel
                    {
                        ApplyDate = e.SubmittedAt,
                        Status1 = e.CollageApproved ? true : false,
                        UserName = e.User.Username,
                        Status2 = e.DeanApproved ? true : false,
                        userId = e.User.UserID,
                        Semester = 0,


                    }).OrderByDescending(e => e.ApplyDate).ToList();
            }
            else if (form.Name == "First Sem Exam Form")
            {
                formApplications = _context.FirstSemExamForms
                    .Include(e => e.User)
                    .Select(e => new ApplicationViewModel
                    {
                        ApplyDate = e.SubmittedAt,
                        Status2 = e.DeanApproved ? true : false,
                        Status1 = e.CollageApproved ? true : false,
                        UserName = e.User.Username,
                        userId = e.User.UserID,
                        Semester = 1

                    }).OrderByDescending(e => e.ApplyDate).ToList();
            }
            else if (form.Name == "Eighth Sem Exam Form")
            {
                formApplications = _context.EighthSemExamForms
                    .Include(e => e.User)
                    .Select(e => new ApplicationViewModel
                    {
                        ApplyDate = e.SubmittedAt,
                        Status1 = e.CollageApproved ? true : false,
                        UserName = e.User.Username,
                        Status2 = e.DeanApproved ? true : false,
                        userId = e.User.UserID,
                        Semester = 8

                    }).OrderByDescending(e => e.ApplyDate).ToList();
            }
            // Repeat for other forms...

            return View(formApplications);
        }

        public IActionResult FormApplicationViewDetail(int id, int sem)
        {

            if (sem == 8)
            {
                var userid = id;
                var application = _context.EighthSemExamForms
                                          .Include(a => a.User)
                                          .FirstOrDefault(a => a.UserId == userid);

                var eighthSemester = _context.Semesters
                              .Include(s => s.Subjects)
                              .SingleOrDefault(s => s.Name == "Eight");


                var viewModel = new DashboardViewModel
                {
                    User = _context.Users.FirstOrDefault(u => u.UserID == id),
                    Subject = _context.Subjects.ToList(),
                    ApplicationViewModel1 = new ApplicationViewModel
                    {
                        ApplyDate = application.SubmittedAt,
                        userId = application.UserId,
                        Status1 = application.CollageApproved,
                        SelectedSubjects = application?.SelectedSubjects?.Split(',').Select(int.Parse).ToArray() ?? new int[0],
                        College = application?.Collage ?? string.Empty,
                        PaymentImage = application?.PaymentImagePath ?? string.Empty,
                        Semester = 8,

                        Status = application?.CollageApproved ?? false
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

                                          .FirstOrDefault(a => a.UserId == id);
                var firstSemester = _context.Semesters
                                 .Include(s => s.Subjects)
                                 .SingleOrDefault(s => s.Name == "First");

                var viewModel = new DashboardViewModel
                {
                    Subject = _context.Subjects.ToList(),
                    User = _context.Users.FirstOrDefault(u => u.UserID == id),
                    ApplicationViewModel1 = new ApplicationViewModel
                    {
                        ApplyDate = application.SubmittedAt,
                        userId = application.UserId,

                        Status1 = application.CollageApproved,
                        SelectedSubjects = application?.SelectedSubjects?.Split(',').Select(int.Parse).ToArray() ?? new int[0],
                        College = application?.Collage ?? string.Empty,
                        PaymentImage = application?.PaymentImagePath ?? string.Empty,
                        Semester = 1,
                        Status = application?.CollageApproved ?? false
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

        public IActionResult ApproveApplication(int id, int sem)
        {
            if (sem == 1)
            {


                // Retrieve the application based on the semester and ID
                var application = _context.FirstSemExamForms // Change this based on the semester value
                                         .FirstOrDefault(a => a.UserId == id);

                if (application == null)
                {
                    return NotFound("Application not found.");
                }

                // Approve the application
                application.CollageApproved = true;

                // Save changes to the database
                _context.SaveChanges();

                // Redirect to the application details page or another page
                return RedirectToAction("FormApplicationViewDetail", new { id = id, sem = sem });
            }
            else if (sem == 8)
            {


                // Retrieve the application based on the semester and ID
                var application = _context.EighthSemExamForms // Change this based on the semester value
                                         .FirstOrDefault(a => a.UserId == id);

                if (application == null)
                {
                    return NotFound("Application not found.");
                }

                // Approve the application
                application.CollageApproved = true;

                // Save changes to the database
                _context.SaveChanges();

                // Redirect to the application details page or another page
                return RedirectToAction("FormApplicationViewDetail", new { id = id, sem = sem });
            }
            else
            {
                return RedirectToAction("FormApplicationViewDetail", new { id = id, sem = sem });
            }
        }

        [HttpPost]
        public IActionResult RejectApplication(int id, int sem, string message)
        {
            if (sem == 1)
            {
                // Retrieve the application based on the semester and ID
                var application = _context.FirstSemExamForms // Adjust this based on the semester
                                         .Include(a => a.User)
                                         .FirstOrDefault(a => a.UserId == id);

                if (application == null)
                {
                    return NotFound("Application not found.");
                }

                // Retrieve user details for email
                var user = application.User;

                if (user != null)
                {
                    // Send rejection email
                    string subject = "Your Application Has Been Rejected";
                    string body = $"Dear {user.Username},\n\nYour application for the semester has been rejected for the following reason:\n\n{message}\n\nBest regards,\nCollege Administration";
                    SendEmail(user.Email, subject, body); // Assume you have a method to send emails
                }

                // Delete the application
                _context.FirstSemExamForms.Remove(application); // Adjust for the correct semester table
                _context.SaveChanges();

                // Redirect to a page after rejection
                return RedirectToAction("FormApplications", new { formid = application.Id }); // Redirect to an appropriate page after rejecting the form
            }
            else if (sem == 8)
            {
                // Retrieve the application based on the semester and ID
                var application = _context.EighthSemExamForms // Adjust this based on the semester
                                         .Include(a => a.User)
                                         .FirstOrDefault(a => a.UserId == id);

                if (application == null)
                {
                    return NotFound("Application not found.");
                }

                // Retrieve user details for email
                var user = application.User;

                if (user != null)
                {
                    // Send rejection email
                    string subject = "Your Application Has Been Rejected";
                    string body = $"Dear {user.Username},\n\nYour application for the semester has been rejected for the following reason:\n\n{message}\n\nBest regards,\nCollege Administration";
                    SendEmail(user.Email, subject, body); // Assume you have a method to send emails
                }

                // Delete the application
                _context.EighthSemExamForms.Remove(application); // Adjust for the correct semester table
                _context.SaveChanges();

                // Redirect to a page after rejection
                return RedirectToAction("FormApplications", new { formid = application.Id }); // Redirect to an appropriate page after rejecting the form
            }
            else
            {
                return NotFound();
            }
        }

        private void SendEmail(string email, string subject, string message)
        {
            try
            {
                // Create a new MailMessage object
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("mukesh9816083433@gmail.com"), // Replace with your email
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = false // Set to true if your message contains HTML
                };

                // Add the recipient email address
                mailMessage.To.Add(email);

                // Create a new SmtpClient object
                using (var smtpClient = new SmtpClient("smtp.example.com", 587)) // Replace with your SMTP server and port
                {
                    smtpClient.Credentials = new NetworkCredential("mukesh9816083433@gmail.com", "Khatiwada12"); // Replace with your email and password
                    smtpClient.EnableSsl = true; // Enable SSL if required by your SMTP server

                    // Send the email
                    smtpClient.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occurred while sending the email
                Console.WriteLine("Error sending email: " + ex.Message);
                // Optionally, log the exception
            }
        }



        public IActionResult Users(string searchQuery)
        {
            ViewData["ActiveSection"] = "Users";

            // Get the selected college ID from session or cookies (assuming you store it during login)
            var selectedCollege = HttpContext.Session.GetString("SelectedCollege");

            if (string.IsNullOrEmpty(selectedCollege))
            {
                // If no college is selected, return an empty view
                return View(new List<UserDetailsViewModel>());
            }

            // Get users that belong to the selected college
            var usersQuery = _context.Users.Where(u => u.Collage == selectedCollege);

            // If a search query is provided, filter the users based on the username
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                usersQuery = usersQuery.Where(u => u.Username.ToLower().Contains(searchQuery));
            }

            // Project the filtered users into the UserDetailsViewModel
            var users = usersQuery
                .Select(user => new UserDetailsViewModel
                {
                    UserId = user.UserID,
                    Username = user.Username,
                    PersonalDetails = _context.PersonalDetails.FirstOrDefault(pd => pd.UserID == user.UserID),
                    EducationalDetails = _context.EducationalDetails.FirstOrDefault(ed => ed.UserID == user.UserID),
                    ContactDetails = _context.ContactDetails.FirstOrDefault(cd => cd.UserID == user.UserID)
                })
                .ToList();

            return View(users);
        }









        public IActionResult UserDetails(int id)
        {
            ViewData["ActiveSection"] = "Users";



            // Query all forms for the current user
            var entranceForms = _context.EntranceForms.Where(f => f.UserId == id)
                                    .Select(f => new ApplicationViewModel
                                    {
                                        ApplyDate = f.SubmittedAt,
                                        Id = f.Id,
                                        userId = f.UserId,
                                        College = f.Collage,
                                        Semester = 1,
                                        Status = f.CollageApproved
            ? (f.DeanApproved ? "Approved" : "Collage Approved")
            : "Pending"
                                    });

            var firstSemForms = _context.FirstSemExamForms.Where(f => f.UserId == id)
                                .Select(f => new ApplicationViewModel
                                {
                                    ApplyDate = f.SubmittedAt,
                                    Id = f.Id,
                                    userId = f.UserId,
                                    College = f.Collage,
                                    Semester = 1,
                                    Status = f.CollageApproved
        ? (f.DeanApproved ? "Approved" : "Collage Approved")
        : "Pending"

                                });
            var secondSemForms = _context.SecondSemExamForms.Where(f => f.UserId == id)
                                  .Select(f => new ApplicationViewModel
                                  {
                                      ApplyDate = f.SubmittedAt,
                                      Id = f.Id,
                                      userId = f.UserId,
                                      College = f.Collage,
                                      Semester = 2,
                                      Status = f.CollageApproved
            ? (f.DeanApproved ? "Approved" : "Collage Approved")
            : "Pending"

                                  });
            var eightSemForms = _context.EighthSemExamForms.Where(f => f.UserId == id)
                                .Select(f => new ApplicationViewModel
                                {
                                    ApplyDate = f.SubmittedAt,
                                    Id = f.Id,
                                    userId = f.UserId,
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

            var userDetails = new UserDetailsViewModel
            {
                UserId = id,
                PersonalDetails = _context.PersonalDetails.FirstOrDefault(pd => pd.UserID == id),
                EducationalDetails = _context.EducationalDetails.FirstOrDefault(ed => ed.UserID == id),
                ContactDetails = _context.ContactDetails.FirstOrDefault(cd => cd.UserID == id),
                ApplicationViewModel = allForms ?? new List<ApplicationViewModel>()
            };

            if (userDetails.PersonalDetails == null ||
                userDetails.EducationalDetails == null ||
                userDetails.ContactDetails == null)
            {
                return NotFound();
            }

            return View(userDetails);
        }

        public IActionResult ViewDetail(int id, int sem)
        {
            var userId = id;

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
                    User = _context.Users.FirstOrDefault(u => u.UserID == id),
                    Subject = _context.Subjects.ToList(),
                    ApplicationViewModel1 = new ApplicationViewModel
                    {
                        ApplyDate = application.SubmittedAt,
                        SelectedSubjects = application?.SelectedSubjects?.Split(',').Select(int.Parse).ToArray() ?? new int[0],
                        College = application?.Collage ?? string.Empty,
                        PaymentImage = application?.PaymentImagePath ?? string.Empty,
                        Semester = 8,
                        Status = application?.CollageApproved ?? false
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
                    User = _context.Users.FirstOrDefault(u => u.UserID == id),
                    ApplicationViewModel1 = new ApplicationViewModel
                    {
                        ApplyDate = application.SubmittedAt,
                        SelectedSubjects = application?.SelectedSubjects?.Split(',').Select(int.Parse).ToArray() ?? new int[0],
                        College = application?.Collage ?? string.Empty,
                        PaymentImage = application?.PaymentImagePath ?? string.Empty,
                        Semester = 1,
                        Status = application?.CollageApproved ?? false
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

        [HttpPost]
        public IActionResult ApproveUserDetails(int? userId, bool ApprovePersonal, bool ApproveEducational, bool ApproveContact)
        {
            var personalDetails = _context.PersonalDetails.FirstOrDefault(pd => pd.UserID == userId);
            var educationalDetails = _context.EducationalDetails.FirstOrDefault(ed => ed.UserID == userId);
            var contactDetails = _context.ContactDetails.FirstOrDefault(cd => cd.UserID == userId);

            if (personalDetails != null && ApprovePersonal)
            {
                personalDetails.IsDeanApproved = true;
            }

            if (educationalDetails != null && ApproveEducational)
            {
                educationalDetails.IsDeanApproved = true;
            }

            if (contactDetails != null && ApproveContact)
            {
                contactDetails.IsDeanApproved = true;
            }

            _context.SaveChanges();

            return RedirectToAction("Users");
        }










        public IActionResult Notes(string searchQuery)
        {
            var selectedCollege = HttpContext.Session.GetString("SelectedCollege");
            ViewData["ActiveSection"] = "Notes";
            var semesters = _context.Semesters
                                     .Include(s => s.Subjects)
                                     .ThenInclude(sub => sub.Notes)
                                     .ToList();

            if (!semesters.Any())
            {
                // Initialize an empty list of semesters if none exist
                semesters = new List<Semester>();
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();

                // Filter semesters, subjects, and notes based on the search query
                semesters = semesters
                    .Where(s => s.Subjects.Any(sub =>
                        sub.Name.ToLower().Contains(searchQuery) ||
                        sub.Notes.Any(n => n.FileName.ToLower().Contains(searchQuery))
                    ))
                    .ToList();

                // Filter the subjects and notes within the filtered semesters
                foreach (var semester in semesters)
                {
                    semester.Subjects = semester.Subjects
                        .Where(sub =>
                            sub.Name.ToLower().Contains(searchQuery) ||
                            sub.Notes.Any(n => n.FileName.ToLower().Contains(searchQuery))
                        )
                        .ToList();
                }
            }

            return View(semesters);
        }





        [HttpPost]
        public IActionResult AddNote(int subjectId, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {

                // Define the path to the "notes" directory
                var notesDirectory = Path.Combine(_hostEnvironment.WebRootPath, "notes");

                // Ensure the directory exists; if not, create it
                if (!Directory.Exists(notesDirectory))
                {
                    Directory.CreateDirectory(notesDirectory);
                }

                var filePath = Path.Combine(_hostEnvironment.WebRootPath, "notes", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var note = new Note
                {
                    FileName = file.FileName,
                    FilePath = $"/notes/{file.FileName}",
                    SubjectId = subjectId
                };

                _context.Notes.Add(note);
                _context.SaveChanges();
            }

            return RedirectToAction("Notes");
        }

    }
    



    
}
