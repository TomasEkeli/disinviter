using System.Diagnostics.Tracing;
using disinviter.Events;
using Dolittle.SDK.Projections;

namespace disinviter.Projections;

[Projection("351FC96D-2476-4CFF-BD83-561F31822275")]
public class ChatMessagesProjection
{
    public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

    [KeyFromEventSource]
    public void On(ChatMessageSent evt, ProjectionContext context)
    {
        Messages.Add(new(evt.User, evt.Message, context.EventContext.Occurred));
    }
}

public record ChatMessage(string User, string Message, DateTimeOffset Time);