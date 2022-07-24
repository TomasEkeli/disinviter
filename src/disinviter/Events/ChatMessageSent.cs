using Dolittle.SDK.Events;

namespace disinviter.Events;

[EventType("B0B579B0-2A01-40AC-8247-4D5A2706DBD1")]
public record ChatMessageSent(string User, string Message);