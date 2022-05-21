namespace Core.Entities
{
    public class Portfolio
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
