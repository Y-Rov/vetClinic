using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public static class DataSeeder
{
    public static void SeedRoles(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole<int>>().HasData(new[]
        {
            new IdentityRole<int>() {Id = 1, Name = "Admin", NormalizedName = "Admin".ToUpper()},
            new IdentityRole<int>() {Id = 2, Name = "Doctor", NormalizedName = "Doctor".ToUpper()},
            new IdentityRole<int>() {Id = 3, Name = "Accountant", NormalizedName = "Accountant".ToUpper()},
            new IdentityRole<int>() {Id = 4, Name = "Client", NormalizedName = "Client".ToUpper()}
        });
    }

    public static void SeedAdmin(this ModelBuilder modelBuilder)
    {
        var hasher = new PasswordHasher<User>();
        var admin = new User()
        {
            Id = 1,
            UserName = "Admin",
            NormalizedUserName = "Admin".ToUpper(),
            FirstName = "AdminFirstName",
            
        };
        admin.PasswordHash = hasher.HashPassword(admin, "Admin_password123");
        // seed admin
        modelBuilder.Entity<User>().HasData(admin);
        // assign "Admin" role 
        modelBuilder.Entity<IdentityUserRole<int>>().HasData(new IdentityUserRole<int>()
        {
            RoleId = 1,
            UserId = 1
        });
    }
}