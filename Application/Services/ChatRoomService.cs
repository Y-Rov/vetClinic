using Core.Emuns;
using Core.Entities;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class ChatRoomService : IChatRoomService
{
    private readonly IChatRoomRepository _chatRoomRepository;
    private readonly IUserChatRoomRepository _userChatRoomRepository;
    private readonly IUserService _userService;
    private readonly ILoggerManager _loggerManager;
    
    public ChatRoomService(
        IChatRoomRepository chatRoomRepository, 
        IUserChatRoomRepository userChatRoomRepository,
        IUserService userService, 
        ILoggerManager loggerManager)
    {
        _chatRoomRepository = chatRoomRepository;
        _userChatRoomRepository = userChatRoomRepository;
        _userService = userService;
        _loggerManager = loggerManager;
    }
    
    public async Task CreateAsync(ChatRoom chatRoom)
    {
        await _chatRoomRepository.InsertAsync(chatRoom);
        await _chatRoomRepository.SaveChangesAsync();
        _loggerManager.LogInfo($"Created new chat room with id {chatRoom.Id}");
    }

    public async Task<IEnumerable<ChatRoom>> GetChatRoomsForUserAsync(int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        if (user is null)
        {
            _loggerManager.LogWarn($"User with id {userId} does not exist");
            throw new NotFoundException($"User with id {userId} does not exist");
        }

        var chatRooms = await _chatRoomRepository.QueryAsync(
            include: q => q.Include(cr => cr.UserChatRooms)
                            .ThenInclude(ur => ur.User),
            filter: cr => cr.UserChatRooms.Select(ur => ur.UserId).Contains(userId));
        
        _loggerManager.LogInfo($"Returned all chats for user with id {userId}");
        return chatRooms;
    }

    public async Task<UserChatRoom?> GetUserChatRoomAsync(int userId, int chatRoomId)
    {
        return await _userChatRoomRepository.GetFirstOrDefaultAsync(
            filter: ur => 
                ur.UserId == userId 
                && ur.ChatRoomId == chatRoomId);
    }
    
    public async Task<ChatRoom> EnsurePrivateRoomCreatedAsync(int memberId1, int memberId2)
    {
        var (member1, member2) = (await _userService.GetUserByIdAsync(memberId1), 
                                 await _userService.GetUserByIdAsync(memberId2));
        if (member1 is null)
        {
            _loggerManager.LogWarn($"User with id {memberId1} does not exist");
            throw new NotFoundException($"User with id {memberId1} does not exist");
        }
        if (member2 is null)
        {
            _loggerManager.LogWarn($"User with id {memberId2} does not exist");
            throw new NotFoundException($"User with id {memberId2} does not exist");
        }

        var chatRoom = await _chatRoomRepository.GetFirstOrDefaultAsync(
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

    public async Task<bool> ExistsAsync(int id)
    {
        return await _chatRoomRepository.ExistsAsync(id);
    }
}