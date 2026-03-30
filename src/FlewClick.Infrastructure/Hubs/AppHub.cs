using System.Security.Claims;
using FlewClick.Application.Features.Chat.SendMessage;
using FlewClick.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FlewClick.Infrastructure.Hubs;

[Authorize]
public class AppHub(IMediator mediator) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        // Join a personal room for private notifications (like "New Booking Request")
        await Groups.AddToGroupAsync(Context.ConnectionId, userId.ToString());
        await base.OnConnectedAsync();
    }

    public async Task JoinProject(string bookingId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, bookingId);
    }

    public async Task LeaveProject(string bookingId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, bookingId);
    }

    public async Task SendMessage(Guid bookingRequestId, string content)
    {
        var userId = GetUserId();
        var isProfessional = Context.User?.HasClaim(c => c.Type == "profileId") ?? false;
        var senderType = isProfessional ? MessageSenderType.Professional : MessageSenderType.Consumer;

        await mediator.Send(new SendMessageCommand(
            bookingRequestId, userId, senderType, content));
    }

    public async Task TypingIndicator(string bookingId)
    {
        var userId = GetUserId();
        await Clients.OthersInGroup(bookingId).SendAsync("UserTyping", userId);
    }

    private Guid GetUserId()
    {
        var sub = Context.User?.FindFirstValue("profileId")
            ?? Context.User?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? Context.User?.FindFirstValue("sub")
            ?? throw new HubException("Invalid token.");
        return Guid.Parse(sub);
    }
}
