using System.Collections;
using CsvHelper.Configuration.Attributes;

namespace Petroineos.Aggregator.Service.Reporters.Reports;

public class PowerPositionReport : IEnumerable<PowerPositionReport.Row>
{
    private static readonly TimeOnly Time = new(23, 0);

    private readonly IEnumerable<Row> _rows;

    public PowerPositionReport(IEnumerable<double>? volumes)
    {
        if (volumes == null) throw new ArgumentNullException(nameof(volumes));

        _rows = volumes.Select((v, p) => new Row(p, v));
    }

    public IEnumerator<Row> GetEnumerator()
    {
        return _rows.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public class Row
    {
        public Row(int period, double volume)
        {
            LocalTime = Time.AddHours(period).ToString("HH:mm");
            Volume = volume;
        }

        [Name("Local Time")] public string LocalTime { get; }

        public double Volume { get; }
    }
}