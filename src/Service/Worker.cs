using Petroineos.Aggregator.Service.Aggregators;
using Petroineos.Aggregator.Service.Config;
using Petroineos.Aggregator.Service.Reporters;
using Services;

namespace Petroineos.Aggregator.Service;

public class Worker : BackgroundService
{
    private static readonly TimeSpan DefaultDelay = TimeSpan.FromHours(1);

    private readonly IServiceConfiguration _configuration;
    private readonly ILogger<Worker> _logger;
    private readonly IPowerService _powerService;
    private readonly IPowerPositionReporter _reporter;

    public Worker(IServiceConfiguration configuration, ILogger<Worker> logger, IPowerService powerService,
        IPowerPositionReporter reporter)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _powerService = powerService ?? throw new ArgumentNullException(nameof(powerService));
        _reporter = reporter ?? throw new ArgumentNullException(nameof(reporter));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker started at: {time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            var trades = (await _powerService.GetTradesAsync(DateTime.Now)).ToList();

            _logger.LogInformation("Worker obtained positions: {positions}", trades.Count);

            var filename = await _reporter.SaveAsync(new PowerPositionAggregator(trades), stoppingToken);

            _logger.LogInformation("Worker wrote file: {filename}", filename);

            await Task.Delay(_configuration.ServiceDelay ?? DefaultDelay, stoppingToken);
        }
    }
}