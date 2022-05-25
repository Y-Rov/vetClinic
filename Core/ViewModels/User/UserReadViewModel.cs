﻿using Microsoft.AspNetCore.Identity;

namespace Core.ViewModels.User
{
    public record UserReadViewModel : UserBaseViewModel
    {
        public int Id { get; init; }
        public string? Role { get; init; }
    }
}