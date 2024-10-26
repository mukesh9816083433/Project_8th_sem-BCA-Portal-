using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Data;
using Project.ViewModels;
using System.Threading.Tasks;
using Project.DataAccess.Data;
using Project.Models;
using System.Security.Claims;


namespace Project.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;


        public ProfileController(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;

        }


        public IActionResult Personal()
        {
            ViewData["ActiveSection"] = "Profile";
            var user = _context.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            var userId = _userService.GetUserId();
            PersonalDetails? personalDetailsFromDb = _context.PersonalDetails.FirstOrDefault(defaultValue => defaultValue.UserID == userId);
            if (user == null)
            {
                return RedirectToAction("Campuses", "Home");
            }


            // Add logic for other recommendations as needed.

            var model = new DashboardViewModel
            {
                User = user,
                PersonalDetails = personalDetailsFromDb,
                // Add other recommendations to the model.
            };

            return View(model);



        }

        [HttpPost]
        public async Task<IActionResult> Personal(PersonalDetails model, IFormFile PhotoPath)
        {
            var userId = _userService.GetUserId();

            // Fetch the existing record from the database
            var existingPersonalDetails = _context.PersonalDetails.SingleOrDefault(p => p.UserID == userId);

            if (existingPersonalDetails != null)
            {
                // Update the existing record
                existingPersonalDetails.FirstName = model.FirstName;
                existingPersonalDetails.LastName = model.LastName;
                existingPersonalDetails.Gender = model.Gender;
                existingPersonalDetails.DateOfBirth = model.DateOfBirth;
                existingPersonalDetails.IsApproved = false;  // Reset the approval status

                if (PhotoPath != null && PhotoPath.Length > 0)
                {
                    var fileName = Path.GetFileName(PhotoPath.FileName);
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    // Ensure the directory exists
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    var filePath = Path.Combine(uploadDir, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await PhotoPath.CopyToAsync(fileStream);
                    }

                    existingPersonalDetails.PhotoPath = "/images/" + fileName; // Save relative path to the database
                }
            }
            else
            {
                // Create a new record if none exists
                model.UserID = userId;
                model.IsApproved = false;

                if (PhotoPath != null && PhotoPath.Length > 0)
                {
                    var fileName = Path.GetFileName(PhotoPath.FileName);
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    // Ensure the directory exists
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    var filePath = Path.Combine(uploadDir, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await PhotoPath.CopyToAsync(fileStream);
                    }

                    model.PhotoPath = "/images/" + fileName; // Save relative path to the database
                }
                else
                {
                    ModelState.AddModelError("PhotoPath", "Please upload a photo.");
                    return View(model); // Return with an error if no photo is uploaded
                }

                _context.PersonalDetails.Add(model);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Educational");

            /*var user1 = _context.Users.SingleOrDefault(u => u.Username == User.Identity.Name);

              if (user1 == null)
              {
                  return RedirectToAction("Campuses", "Home");
              }


            // Add logic for other recommendations as needed.

            var model1 = new DashboardViewModel
            {
                User = user1,
                // Add other recommendations to the model.
            };
            return View(model);*/
        }

        [HttpGet]
        public IActionResult Educational()
        {
            ViewData["ActiveSection"] = "Profile";
            var userId = _userService.GetUserId();
            var user = _context.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            var educationalDetails = _context.EducationalDetails.FirstOrDefault(e => e.UserID == userId);

            if (user == null)
            {
                return RedirectToAction("Campuses", "Home");
            }

            var model = new DashboardViewModel
            {
                User = user,
                EducationalDetails = educationalDetails,
                // Add other properties to the model as needed
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Educational(EducationalDetails model, IFormFile Marksheet10, IFormFile Marksheet12)
        {
            var userId = _userService.GetUserId();

            var educationalDetailsFromDb = _context.EducationalDetails.FirstOrDefault(e => e.UserID == userId);

            if (educationalDetailsFromDb != null)
            {
                // Update existing record
                educationalDetailsFromDb.Level = model.Level;
                educationalDetailsFromDb.College = model.College;
                educationalDetailsFromDb.Programme = model.Programme;
                educationalDetailsFromDb.AcademicYear = model.AcademicYear;
                educationalDetailsFromDb.RegistrationNumber = model.RegistrationNumber;
                educationalDetailsFromDb.IsApproved = false;
                if (Marksheet10 != null && Marksheet10.Length > 0)
                {
                    var fileName = Path.GetFileName(Marksheet10.FileName);
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    // Ensure the directory exists
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    var filePath = Path.Combine(uploadDir, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Marksheet10.CopyToAsync(fileStream);
                    }

                    educationalDetailsFromDb.MarksheetPic10 = "/images/" + fileName; // Save relative path to the database
                }
                if (Marksheet12 != null && Marksheet12.Length > 0)
                {
                    var fileName = Path.GetFileName(Marksheet12.FileName);
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    // Ensure the directory exists
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    var filePath = Path.Combine(uploadDir, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Marksheet12.CopyToAsync(fileStream);
                    }

                    educationalDetailsFromDb.MaeksheetPic12 = "/images/" + fileName; // Save relative path to the database
                }
                _context.EducationalDetails.Update(educationalDetailsFromDb);
            }
            else
            {
                // Create new record
                model.UserID = userId;
                model.IsApproved = false;
                if (Marksheet10 != null && Marksheet10.Length > 0)
                {
                    var fileName = Path.GetFileName(Marksheet10.FileName);
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    // Ensure the directory exists
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    var filePath = Path.Combine(uploadDir, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Marksheet10.CopyToAsync(fileStream);
                    }

                    model.MarksheetPic10 = "/images/" + fileName; // Save relative path to the database
                }
                else
                {
                    ModelState.AddModelError("PhotoPath", "Please upload a photo.");
                    return View(model); // Return with an error if no photo is uploaded
                }
                if (Marksheet12 != null && Marksheet12.Length > 0)
                {
                    var fileName = Path.GetFileName(Marksheet12.FileName);
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    // Ensure the directory exists
                    if (!Directory.Exists(uploadDir))
                    {
                        Directory.CreateDirectory(uploadDir);
                    }

                    var filePath = Path.Combine(uploadDir, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await Marksheet12.CopyToAsync(fileStream);
                    }

                    model.MaeksheetPic12 = "/images/" + fileName; // Save relative path to the database
                }
                else
                {
                    ModelState.AddModelError("PhotoPath", "Please upload a photo.");
                    return View(model); // Return with an error if no photo is uploaded
                }
                _context.EducationalDetails.Add(model);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Contact");
            /*  var user1 = _context.Users.SingleOrDefault(u => u.Username == User.Identity.Name);

              if (user1 == null)
              {
                  return RedirectToAction("Campuses", "Home");
              }


              // Add logic for other recommendations as needed.

              var model1 = new DashboardViewModel
              {
                  User = user1,
                  // Add other recommendations to the model.
              };
              return View(model);*/
        }


        [HttpGet]
        public IActionResult Contact()
        {
            ViewData["ActiveSection"] = "Profile";
            var user = _context.Users.SingleOrDefault(u => u.Username == User.Identity.Name);
            var userId = _userService.GetUserId();

            if (user == null)
            {
                return RedirectToAction("Campuses", "Home");
            }

            var contactDetailsFromDb = _context.ContactDetails.FirstOrDefault(cd => cd.UserID == userId);

            var model = new DashboardViewModel
            {
                User = user,
                ContactDetails = contactDetailsFromDb,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Contact(ContactDetails model)
        {
            var userId = _userService.GetUserId();

            var existingContactDetails = _context.ContactDetails.FirstOrDefault(cd => cd.UserID == userId);

            if (existingContactDetails != null)
            {
                if (!existingContactDetails.IsApproved)
                {
                    existingContactDetails.District = model.District;
                    existingContactDetails.PermanentAddress = model.PermanentAddress;
                    existingContactDetails.ContactAddress = model.ContactAddress;
                    existingContactDetails.ContactPhoneNo = model.ContactPhoneNo;
                    existingContactDetails.IsApproved = false;

                    _context.ContactDetails.Update(existingContactDetails);
                }
            }
            else
            {
                model.UserID = userId;
                model.IsApproved = false;
                _context.ContactDetails.Add(model);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Dashboard");
        }

    }
}