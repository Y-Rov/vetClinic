using System.Security.Claims;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModels.ChatRoomViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.AutoMapper.Interface;

namespace WebApi.Controllers;

[ApiController]
[Authorize]
[Route("api/chats")]
public class ChatsController : ControllerBase
{
    private readonly IChatRoomService _chatRoomService;
    private readonly UserManager<User> _userManager;

    private readonly IUserOrientedEnumerableViewModelMapper<ChatRoom, ChatRoomGetViewModel> _enumChatRoomMapper;
    
    public ChatsController(
        IChatRoomService chatRoomService,
        IUserOrientedEnumerableViewModelMapper<ChatRoom, ChatRoomGetViewModel> enumChatRoomMapper,
        UserManager<User> userManager)
    {
        _chatRoomService = chatRoomService;
        _enumChatRoomMapper = enumChatRoomMapper;
        _userManager = userManager;
    }
    
    [HttpGet]
    public async Task<IEnumerable<ChatRoomGetViewModel>> Get()
    {
        var userId = (await _userManager.GetUserAsync(HttpContext.User)).Id;
        var chatRooms = await _chatRoomService.GetChatRoomsForUserAsync(userId);
        return _enumChatRoomMapper.Map(chatRooms, userId);
    }
}