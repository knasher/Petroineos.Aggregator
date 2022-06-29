namespace Petroineos.Aggregator.Service.Config;

public interface IServiceConfiguration
{
    string? ReportPath { get; }

    TimeSpan? ServiceDelay { get; }
}