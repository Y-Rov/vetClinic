namespace Core.ViewModels.User
{
    public record UserCreateViewModel : UserBaseViewModel
    {
        public string? Email { get; init; }
        public string? ProfilePicture { get; init; }
        public string? Password { get; init; }
        public string? ConfirmPassword { get; init; }
    }
}