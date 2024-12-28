using BerberApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BerberApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Staff> Staffes { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<User> Users { get; set; } 

        
    }
}
