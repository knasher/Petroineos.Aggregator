using Petroineos.Aggregator.Service.Aggregators;

namespace Petroineos.Aggregator.Service.Reporters;

public interface IPowerPositionReporter
{
    Task<string> SaveAsync(PowerPositionAggregator? aggregator, CancellationToken stoppingToken = default);
}