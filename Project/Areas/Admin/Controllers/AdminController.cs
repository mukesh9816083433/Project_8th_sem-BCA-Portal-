using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DataAccess.Data;
using Project.Models;
using Microsoft.AspNetCore.Hosting;
using Project.Models.ViewModel;
using System.Net.Mail;
using System.Net;

namespace Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdminController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _hostEnvironment = webHostEnvironment;
        }


        public IActionResult Campuses()
        {
            ViewData["ActiveSection"] = "Campuses";
            var campuses = _context.Campuses.ToList();
            return View(campuses);
        }


      

            public IActionResult Report()
            {
            // Get user statistics
            ViewData["ActiveSection"] = "Report";
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
        





        public IActionResult Users(string searchQuery)
        {
            ViewData["ActiveSection"] = "Users";
            var usersQuery = _context.Users.AsQueryable();

            // If a search query is provided, filter the users based on the username
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower();
                usersQuery = usersQuery.Where(u => u.Username.ToLower().Contains(searchQuery));


                // Project the filtered users into the UserDetailsViewModel
                var users1 = usersQuery
                    .Select(user1 => new UserDetailsViewModel
                    {
                        UserId = user1.UserID,
                        Username = user1.Username,
                        PersonalDetails = _context.PersonalDetails.FirstOrDefault(pd => pd.UserID == user1.UserID),
                        EducationalDetails = _context.EducationalDetails.FirstOrDefault(ed => ed.UserID == user1.UserID),
                        ContactDetails = _context.ContactDetails.FirstOrDefault(cd => cd.UserID == user1.UserID)
                    })
                    .ToList(); // Convert the query to a list

                return View(users1);
            }else
            {
                var users = _context.Users
                       .Select(user => new UserDetailsViewModel
                       {
                           UserId = user.UserID,
                           Username = user.Username,
                           PersonalDetails = _context.PersonalDetails.FirstOrDefault(pd => pd.UserID == user.UserID),
                           EducationalDetails = _context.EducationalDetails.FirstOrDefault(ed => ed.UserID == user.UserID),
                           ContactDetails = _context.ContactDetails.FirstOrDefault(cd => cd.UserID == user.UserID)
                       }).ToList();

                return View(users);
            }
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
                                        userId=f.UserId,
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
        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserID == id);

            if (user != null)
            {
                // Delete associated PersonalDetails
                var personalDetails = _context.PersonalDetails.FirstOrDefault(p => p.UserID == id);
                if (personalDetails != null)
                {
                    _context.PersonalDetails.Remove(personalDetails);
                }

                // Delete associated EducationalDetails
                var educationalDetails = _context.EducationalDetails.FirstOrDefault(e => e.UserID == id);
                if (educationalDetails != null)
                {
                    _context.EducationalDetails.Remove(educationalDetails);
                }

                // Delete associated ContactDetails
                var contactDetails = _context.ContactDetails.FirstOrDefault(c => c.UserID == id);
                if (contactDetails != null)
                {
                    _context.ContactDetails.Remove(contactDetails);
                }

                // Finally, delete the User
                _context.Users.Remove(user);

                // Save changes to the database
                _context.SaveChanges();
            }
            ViewData["ActiveSection"] = "Users";
            return RedirectToAction("Users");
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










       

    }
}
