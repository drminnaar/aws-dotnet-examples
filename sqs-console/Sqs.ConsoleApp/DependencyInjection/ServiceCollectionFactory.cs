using Amazon.SQS;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sqs.ConsoleApp.Consoles;
using Sqs.ConsoleApp.Managers.Games;
using Sqs.ConsoleApp.Managers.Queues;

namespace Sqs.ConsoleApp.DependencyInjection
{
    internal static class ServiceCollectionFactory
    {
        internal static IServiceCollection CreateServices(IConfiguration configuration)
        {
            var services = new ServiceCollection();

            services.AddLogging(configure => configure.AddConsole());

            // Add AWS services
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<IAmazonSQS>();            

            // Add managers
            services.AddSingleton<IGameRankQueueManager, GameRankQueueManager>();
            services.AddSingleton<IQueueManager, QueueManager>();

            // Add consoles
            services.AddSingleton<DashboardConsole>();
            services.AddSingleton<GameRankQueueManagerConsole>();
            services.AddSingleton<QueueManagerConsole>();

            return services;
        }
    }
}