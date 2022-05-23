namespace Core.DTO
{
    public record UserCreateDto : UserUpdateDto
    {
        public string? Password { get; init; }
        public string? ConfirmPassword { get; init; }
    }
}