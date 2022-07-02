using Core.Entities;

namespace Core.ViewModels.User
{
    public record UserReadViewModel : UserBaseViewModel
    {
        public int Id { get; init; }
        public string? Role { get; init; }
        public string? Email { get; init; }
        public string? Address { get; init; }
        public string? ProfilePicture { get; init; }
        public Portfolio? Portfolio { get; init; }
        public IEnumerable<Specialization?>? Specializations { get; init; }
    }
}