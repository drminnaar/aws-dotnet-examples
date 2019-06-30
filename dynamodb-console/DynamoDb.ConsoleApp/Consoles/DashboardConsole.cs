using System;
using System.Text;
using System.Threading.Tasks;
using DynamoDb.ConsoleApp.Repositories;
using Microsoft.Extensions.Logging;

namespace DynamoDb.ConsoleApp.Consoles
{
    public sealed class DashboardConsole
    {
        private readonly TableManagerConsole _tableManagerConsole;
        private readonly BookManagerConsole _bookManagerConsole;
        private readonly ILogger<DashboardConsole> _logger;
        private const ConsoleColor DefaultForegroundColor = ConsoleColor.White;

        public DashboardConsole(
            TableManagerConsole tableManagerConsole,
            BookManagerConsole bookManagerConsole,
            ILogger<DashboardConsole> logger)
        {
            _tableManagerConsole = tableManagerConsole ?? throw new ArgumentNullException(nameof(tableManagerConsole));
            _bookManagerConsole = bookManagerConsole ?? throw new ArgumentNullException(nameof(bookManagerConsole));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task DisplayAsync()
        {
            DisplayTitle();
            await DisplayMenu();
        }

        private static void DisplayTitle()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(@" ____                                    ____  _       ____                       ");
            Console.WriteLine(@"|  _ \ _   _ _ __   __ _ _ __ ___   ___ |  _ \| |__   |  _ \  ___ _ __ ___   ___  ");
            Console.WriteLine(@"| | | | | | | '_ \ / _` | '_ ` _ \ / _ \| | | | '_ \  | | | |/ _ \ '_ ` _ \ / _ \ ");
            Console.WriteLine(@"| |_| | |_| | | | | (_| | | | | | | (_) | |_| | |_) | | |_| |  __/ | | | | | (_) |");
            Console.WriteLine(@"|____/ \__, |_| |_|\__,_|_| |_| |_|\___/|____/|_.__/  |____/ \___|_| |_| |_|\___/ ");
            Console.WriteLine(@"       |___/                                                                      ");
            Console.ForegroundColor = DefaultForegroundColor;
        }

        private async Task DisplayMenu()
        {
            do
            {
                Console.WriteLine(GetMenu());
                var selection = Console.ReadLine();

                if (selection == "0")
                    break;

                switch (selection)
                {
                    case "1":
                        await _tableManagerConsole.DisplayAsync();
                        break;
                    case "2":
                        await _bookManagerConsole.DisplayAsync();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid selection. Please try again.");
                        Console.ForegroundColor = DefaultForegroundColor;
                        break;
                }
            } while (true);
        }

        private static string GetMenu()
        {
            var menu = new StringBuilder();
            menu.AppendLine();
            menu.AppendLine("0 - Quit");
            menu.AppendLine("1 - Manage Tables");
            menu.AppendLine("2 - Manage Book Data");
            return menu.ToString();
        }         
    }
}