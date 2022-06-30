using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder
            .HasKey(art => art.Id);

        builder
            .Property(art => art.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Property(art => art.Body)
            .HasMaxLength(6000)
            .IsRequired();
        
        builder
            .Property(art => art.CreatedAt);

        builder
            .HasOne<User>(art => art.Author)
            .WithMany(user => user.Articles)
            .HasForeignKey(art => art.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("Articles");
    }
}