using Moq;
using Petroineos.Aggregator.Service.Aggregators;
using Petroineos.Aggregator.Service.Config;
using Petroineos.Aggregator.Service.Reporters;
using Services;
#pragma warning disable CS8604

namespace Petroineos.Aggregator.Service.Tests.Unit.Reporters;

public class PowerPositionReporterTests
{
    [Fact]
    public void Construct_WithNullConfiguration_ShouldThrowArgumentNullException()
    {
        IServiceConfiguration? configuration = null;

        Action act = () => _ = new PowerPositionReporter(configuration);
        
        act.Should().Throw<ArgumentNullException>().WithMessage($"Value cannot be null. (Parameter '{nameof(configuration)}')");
    }

    [Fact]
    public void SaveAsync_WithNullAggregator_ShouldThrowArgumentNullException()
    {
        var sut = new PowerPositionReporter(Mock.Of<IServiceConfiguration>());

        PowerPositionAggregator? aggregator = null;
        Func<Task> func = async () => _ = await sut.SaveAsync(aggregator);
        
        func.Should().ThrowAsync<ArgumentNullException>().WithMessage($"Value cannot be null. (Parameter '{nameof(aggregator)}')");
    }
    
    [Fact]
    public async Task SaveAsync_WithAggregator_ShouldSaveFile()
    {
        var configurationMock = new Mock<IServiceConfiguration>();
        configurationMock.SetupGet(m => m.ReportPath).Returns(Path.GetTempPath());
        
        var sut = new PowerPositionReporter(configurationMock.Object);

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
        
        var filename = await sut.SaveAsync(aggregator);
        
        File.Exists(filename).Should().Be(true);
        
        File.Delete(filename);
    }
}