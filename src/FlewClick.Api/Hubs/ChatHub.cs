using System.Security.Claims;
using FlewClick.Application.Features.Chat.SendMessage;
using FlewClick.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FlewClick.Api.Hubs;

[Authorize]
public class ChatHub(IMediator mediator) : Hub
{
    public async Task JoinConversation(string conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
    }

    public async Task LeaveConversation(string conversationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
    }

    public async Task SendMessage(Guid bookingRequestId, string content)
    {
        var userId = GetUserId();
        var result = await mediator.Send(new SendMessageCommand(
            bookingRequestId, userId, MessageSenderType.Consumer, content));

        await Clients.Group(bookingRequestId.ToString()).SendAsync("ReceiveMessage", result);
    }

    public async Task TypingIndicator(string conversationId)
    {
        var userId = GetUserId();
        await Clients.OthersInGroup(conversationId).SendAsync("UserTyping", userId);
    }

    private Guid GetUserId()
    {
        var sub = Context.User?.FindFirstValue("sub")
            ?? throw new HubException("Invalid token.");
        return Guid.Parse(sub);
    }
}
