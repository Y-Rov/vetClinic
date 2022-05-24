namespace Core.Entities
{
    public class Portfolio
    {
        public string? Description { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
