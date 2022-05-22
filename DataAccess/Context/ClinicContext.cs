using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class ClinicContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentProcedure> AppointmentProcedures { get; set; }
        public DbSet<AppointmentUser> AppointmentUsers { get; set; }
        public DbSet<ExceptionEntity> Exceptions { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }

        public ClinicContext(DbContextOptions<ClinicContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
