using Amazon.DynamoDBv2;
using DynamoDb.ConsoleApp.Consoles;
using DynamoDb.ConsoleApp.Repositories;
using DynamoDb.ConsoleApp.Repositories.Models;
using DynamoDb.ConsoleApp.Services.Books;
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
            services.AddSingleton<IEntityRepository<BookEntity>, EntityRepository<BookEntity>>();
            services.AddSingleton<ITableRepository, TableRepository>();

            // Add book related services
            services.AddSingleton<IBooksManager, BooksManager>();
            services.AddSingleton<IBooksTableManager, BooksTableManager>();

            // Add consoles
            services.AddSingleton<DashboardConsole>();
            services.AddSingleton<TableManagerConsole>();
            services.AddSingleton<BookManagerConsole>();

            return services;
        }
    }
}