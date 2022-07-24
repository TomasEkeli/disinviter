using disinviter.Events;
using disinviter.Projections;
using Dolittle.SDK;
using Dolittle.SDK.Tenancy;
using Microsoft.AspNetCore.SignalR;

namespace disinviter.Commands;

public class ChatHub : Hub
{
    const string EventSourceId = "ChatHub";
    readonly IDolittleClient _dolittleClient;

    public ChatHub(IDolittleClient dolittleClient)
    {
        _dolittleClient = dolittleClient;
    }

    public async Task SendMessage(string user, string message)
    {
        await _dolittleClient
            .EventStore
            .ForTenant(TenantId.Development)
            .CommitEvent(
                new ChatMessageSent(user, message),
                EventSourceId
            );

        Console.WriteLine($"{Context.ConnectionId} {user} said: {message}");

        await Clients
            .All
            .SendAsync(
                "ReceiveMessage",
                DateTimeOffset.UtcNow,
                user,
                message
            );
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"Connected {Context.ConnectionId}");

        var state = await _dolittleClient
            .Projections
            .ForTenant(TenantId.Development)
            .Get<ChatMessagesProjection>(
                EventSourceId
            );

        foreach(var message in state.Messages)
        {
            await Clients
                .Caller
                .SendAsync(
                    "ReceiveMessage",
                    message.Time,
                    message.User,
                    message.Message
                );
        }

        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        Console.WriteLine($"Disconnected {Context.ConnectionId}");
        return base.OnDisconnectedAsync(exception);
    }
}