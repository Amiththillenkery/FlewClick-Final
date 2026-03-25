using System.Security.Claims;
using FlewClick.Application.Features.Chat.GetConversation;
using FlewClick.Application.Features.Chat.GetMessages;
using FlewClick.Application.Features.Chat.MarkMessagesRead;
using FlewClick.Application.Features.Chat.SendMessage;
using FlewClick.Domain.Enums;
using MediatR;

namespace FlewClick.Api.Endpoints.Chat;

public record SendMessageBody(string Content, MessageSenderType SenderType);

public class ChatEndpoint : IEndpointGroup
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/chat/{bookingId:guid}", async (IMediator mediator, ClaimsPrincipal user, Guid bookingId) =>
            {
                var requesterId = Guid.Parse(user.FindFirstValue("sub")
                    ?? throw new UnauthorizedAccessException("Invalid token."));
                var result = await mediator.Send(new GetConversationQuery(bookingId, requesterId));
                return Results.Ok(result);
            })
            .WithName("GetConversation")
            .WithTags("Chat")
            .RequireAuthorization();

        app.MapGet("/api/chat/{bookingId:guid}/messages", async (IMediator mediator, Guid bookingId, int page = 1, int pageSize = 50) =>
            {
                var result = await mediator.Send(new GetMessagesQuery(bookingId, page, pageSize));
                return Results.Ok(result);
            })
            .WithName("GetMessages")
            .WithTags("Chat")
            .RequireAuthorization();

        app.MapPost("/api/chat/{bookingId:guid}/messages", async (IMediator mediator, ClaimsPrincipal user, Guid bookingId, SendMessageBody body) =>
            {
                var senderId = Guid.Parse(user.FindFirstValue("sub")
                    ?? throw new UnauthorizedAccessException("Invalid token."));
                var result = await mediator.Send(new SendMessageCommand(
                    bookingId,
                    senderId,
                    body.SenderType,
                    body.Content));
                return Results.Created($"/api/chat/{bookingId}/messages", result);
            })
            .WithName("SendMessage")
            .WithTags("Chat")
            .RequireAuthorization();

        app.MapPatch("/api/chat/{bookingId:guid}/messages/read", async (IMediator mediator, ClaimsPrincipal user, Guid bookingId) =>
            {
                var recipientId = Guid.Parse(user.FindFirstValue("sub")
                    ?? throw new UnauthorizedAccessException("Invalid token."));
                await mediator.Send(new MarkMessagesReadCommand(bookingId, recipientId));
                return Results.NoContent();
            })
            .WithName("MarkMessagesRead")
            .WithTags("Chat")
            .RequireAuthorization();
    }
}
