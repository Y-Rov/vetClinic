﻿using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class ClinicContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Animal> Animals { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<UserSpecialization> UserSpecializations { get; set; }
        public DbSet<AppointmentProcedure> AppointmentProcedures { get; set; }
        public DbSet<AppointmentUser> AppointmentUsers { get; set; }
        public DbSet<ExceptionEntity> Exceptions { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<ProcedureSpecialization> ProcedureSpecializations { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<UserChatRoom> UserChatRooms { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        public ClinicContext(DbContextOptions<ClinicContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            builder.SeedRoles();
            builder.SeedAdmin();
        }
    }
}
