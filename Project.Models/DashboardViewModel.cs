using System.Collections.Generic;
using Project.Models.ViewModel;

namespace Project.Models
{
    public class DashboardViewModel
    {
        public User? User { get; set; }
        public PersonalDetails? PersonalDetails { get; set; }
        public EducationalDetails? EducationalDetails { get; set; }
        public ContactDetails? ContactDetails { get; set; }
        public List<Forms>? Forms { get; set; }
        public string? FormName { get; set; }

        public List<Campus>? RecommendedCampuses { get; set; }
        public List<Note>? RecommendedNotes { get; set; }

        public List<Notice>? RecommendedNotices { get; set; }
        // Add other properties for different recommendations as needed.
        public List<Subject>? EightSemesterSubjects { get; set; }
        public List<Subject>? FirstSemesterSubjects { get; set; }
        public List<Subject>? Subject { get; set; }   

        // Add the EighthSemExamFormViewModel as a property
        public EightSemExamFormViewModel? EightSemExamForm { get; set; }
        public FirstSemExamFormViewModel? FirstSemExamForm { get; set; }
        public List<ApplicationViewModel>? ApplicationViewModel { get; set; }
        public ApplicationViewModel? ApplicationViewModel1 { get; set; }

        public Note? SelectedNote { get; set; }
        public List<Note>? RelatedNotes { get; set; }
        public List<Note>? RelatedNotices { get; set; }


        public List<Notice>? Notices { get; set; }
            public string? SearchTerm { get; set; }
            public string? SelectedSemester { get; set; }
            public List<Semester> ?AllSemesters { get; set; }
        

    }
}
