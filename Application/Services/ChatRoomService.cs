using Core.Emuns;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ChatRoomService : IChatRoomService
{
    private readonly IChatRoomRepository _chatRoomRepository;
    private readonly IUserChatRoomRepository _userChatRoomRepository;
    private readonly IUserService _userService;

    public ChatRoomService(
        IChatRoomRepository chatRoomRepository, 
        IUserChatRoomRepository userChatRoomRepository,
        IUserService userService)
    {
        _chatRoomRepository = chatRoomRepository;
        _userChatRoomRepository = userChatRoomRepository;
        _userService = userService;
    }
    
    public async Task CreateAsync(ChatRoom chatRoom)
    {
        await _chatRoomRepository.InsertAsync(chatRoom);
    }

    public async Task<IEnumerable<ChatRoom>> GetChatRoomsForUserAsync(int userId)
    {
        return await _chatRoomRepository.QueryAsync(
            include: q => q.Include(cr => cr.UserChatRooms),
            filter: cr => cr.UserChatRooms.Select(ur => ur.UserId).Contains(userId));
    }

    public async Task<IEnumerable<User>> GetParticipantsAsync(int chatRoomId)
    {
        var userChatRooms = await _userChatRoomRepository.QueryAsync(
            filter: ur => ur.ChatRoomId == chatRoomId,
            include: q => q.Include(ur => ur.User),
            asNoTracking: true);
        return userChatRooms.Select(ur => ur.User);
    }

    public async Task<UserChatRoom?> GetUserChatRoomAsync(int userId, int chatRoomId)
    {
        return await _userChatRoomRepository.GetFirstOrDefaultAsync(
            filter: ur => 
                ur.UserId == userId 
                && ur.ChatRoomId == chatRoomId);
    }
    
    public async Task AddMemberAsync(int chatRoomId, int userId)
    {
        if (!await _chatRoomRepository.ExistsAsync(chatRoomId))
            throw new NotFoundException($"Chatroom with id {chatRoomId} does not exist");

        var userChatRoom = new UserChatRoom()
        {
            UserId = userId,
            ChatRoomId = chatRoomId
        };
        await _userChatRoomRepository.InsertAsync(userChatRoom);
    }

    public async Task KickMemberAsync(int roomId, int userId)
    {
        var userChatRoom = await _userChatRoomRepository.GetFirstOrDefaultAsync(
            filter: ur => ur.ChatRoomId == roomId && ur.UserId == userId);

        if (userChatRoom is null)
            throw new NotFoundException($"User {userId} is not a chat member");

        _userChatRoomRepository.Delete(userChatRoom);
        await _userChatRoomRepository.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _chatRoomRepository.ExistsAsync(id);
    } 
    
    public async Task<ChatRoom> EnsurePrivateRoomCreatedAsync(int memberId1, int memberId2)
    {
        ChatRoom? chatRoom;
        chatRoom = await _chatRoomRepository.GetFirstOrDefaultAsync(
            include: q => q.Include(cr => cr.UserChatRooms),
            filter: cr =>
                cr.UserChatRooms.Select(ur => ur.UserId).Contains(memberId1)
                && cr.UserChatRooms.Select(ur => ur.UserId).Contains(memberId2)
                && cr.Type == ChatType.Private);

        if (chatRoom is null)
        {
            chatRoom = new ChatRoom()
            {
                Type = ChatType.Private,
                UserChatRooms = new[]
                {
                    new UserChatRoom() { UserId = memberId1 },
                    new UserChatRoom() { UserId = memberId2 }
                }
            };
            await CreateAsync(chatRoom);
        }
        
        return chatRoom;
    } 
}