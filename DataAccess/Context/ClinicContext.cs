using Core.Entities;
using DataAccess.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    public class ClinicContext : IdentityDbContext<User, IdentityRole<int>,int>
    {
        //public DbSet<Address> Addresses { get; set; }
        public DbSet<Animal> Animals { get; set; }
        //public DbSet<Appointment> Appointments { get; set; }
        //public DbSet<Portfolio> Portfolios { get; set; }
        //public DbSet<Procedure> Procedures { get; set; }
        //public DbSet<Salary> Salaries { get; set; }
        //public DbSet<Specialization> Specializations { get; set; }

        public ClinicContext(DbContextOptions<ClinicContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
