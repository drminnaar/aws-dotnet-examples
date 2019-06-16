using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sns.ConsoleApp.Consoles;
using Sns.ConsoleApp.DependencyInjection;

namespace Sns.ConsoleApp
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
