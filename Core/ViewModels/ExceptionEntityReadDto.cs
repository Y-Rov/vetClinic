namespace Core.ViewModel
{
    public record ExceptionEntityReadDto
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public DateTime DateTime { get; init; }
        public string? Path { get; init; }

    }
}
