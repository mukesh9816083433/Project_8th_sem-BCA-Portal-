using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<PersonalDetails> PersonalDetails { get; set; }
        public DbSet<EducationalDetails> EducationalDetails { get; set; }
        public DbSet<ContactDetails> ContactDetails { get; set; }
        public DbSet<Campus> Campuses { get; set; }

        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Note> Notes { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Campus>().HasData(
                new Campus
                {
                    CampusID = 1,
                    CampusName = "Mechi Multiple Campus",
                    Province = "Province 1",
                    Address = "Bhadrapur, Jhapa",
                    CampusChief = "Jiwan Pokhrel",
                    ContactInfo = "9852655607",
                    LoginCode=11111
                },
                new Campus
                {
                    CampusID = 2,
                    CampusName = "Mahendra Ratna Multiple Campus",
                    Province = "Province 1",
                    Address = "Illam",
                    CampusChief = "Dinanath Phuyal",
                    ContactInfo = "9812345678",
                    LoginCode = 22222
                },
                new Campus
                {
                    CampusID = 3,
                    CampusName = "Jwala Prasad Syo Wai Devi Murarka Campus",
                    Province = "Province 2",
                    Address = "Lahan",
                    CampusChief = "Tulsi Ram Pokhrel",
                    ContactInfo = "9812365855",
                    LoginCode = 33333
                },
                new Campus
                {
                    CampusID = 4,
                    CampusName = "Bhaktapur Multiple Campus",
                    Province = "Province 3",
                    Address = "Bhaktapur",
                    CampusChief = "Krishna Pokhrel",
                    ContactInfo = "990000045678",
                    LoginCode = 44444
                }
            );
            modelBuilder.Entity<Forms>().HasData(
       new Forms { Id = 1, Name = "Entrance Form", IsOpen = false },
       new Forms { Id = 2, Name = "Registration Form", IsOpen = false },
       new Forms { Id = 3, Name = "1st Sem Exam Form", IsOpen = false },
       new Forms { Id = 4, Name = "2nd Sem Exam Form", IsOpen = false },
       new Forms { Id = 5, Name = "3rd Sem Exam Form", IsOpen = false },
       new Forms { Id = 6, Name = "4th Sem Exam Form", IsOpen = false },
       new Forms { Id = 7, Name = "5th Sem Exam Form", IsOpen = false },
       new Forms { Id = 8, Name = "6th Sem Exam Form", IsOpen = false },
       new Forms { Id = 9, Name = "7th Sem Exam Form", IsOpen = false },
       new Forms { Id = 10, Name = "8th Sem Exam Form", IsOpen = false }
   );
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Forms> Forms { get; set; }
        public DbSet<EightSemExamForm> EighthSemExamForms { get; set; }
        public DbSet<FirstSemExamForm> FirstSemExamForms { get; set; }
        public DbSet<SecondSemExamForm> SecondSemExamForms { get; set; }
        public DbSet<EntranceForm> EntranceForms { get; set; }

        public DbSet<Notice> Notice { get; set; }



    }
}
