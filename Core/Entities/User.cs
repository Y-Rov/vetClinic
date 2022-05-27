using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class User : IdentityUser<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsActive { get; set; } = true;

        public Salary? Salary { get; set; }
        public Address? Address { get; set; }
        public Portfolio? Portfolio { get; set; }

        public IEnumerable<Animal>? Animals { get; set; } = new List<Animal>();
        public IEnumerable<AppointmentUser>? AppointmentUsers { get; set; } = new List<AppointmentUser>();
        public IEnumerable<UserSpecialization>? UserSpecializations { get; set; } = new List<UserSpecialization>();
    }
}
