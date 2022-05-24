namespace Core.ViewModels.User
{
    public record UserReadViewModel : UserBaseViewModel
    {
        public int Id { get; init; }
    }
}