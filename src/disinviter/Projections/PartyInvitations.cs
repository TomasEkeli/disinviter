using disinviter.Events;
using Dolittle.SDK.Projections;

namespace disinviter.Projections;

[Projection("5B3BA213-E4F8-4ACD-BE72-FB6E6C258CC7")]
public class PartyInvitations
{
    public List<string> Invited { get; set; } = new List<string>();
    public List<string> Snubbed { get; set; } = new List<string>();

    [KeyFromEventSource]
    public void On(PersonInvited evt, ProjectionContext context)
    {
        Invited.Add(evt.InvitedName);
        if (Snubbed.Contains(evt.InvitedName))
        {
            Snubbed.Remove(evt.InvitedName);
        }
    }

    [KeyFromEventSource]
    public void On(PersonSnubbed evt, ProjectionContext context)
    {
        Snubbed.Add(evt.SnubbedName);
        if (Invited.Contains(evt.SnubbedName))
        {
            Invited.Remove(evt.SnubbedName);
        }
    }
}