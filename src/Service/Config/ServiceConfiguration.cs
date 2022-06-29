namespace Petroineos.Aggregator.Service.Config;

public class ServiceConfiguration : IServiceConfiguration
{
    private const string ReportPathVar = "Report:Path";
    private const string ServiceDelayVar = "Service:Delay";

    public string? ReportPath => Environment.GetEnvironmentVariable(ReportPathVar);

    public TimeSpan? ServiceDelay
    {
        get
        {
            var delay = Environment.GetEnvironmentVariable(ServiceDelayVar);

            return delay != null ? TimeSpan.Parse(delay) : null;
        }
    }
}