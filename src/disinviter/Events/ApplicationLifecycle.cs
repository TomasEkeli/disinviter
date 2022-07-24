using Dolittle.SDK.Events;

namespace disinviter.Events;

[EventType("C23F95B5-2F76-4C64-9A1E-4B1633FB9A83")]
public record ApplicationStarted(DateTimeOffset ApplicationStartedTime);

[EventType("ECEE84FD-8F5B-4C6A-A9D3-9DEE19A7AD99")]
public record ApplicationStopped(DateTimeOffset ApplicationStoppedTime);