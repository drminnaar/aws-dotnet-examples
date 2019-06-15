using Amazon.DynamoDBv2;
using DynamoDb.ConsoleApp.Consoles;
using DynamoDb.ConsoleApp.Managers.Books;
using DynamoDb.ConsoleApp.Managers.Tables;
using DynamoDb.ConsoleApp.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DynamoDb.ConsoleApp.DependencyInjection
{
    internal static class ServiceCollectionFactory
    {
        internal static IServiceCollection CreateServices(IConfiguration configuration)
        {
            var services = new ServiceCollection();

            services.AddLogging(configure => configure.AddConsole());

            // Add AWS services
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<IAmazonDynamoDB>();

            // Add repositories
            services.AddSingleton<IEntityRepository<Book>, EntityRepository<Book>>();
            services.AddSingleton<ITableRepository, TableRepository>();

            // Add managers
            services.AddSingleton<IDynamoDbTableManager, DynamoDbTableManager>();
            services.AddSingleton<IBookManager, BookManager>();

            // Add consoles
            services.AddSingleton<DashboardConsole>();
            services.AddSingleton<TableManagerConsole>();
            services.AddSingleton<BookManagerConsole>();

            return services;
        }
    }
}