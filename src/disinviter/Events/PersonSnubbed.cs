using Dolittle.SDK.Events;

namespace disinviter.Events;

[EventType("65E401CF-1A46-4618-9FFA-046DD23A9223")]
public record PersonSnubbed(string SnubbedName);