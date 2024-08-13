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
                    ContactInfo = "9852655607"
                },
                new Campus
                {
                    CampusID = 2,
                    CampusName = "Mahendra Ratna Multiple Campus",
                    Province = "Province 1",
                    Address = "Illam",
                    CampusChief = "Dinanath Phuyal",
                    ContactInfo = "9812345678"
                },
                new Campus
                {
                    CampusID = 3,
                    CampusName = "Jwala Prasad Syo Wai Devi Murarka Campus",
                    Province = "Province 2",
                    Address = "Lahan",
                    CampusChief = "Tulsi Ram Pokhrel",
                    ContactInfo = "9812365855"
                },
                new Campus
                {
                    CampusID = 4,
                    CampusName = "Bhaktapur Multiple Campus",
                    Province = "Province 3",
                    Address = "Bhaktapur",
                    CampusChief = "Krishna Pokhrel",
                    ContactInfo = "990000045678"
                }
            );
        }

        public DbSet<User> Users { get; set; }
    }
}
