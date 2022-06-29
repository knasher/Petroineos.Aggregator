using Petroineos.Aggregator.Service;
using Petroineos.Aggregator.Service.Config;
using Petroineos.Aggregator.Service.Reporters;
using Services;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<IPowerService, PowerService>();
        services.AddSingleton<IServiceConfiguration, ServiceConfiguration>();
        services.AddSingleton<IPowerPositionReporter, PowerPositionReporter>();

        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();