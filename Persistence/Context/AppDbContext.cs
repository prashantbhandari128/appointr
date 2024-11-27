using Appointr.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointr.Persistence.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        //---------------[ Set Here ]-----------------
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Officer> Officers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<WorkDay> WorkDays { get; set; }
        //--------------------------------------------
    }
}
