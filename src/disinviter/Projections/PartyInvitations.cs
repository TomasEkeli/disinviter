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
        Invited.Add(evt.Name);
        if (Snubbed.Contains(evt.Name))
        {
            Snubbed.Remove(evt.Name);
        }
    }

    [KeyFromEventSource]
    public void On(PersonSnubbed evt, ProjectionContext context)
    {
        Snubbed.Add(evt.Name);
        if (Invited.Contains(evt.Name))
        {
            Invited.Remove(evt.Name);
        }
    }
}