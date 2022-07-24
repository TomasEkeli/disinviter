using disinviter.Events;
using Dolittle.SDK;
using Dolittle.SDK.Tenancy;

namespace disinviter;

public static class RegisterApplicationStart
{
    const string ApplicationLifetime = "CDBBD270-0DE1-4807-8B48-ACF72EC5800C";

    public static void RegisterApplicationLifetimeEvents(this WebApplication application)
    {
        application
            .Lifetime
            .ApplicationStarted
            .Register(async () =>
                {
                    var client = application.Services.GetRequiredService<IDolittleClient>();
                    await client
                        .EventStore
                        .ForTenant(TenantId.Development)
                        .CommitEvent(
                            new ApplicationStarted(DateTimeOffset.Now),
                            ApplicationLifetime
                        );
                }
            );

        application
            .Lifetime
            .ApplicationStopping
            .Register(async () =>
                {
                    var client = application.Services.GetRequiredService<IDolittleClient>();
                    await client
                        .EventStore
                        .ForTenant(TenantId.Development)
                        .CommitEvent(
                            new ApplicationStopped(DateTimeOffset.Now),
                            ApplicationLifetime
                        );
                }
            );
    }
}