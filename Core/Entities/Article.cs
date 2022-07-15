namespace Core.Entities;

public class Article
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Body { get; set; }
    public DateTime CreatedAt { get; set; }
    public int AuthorId { get; set; }
    public User? Author { get; set; }
    public bool Published { get; set; }
    public bool Edited { get; set; }
    public IEnumerable<Comment> Comments = new List<Comment>();
}