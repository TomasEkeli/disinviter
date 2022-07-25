using disinviter.Events;
using disinviter.Projections;
using Dolittle.SDK;
using Dolittle.SDK.Tenancy;
using Microsoft.AspNetCore.SignalR;

namespace disinviter.Commands;

public class PersonCommands : Hub
{
    const string EventSourceId = "PersonCommands";
    readonly IDolittleClient _dolittleClient;

    public PersonCommands(IDolittleClient dolittleClient)
    {
        _dolittleClient = dolittleClient;
    }

    public async Task<bool> Invite(string name)
    {
        try
        {
            await _dolittleClient
                .EventStore
                .ForTenant(TenantId.Development)
                .CommitEvent(
                    new PersonInvited(name),
                    EventSourceId
                );

            Console.WriteLine($"Invited {name}");

            await Clients.All.SendAsync(
                "Invited",
                name
            );

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> Snub(string name)
    {
        try
        {
            await _dolittleClient
                .EventStore
                .ForTenant(TenantId.Development)
                .CommitEvent(
                    new PersonSnubbed(name),
                    EventSourceId
                );

            Console.WriteLine($"Snubbed {name}");

            await Clients.All.SendAsync(
                "Snubbed",
                name
            );

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine($"{Context.ConnectionId} Connected");
        var state = await _dolittleClient
            .Projections
            .ForTenant(TenantId.Development)
            .Get<PartyInvitations>(
                EventSourceId
            );
        await base.OnConnectedAsync();
        await Clients
            .Caller
            .SendAsync(
                "PartyInvitations",
                state
            );
    }
}
