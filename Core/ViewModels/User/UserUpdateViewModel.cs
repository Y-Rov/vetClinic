namespace Core.ViewModels.User
{
    public record UserUpdateViewModel : UserBaseViewModel
    {
        public string ProfilePicture { get; init; } = string.Empty;
    }
}