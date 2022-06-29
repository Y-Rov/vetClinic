using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder
            .HasKey(comment => comment.Id);

        builder
            .Property(comment => comment.Content)
            .HasMaxLength(1000);

        builder
            .HasOne<User>(comment => comment.Author)
            .WithMany(user => user.Comments)
            .HasForeignKey(comment => comment.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<Article>(comment => comment.Article)
            .WithMany(art => art.Comments)
            .HasForeignKey(comment => comment.ArticleId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder
            .ToTable("Comments");
    }
}