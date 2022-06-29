using Petroineos.Aggregator.Service.Reporters.Reports;

namespace Petroineos.Aggregator.Service.Tests.Unit.Reporters.Reports;

public class PowerPositionReportTests
{
    [Fact]
    public void Construct_WithNullVolumes_ShouldThrowArgumentNullException()
    {
        IEnumerable<double>? volumes = null;

        Action act = () => _ = new PowerPositionReport(volumes);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage($"Value cannot be null. (Parameter '{nameof(volumes)}')");
    }

    [Fact]
    public void Construct_WithVolumes_ShouldAddRows()
    {
        var volumes = new double[] { 100, 80, 120 };
        var report = new PowerPositionReport(volumes);

        report.Should().HaveCount(volumes.Length);
    }

    [Fact]
    public void Construct_WithVolumes_ShouldAddVolumesToRows()
    {
        var volumes = new double[] { 100, 80, 120 };
        var report = new PowerPositionReport(volumes);

        report.Select(r => r.Volume).Should().Equal(volumes);
    }

    [Fact]
    public void Construct_WithVolumes_ShouldAddLocalTimesToRows()
    {
        var volumes = new double[] { 100, 80, 120 };
        var report = new PowerPositionReport(volumes);

        report.Select(r => r.LocalTime).Should().Equal("23:00", "00:00", "01:00");
    }

    [Fact]
    public void Construct_WithManyVolumes_ShouldLoopLocalTimesInRows()
    {
        var volumes = new double[]
        {
            100, 80, 120, 60, 40, 100, 120, 140, 120, 60, 80, 100,
            40, 60, 140, 100, 100, 140, 120, 80, 40, 100, 100, 60,
            120
        };
        var report = new PowerPositionReport(volumes);

        report.Select(r => r.LocalTime).Last().Should().Be("23:00");
    }
}