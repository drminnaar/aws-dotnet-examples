using System;
using System.IO;
using System.Threading.Tasks;
using DynamoDb.ConsoleApp.Consoles;
using DynamoDb.ConsoleApp.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DynamoDb.ConsoleApp
{
    internal sealed class Program
    {
        private Program()
        {
        }

        internal async static Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .Build();

            var dashboardConsole = ServiceCollectionFactory
                .CreateServices(configuration)
                .BuildServiceProvider()
                .GetRequiredService<DashboardConsole>();

            await dashboardConsole.DisplayAsync();

            Console.WriteLine("Done!");
        }

    }
}
