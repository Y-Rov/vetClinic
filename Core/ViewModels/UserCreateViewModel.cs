namespace Core.ViewModel
{
    public record UserCreateViewModel : UserUpdateViewModel
    {
        public string? Password { get; init; }
        public string? ConfirmPassword { get; init; }
    }
}