﻿using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class User : IdentityUser<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string? ProfilePicture { get; set; }

        public Address? Address { get; set; }
        public Portfolio? Portfolio { get; set; }

        public IEnumerable<Salary> Salaries { get; set; } = new List<Salary>();
        public IEnumerable<Animal> Animals { get; set; } = new List<Animal>();
        public IEnumerable<Article> Articles { get; set; } = new List<Article>();
        public IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
        public IEnumerable<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public IEnumerable<AppointmentUser> AppointmentUsers { get; set; } = new List<AppointmentUser>();
        public IEnumerable<UserSpecialization> UserSpecializations { get; set; } = new List<UserSpecialization>();
        public IEnumerable<Message> Messages { get; set; } = new List<Message>();
        public IEnumerable<UserChatRoom> UserChatRooms { get; set; } = new List<UserChatRoom>();
    }
}
