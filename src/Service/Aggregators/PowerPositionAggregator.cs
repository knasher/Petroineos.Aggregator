using System.Collections;
using Services;

namespace Petroineos.Aggregator.Service.Aggregators;

public class PowerPositionAggregator : IEnumerable<double>
{
    private readonly List<PowerPeriod> _aggregates;

    public PowerPositionAggregator(IEnumerable<PowerTrade>? trades)
    {
        if (trades == null) throw new ArgumentNullException(nameof(trades));

        _aggregates = new List<PowerPeriod>();

        foreach (var trade in trades)
        {
            if (DateTime == default) DateTime = trade.Date;

            foreach (var period in trade.Periods)
            {
                var aggregate = _aggregates.SingleOrDefault(a => a.Period == period.Period);

                if (aggregate == null)
                {
                    aggregate = new PowerPeriod
                    {
                        Period = period.Period,
                        Volume = 0
                    };
                    _aggregates.Add(aggregate);
                }

                aggregate.Volume += period.Volume;
            }
        }
    }

    public DateTime DateTime { get; }

    public IEnumerator<double> GetEnumerator()
    {
        return _aggregates
            .OrderBy(a => a.Period)
            .Select(a => a.Volume)
            .AsEnumerable()
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}