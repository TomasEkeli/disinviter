using disinviter.Events;
using disinviter.Projections;
using Dolittle.SDK;
using Dolittle.SDK.Tenancy;
using Microsoft.AspNetCore.SignalR;

namespace disinviter.Commands;

public class ChatHub : Hub
{
    const string EventSourceId = "ChatHub";
    readonly Random _random = new();
    readonly IDolittleClient _dolittleClient;

    public ChatHub(IDolittleClient dolittleClient)
    {
        _dolittleClient = dolittleClient;
    }

    public async Task SendMessage(string user, string message)
    {
        await _dolittleClient
            .EventStore
            .ForTenant(SingleTenant.TenantId)
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

    public async Task Play(string user, string soundName)
    {
        await _dolittleClient
            .EventStore
            .ForTenant(SingleTenant.TenantId)
            .CommitEvent(
                new SoundPlayed(user, soundName),
                EventSourceId
            );

        await Clients
            .All
            .SendAsync(
                "SoundPlayed",
                user,
                soundName
            );
    }

    public async Task Toast(string user)
    {
        await _dolittleClient
            .EventStore
            .ForTenant(SingleTenant.TenantId)
            .CommitEvent(
                new Toast(user),
                EventSourceId
            );

        if (_random.Next(2) == 0)
        {
            await Clients
                .All
                .SendAsync(
                    "SoundPlayed",
                    user,
                    "tore"
                );
        }
        else
        {
            await Clients
                .All
                .SendAsync(
                    "SoundPlayed",
                    user,
                    "mike"
                );
        }
    }

    public async Task Stop(string user)
    {
        await _dolittleClient
            .EventStore
            .ForTenant(SingleTenant.TenantId)
            .CommitEvent(
                new SoundStopped(user),
                EventSourceId
            );

        await Clients
            .All
            .SendAsync(
                "SoundStopped",
                user
            );
    }

    public override async Task OnConnectedAsync()
    {
        var state = await _dolittleClient
            .Projections
            .ForTenant(SingleTenant.TenantId)
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