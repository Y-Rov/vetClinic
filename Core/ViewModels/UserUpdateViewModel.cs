﻿namespace Core.ViewModel
{
    public record UserUpdateViewModel
    {
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Email { get; init; }
        public string? PhoneNumber { get; init; }
        public DateTime BirthDate { get; init; }
    }
}