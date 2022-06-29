using Petroineos.Aggregator.Service.Aggregators;
using Services;

namespace Petroineos.Aggregator.Service.Tests.Unit.Aggregators;

public class PowerPositionAggregatorTests
{
    [Fact]
    public void Construct_WithNullTrades_ShouldThrowArgumentNullException()
    {
        IEnumerable<PowerTrade>? trades = null;

        Action act = () => _ = new PowerPositionAggregator(trades);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{nameof(trades)}')");
    }

    [Fact]
    public void Construct_WithEmptyTrades_ShouldSetNoDateTime()
    {
        var trades = Array.Empty<PowerTrade>();

        var aggregator = new PowerPositionAggregator(trades);

        aggregator.DateTime.Should().Be(default);
    }

    [Fact]
    public void Construct_WithEmptyTrades_ShouldAddNoVolumes()
    {
        var trades = Array.Empty<PowerTrade>();

        var aggregator = new PowerPositionAggregator(trades);

        aggregator.Should().BeEmpty();
    }

    [Fact]
    public void Construct_WithTrades_ShouldSetDateTimeFromFirstTrade()
    {
        var dateTime = DateTime.Now;
        var trades = new[] { PowerTrade.Create(dateTime, 1), PowerTrade.Create(dateTime.AddDays(1), 1) };

        var aggregator = new PowerPositionAggregator(trades);

        aggregator.DateTime.Should().Be(dateTime);
    }

    [Fact]
    public void Construct_WithASingleTrade_ShouldReturnTheSameVolume()
    {
        var trade = PowerTrade.Create(DateTime.Now, 1);
        trade.Periods.First().Volume = 100;

        var aggregator = new PowerPositionAggregator(new[] { trade });

        aggregator.Should().Equal(100);
    }

    [Fact]
    public void Construct_WithComparableNumberOfPeriods_ShouldReturnVolumes()
    {
        var trade1 = PowerTrade.Create(DateTime.Now, 5);
        trade1.Periods[0].Volume = 40;
        trade1.Periods[1].Volume = 60;
        trade1.Periods[2].Volume = 80;
        trade1.Periods[3].Volume = 100;
        trade1.Periods[4].Volume = 120;

        var trade2 = PowerTrade.Create(DateTime.Now, 5);
        trade2.Periods[0].Volume = 120;
        trade2.Periods[1].Volume = 100;
        trade2.Periods[2].Volume = 80;
        trade2.Periods[3].Volume = 60;
        trade2.Periods[4].Volume = 40;

        var aggregator = new PowerPositionAggregator(new[] { trade1, trade2 });

        aggregator.Should().Equal(160, 160, 160, 160, 160);
    }

    [Fact]
    public void Construct_WithIncomparableNumberOfPeriods_ShouldReturnVolumes()
    {
        var trade1 = PowerTrade.Create(DateTime.Now, 7);
        trade1.Periods[0].Volume = 40;
        trade1.Periods[1].Volume = 60;
        trade1.Periods[2].Volume = 80;
        trade1.Periods[3].Volume = 100;
        trade1.Periods[4].Volume = 120;
        trade1.Periods[5].Volume = 100;
        trade1.Periods[6].Volume = 80;

        var trade2 = PowerTrade.Create(DateTime.Now, 5);
        trade2.Periods[0].Volume = 120;
        trade2.Periods[1].Volume = 100;
        trade2.Periods[2].Volume = 80;
        trade2.Periods[3].Volume = 60;
        trade2.Periods[4].Volume = 40;

        var aggregator = new PowerPositionAggregator(new[] { trade1, trade2 });

        aggregator.Should().Equal(160, 160, 160, 160, 160, 100, 80);
    }
}