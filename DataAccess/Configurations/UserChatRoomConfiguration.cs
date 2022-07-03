using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public class UserChatRoomConfiguration : IEntityTypeConfiguration<UserChatRoom>
{
    public void Configure(EntityTypeBuilder<UserChatRoom> builder)
    {
        builder.HasKey(userChatRoom => new { userChatRoom.UserId, userChatRoom.ChatRoomId });

        builder.HasOne(userChatRoom => userChatRoom.User)
            .WithMany(u => u.UserChatRooms)
            .HasForeignKey(userChatRoom => userChatRoom.UserId);

        builder.HasOne(userChatRoom => userChatRoom.ChatRoom)
            .WithMany(cr => cr.UserChatRooms)
            .HasForeignKey(userChatRoom => userChatRoom.ChatRoomId);

        builder.HasOne(userChatRoom => userChatRoom.LastReadMessage)
            .WithMany(m => m.LastReadByUsers)
            .HasForeignKey(userChatRoom => userChatRoom.LastReadMessageId);
    }
}