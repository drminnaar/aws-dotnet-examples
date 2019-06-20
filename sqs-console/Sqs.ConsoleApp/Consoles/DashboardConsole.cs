using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Sqs.ConsoleApp.Consoles
{
    public sealed class DashboardConsole
    {
        private readonly QueueManagerConsole _queueManagerConsole;
        private readonly GameRankQueueManagerConsole _gameRankQueueManagerConsole;
        private readonly ILogger<DashboardConsole> _logger;
        private const ConsoleColor DefaultForegroundColor = ConsoleColor.White;

        public DashboardConsole(
            QueueManagerConsole queueManagerConsole,
            GameRankQueueManagerConsole gameRankQueueManagerConsole,
            ILogger<DashboardConsole> logger)
        {
            _queueManagerConsole = queueManagerConsole ?? throw new ArgumentNullException(nameof(queueManagerConsole));
            _gameRankQueueManagerConsole = gameRankQueueManagerConsole ?? throw new ArgumentNullException(nameof(gameRankQueueManagerConsole));
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
            Console.WriteLine(@"    ___        ______    ____   ___  ____    ____                       ");
            Console.WriteLine(@"   / \ \      / / ___|  / ___| / _ \/ ___|  |  _ \  ___ _ __ ___   ___  ");
            Console.WriteLine(@"  / _ \ \ /\ / /\___ \  \___ \| | | \___ \  | | | |/ _ \ '_ ` _ \ / _ \ ");
            Console.WriteLine(@" / ___ \ V  V /  ___) |  ___) | |_| |___) | | |_| |  __/ | | | | | (_) |");
            Console.WriteLine(@"/_/   \_\_/\_/  |____/  |____/ \__\_\____/  |____/ \___|_| |_| |_|\___/ ");
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
                        await _queueManagerConsole.DisplayAsync();
                        break;
                    case "2":
                        await _gameRankQueueManagerConsole.DisplayAsync();
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
            menu.Append("0 - Quit".PadRight(30, ' '));
            menu.AppendLine("1 - Manage Queues");
            menu.Append("2 - Manage Game Rank Queue".PadRight(30, ' '));
            return menu.ToString();
        }
    }
}