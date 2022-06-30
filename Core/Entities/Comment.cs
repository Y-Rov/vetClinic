namespace Core.Entities;

public class Comment
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public User? Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? Content { get; set; }
    public bool Edited { get; set; }
    public int ArticleId { get; set; }
    public Article? Article { get; set; }
}