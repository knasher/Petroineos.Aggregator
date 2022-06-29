using System.Globalization;
using CsvHelper;
using Petroineos.Aggregator.Service.Aggregators;
using Petroineos.Aggregator.Service.Config;
using Petroineos.Aggregator.Service.Reporters.Reports;
using Petroineos.Aggregator.Service.Utils;

namespace Petroineos.Aggregator.Service.Reporters;

public class PowerPositionReporter : IPowerPositionReporter
{
    private const string FilenameFormat = "PowerPosition_{0}.csv";

    private readonly IServiceConfiguration _configuration;

    public PowerPositionReporter(IServiceConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    public async Task<string> SaveAsync(PowerPositionAggregator? aggregator, CancellationToken stoppingToken = default)
    {
        if (aggregator == null) throw new ArgumentNullException(nameof(aggregator));

        var filename = FilenameFormat.GenerateFilename(aggregator.DateTime).AddPath(_configuration.ReportPath);
        
        await using var writer = new StreamWriter(filename);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(new PowerPositionReport(aggregator), stoppingToken);

        return filename;
    }
}