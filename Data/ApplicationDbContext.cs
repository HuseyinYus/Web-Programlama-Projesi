using Web_Programlama_Projesi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web_Programlama_Projesi.Models; // Modellerin olduğu namespace'i eklemeyi unutma

namespace Web_Programlama_Projesi.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // BU SATIRLARI EKLE:
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
    }
}