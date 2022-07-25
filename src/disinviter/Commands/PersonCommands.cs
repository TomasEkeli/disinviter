using disinviter.Events;
using disinviter.Projections;
using Dolittle.SDK;
using Microsoft.AspNetCore.SignalR;

namespace disinviter.Commands;

public interface IInvitationMessages
{
    Task Invited(string name);
    Task Snubbed(string name);
    Task PartyInvitations(PartyInvitations invitations);
}

public class PersonCommands : Hub<IInvitationMessages>
{
    const string EventSourceId = "PersonCommands";
    readonly IDolittleClient _dolittleClient;

    public PersonCommands(IDolittleClient dolittleClient)
    {
        _dolittleClient = dolittleClient;
    }

    public async Task Invite(string name)
    {
        await _dolittleClient
            .EventStore
            .ForTenant(SingleTenant.TenantId)
            .CommitEvent(
                new PersonInvited(name),
                EventSourceId
            );

        await Clients.All.Invited(
            name
        );
    }

    public async Task Snub(string name)
    {
        await _dolittleClient
            .EventStore
            .ForTenant(SingleTenant.TenantId)
            .CommitEvent(
                new PersonSnubbed(name),
                EventSourceId
            );

        await Clients.All.Snubbed(
            name
        );
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"{Context.ConnectionId} connected");
        var state = await _dolittleClient
            .Projections
            .ForTenant(SingleTenant.TenantId)
            .Get<PartyInvitations>(
                EventSourceId
            );

        await Clients
            .Caller
            .PartyInvitations(
                state
            );

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine(
            $"{Context.ConnectionId} disconnected. Aborted: {Context.ConnectionAborted.IsCancellationRequested}, Exception: {exception} {exception?.Message}"
        );

        await base.OnDisconnectedAsync(exception);
    }
}
