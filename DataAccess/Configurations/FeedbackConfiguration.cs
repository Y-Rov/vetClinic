using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class FeedbackConfiguration 
        : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder
                .HasKey(feedback => feedback.Id);

            builder
                .HasOne(feedback => feedback.User)
                .WithMany(user => user.Feedbacks)
                .HasForeignKey(feedback => feedback.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Feedbacks");
        }
    }
}
