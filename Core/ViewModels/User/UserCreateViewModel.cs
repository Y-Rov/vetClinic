namespace Core.ViewModels.User
{
    public record UserCreateViewModel : UserBaseViewModel
    {
        public string? Password { get; init; }
        public string? ConfirmPassword { get; init; }
    }
}