using disinviter.Events;
using Dolittle.SDK.Projections;

namespace disinviter.Projections;

[Projection("351FC96D-2476-4CFF-BD83-561F31822275")]
public class ChatMessagesProjection
{
    public List<ChatMessage> AllMessages = new();

    public List<ChatMessage> Messages
    {
        get
        {
            return AllMessages
                .OrderByDescending(msg => msg.Time)
                .Take(10)
                .ToList();
        }
        set { AllMessages = value.ToList(); }
    }

    [KeyFromEventSource]
    public void On(ChatMessageSent evt, ProjectionContext context)
    {
        AllMessages.Add(new(evt.User, evt.Message, context.EventContext.Occurred));
    }
}

public record ChatMessage(string User, string Message, DateTimeOffset Time);
