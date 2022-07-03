namespace Core.Models
{
    public record CollateParameters
    {
        public string? FilterParam { get; init; }
        public string? OrderByParam { get; init; }
        public int? TakeCount { get; init; }
        public int SkipCount { get; init; } = 0;
    }
}
