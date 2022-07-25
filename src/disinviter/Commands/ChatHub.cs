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
        var state = await _dolittleClient
            .Projections
            .ForTenant(TenantId.Development)
            .Get<ChatMessagesProjection>(
                EventSourceId
            );

        await Clients
            .Caller
            .SendAsync(
                "ReceiveMessages",
                state.AllMessages
            );

        await base.OnConnectedAsync();
    }
}