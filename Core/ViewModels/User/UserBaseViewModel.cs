namespace Core.ViewModels.User
{
    public record UserBaseViewModel
    {
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? PhoneNumber { get; init; }
        public DateTime BirthDate { get; init; }
    }
}