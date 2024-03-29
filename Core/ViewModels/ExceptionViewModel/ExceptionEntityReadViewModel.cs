﻿namespace Core.ViewModels.ExceptionViewModel
{
    public record ExceptionEntityReadViewModel
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public DateTime DateTime { get; init; }
        public string? Path { get; init; }

    }
}
