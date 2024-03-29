﻿namespace Core.Entities
{
    public class Animal
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string? NickName { get; set; }
        public string? PhotoUrl { get; set; }
        public DateTime BirthDate { get; set; }

        public User Owner { get; set; }
        public IEnumerable<Appointment>? Appointments { get; set; }
    }
}
