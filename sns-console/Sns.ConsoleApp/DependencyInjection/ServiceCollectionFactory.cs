using Amazon.SimpleNotificationService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sns.ConsoleApp.Consoles;
using Sns.ConsoleApp.Managers.Games;
using Sns.ConsoleApp.Services.Publications;
using Sns.ConsoleApp.Services.Subscriptions;
using Sns.ConsoleApp.Services.Topics;

namespace Sns.ConsoleApp.DependencyInjection
{
    internal static class ServiceCollectionFactory
    {
        internal static IServiceCollection CreateServices(IConfiguration configuration)
        {
            var services = new ServiceCollection();

            services.AddLogging(configure => configure.AddConsole());

            // Add AWS services
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<IAmazonSimpleNotificationService>();

            // Add domain services
            services.AddSingleton<ITopicService, SnsTopicService>();
            services.AddSingleton<ISubscriptionService, SubscriptionService>();
            services.AddSingleton<IPublicationService<GameRank>, PublicationService<GameRank>>();

            // Add managers
            services.AddSingleton<IGameRankPublicationManager, GameRankPublicationManager>();

            // Add consoles
            services.AddSingleton<DashboardConsole>();
            services.AddSingleton<TopicManagerConsole>();
            services.AddSingleton<PublicationManagerConsole>();
            services.AddSingleton<SubscriptionManagerConsole>();

            return services;
        }
    }
}