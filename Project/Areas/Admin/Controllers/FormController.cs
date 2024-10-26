using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DataAccess.Data;
using Project.Models;
using System.Net.Mail;
using System.Net;

namespace Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FormController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public FormController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _hostEnvironment = webHostEnvironment;
        }
        public IActionResult Applications()
        {
            ViewData["ActiveSection"] = "Applications";
            var forms = _context.Forms.ToList();
            return View(forms);
        }



        public IActionResult FormApplications(int formId)
        {
            ViewData["ActiveSection"] = "Applications";
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
            ViewData["ActiveSection"] = "Applications";
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
                        Status2 = application.DeanApproved,
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
                        Status2 = application.DeanApproved,
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
                application.DeanApproved = true;

                // Save changes to the database
                _context.SaveChanges();

                // Redirect to the application details page or another page
                return RedirectToAction("FormApplicationViewDetail","Form", new { id = id, sem = sem });
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
                application.DeanApproved = true;

                // Save changes to the database
                _context.SaveChanges();

                // Redirect to the application details page or another page
                return RedirectToAction("FormApplicationViewDetail", "Form", new { id = id, sem = sem });
            }
            else
            {
                return RedirectToAction("FormApplicationViewDetail", "Form", new { id = id, sem = sem });
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
                return RedirectToAction("FormApplications", "Form", new { formid = application.Id }); // Redirect to an appropriate page after rejecting the form
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
                return RedirectToAction("FormApplications", "Form", new { formid = application.Id }); // Redirect to an appropriate page after rejecting the form
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





        [HttpPost]
        public IActionResult OpenForm(int formId)
        {
            var form = _context.Forms.Find(formId);
            if (form != null)
            {
                form.IsOpen = true;
                _context.SaveChanges();
            }
            return RedirectToAction("Applications","Form");
        }

        [HttpPost]
        public IActionResult CloseForm(int formId)
        {
            var form = _context.Forms.Find(formId);
            if (form != null)
            {
                form.IsOpen = false;
                _context.SaveChanges();
            }
            return RedirectToAction("Applications", "Form");
        }




    }
}
