using Dolittle.SDK.Events;

namespace disinviter.Events;

[EventType("10530D07-0E7B-4973-8CEB-146B766D8447")]
public record PersonInvited(string InvitedName);