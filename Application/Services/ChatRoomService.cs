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

    public async Task<IEnumerable<User>> GetParticipantsAsync(int chatRoomId)
    {
        return await _userChatRoomRepository.Query(
                filter: ur => ur.ChatRoomId == chatRoomId,
                include: q => q.Include(ur => ur.User),
                asNoTracking: true)
            .Select(ur => ur.User)
            .ToListAsync();
    }

    public async Task AddMemberAsync(int roomId, int userId)
    {
        if (!await _chatRoomRepository.ExistsAsync(roomId))
            throw new NotFoundException($"Chatroom with id {roomId} does not exist");

        var userChatRoom = new UserChatRoom()
        {
            UserId = userId,
            ChatRoomId = roomId
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
}