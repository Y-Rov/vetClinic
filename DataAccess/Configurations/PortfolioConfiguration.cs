using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class PortfolioConfiguration : IEntityTypeConfiguration<Portfolio>
    {
        public void Configure(EntityTypeBuilder<Portfolio> builder)
        {
            builder
                .HasKey(portfolio => portfolio.UserId);

            builder
                .Property(portfolio => portfolio.Description)
                .HasMaxLength(2048)
                .IsRequired();

            builder
                .HasOne<User>(portfolio => portfolio.User)
                .WithOne(user => user.Portfolio)
                .HasForeignKey<Portfolio>(portfolio => portfolio.UserId);
            
            builder.ToTable("Portfolios");
        }
    }

}