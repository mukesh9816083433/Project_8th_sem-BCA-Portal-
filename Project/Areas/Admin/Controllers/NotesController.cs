using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DataAccess.Data;
using Project.Models;

namespace Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public NotesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _hostEnvironment = webHostEnvironment;
        }

        public IActionResult Notes(string searchQuery)
        {
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
        public IActionResult AddSemester(string semesterName)
        {
            if (!string.IsNullOrEmpty(semesterName))
            {
                var semester = new Semester { Name = semesterName };
                _context.Semesters.Add(semester);
                _context.SaveChanges();
            }

            return RedirectToAction("Notes","Notes");
        }

        [HttpPost]
        public IActionResult AddSubject(int semesterId, string subjectName)
        {
            if (semesterId > 0 && !string.IsNullOrEmpty(subjectName))
            {
                var subject = new Subject { Name = subjectName, SemesterId = semesterId };
                _context.Subjects.Add(subject);
                _context.SaveChanges();
            }

            return RedirectToAction("Notes", "Notes");
        }
        [HttpPost]
        public IActionResult DeleteSubject(int subjectId)
        {
            // Find the subject by its ID
            var subject = _context.Subjects
                                  .Include(s => s.Notes)
                                  .FirstOrDefault(s => s.Id == subjectId);

            if (subject != null)
            {
                // Delete associated notes
                foreach (var note in subject.Notes)
                {
                    var filePath = Path.Combine(_hostEnvironment.WebRootPath, "notes", note.FileName);

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    _context.Notes.Remove(note);
                }

                // Delete the subject
                _context.Subjects.Remove(subject);
                _context.SaveChanges();
            }

            return RedirectToAction("Notes", "Notes");
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

            return RedirectToAction("Notes", "Notes");
        }

        [HttpPost]
        public IActionResult DeleteNote(int noteId)
        {
            var note = _context.Notes.Find(noteId);

            if (note != null)
            {
                var filePath = Path.Combine(_hostEnvironment.WebRootPath, "notes", note.FileName);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                _context.Notes.Remove(note);
                _context.SaveChanges();
            }

            return RedirectToAction("Notes", "Notes");
        }


    }
}
