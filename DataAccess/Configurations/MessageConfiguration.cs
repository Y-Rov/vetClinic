using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);

        builder
            .Property(m => m.Text).HasMaxLength(600);

        builder.HasOne(m => m.Sender)
            .WithMany(s => s.Messages)
            .HasForeignKey(m => m.SenderId);

        builder.HasOne(m => m.ChatRoom)
            .WithMany(cr => cr.Messages)
            .HasForeignKey(m => m.ChatRoomId);

        builder.ToTable("Messages");
    }
}