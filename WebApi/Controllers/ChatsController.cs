using System.Security.Claims;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModel.ChatRoomViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/chats")]
public class ChatsController : ControllerBase
{
    private readonly int _userId;
    private readonly IChatRoomService _chatRoomService;
    
    private readonly IEnumerableViewModelMapper<IEnumerable<ChatRoom>, IEnumerable<ChatRoomGetViewModel>>
        _enumChatRoomMapper;
    
    public ChatsController(IChatRoomService chatRoomService, IEnumerableViewModelMapper<IEnumerable<ChatRoom>, IEnumerable<ChatRoomGetViewModel>> enumChatRoomMapper)
    {
        Int32.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out _userId);
        _chatRoomService = chatRoomService;
        _enumChatRoomMapper = enumChatRoomMapper;
    }
    
    [HttpGet]
    public async Task<IEnumerable<ChatRoomGetViewModel>> Get()
    {
        var chatRooms = await _chatRoomService.GetChatRoomsForUserAsync(_userId);
        return _enumChatRoomMapper.Map(chatRooms);

    }
}