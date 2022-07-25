using Dolittle.SDK.Events;

namespace disinviter.Events;

[EventType("B0B579B0-2A01-40AC-8247-4D5A2706DBD1")]
public record ChatMessageSent(string User, string Message);

[EventType("4033D2B2-8039-4EE1-A4E5-79AE9B115388")]
public record SoundPlayed(string User, string SoundName);

[EventType("BD99A245-2767-4D9E-A756-3D04C504B6C0")]
public record SoundStopped(string User);

[EventType("178A8F0F-EFC0-4CB7-830B-3EAB7D8D6B1C")]
public record Toast(string User);