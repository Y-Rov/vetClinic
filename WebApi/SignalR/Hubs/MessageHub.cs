using Core.Entities;
using Core.Interfaces.Services;
using Core.ViewModel.MessageViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebApi.AutoMapper.Interface;

namespace WebApi.SignalR.Hubs;

[Authorize]
public class MessageHub : Hub
{ }