using Dolittle.SDK.Tenancy;

namespace disinviter;

public static class SingleTenant
{
    public static TenantId? TenantId { get; set; }
}
