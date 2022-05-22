﻿using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class ClinicContext : IdentityDbContext<User, IdentityRole<int>,int>
    {
        public DbSet<Animal> Animals { get; set; }
        
        public ClinicContext(DbContextOptions<ClinicContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            modelBuilder.SeedRoles();
            modelBuilder.SeedAdmin();
        }
    }
}
