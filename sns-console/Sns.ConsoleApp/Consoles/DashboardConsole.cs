using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Sns.ConsoleApp.Consoles
{
    public sealed class DashboardConsole
    {
        private readonly TopicManagerConsole _topicManagerConsole;
        private readonly PublicationManagerConsole _publicationManagerConsole;
        private readonly SubscriptionManagerConsole _subscriptionManagerConsole;
        private readonly ILogger<DashboardConsole> _logger;
        private const ConsoleColor DefaultForegroundColor = ConsoleColor.White;

        public DashboardConsole(
            TopicManagerConsole topicManagerConsole,
            PublicationManagerConsole publicationManagerConsole,
            SubscriptionManagerConsole subscriptionManagerConsole,
            ILogger<DashboardConsole> logger)
        {
            _topicManagerConsole = topicManagerConsole ?? throw new ArgumentNullException(nameof(topicManagerConsole));
            _publicationManagerConsole = publicationManagerConsole ?? throw new ArgumentNullException(nameof(publicationManagerConsole));
            _subscriptionManagerConsole = subscriptionManagerConsole ?? throw new ArgumentNullException(nameof(subscriptionManagerConsole));
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
            Console.WriteLine(@"    ___        ______    ____  _   _ ____    ____                       ");
            Console.WriteLine(@"   / \ \      / / ___|  / ___|| \ | / ___|  |  _ \  ___ _ __ ___   ___  ");
            Console.WriteLine(@"  / _ \ \ /\ / /\___ \  \___ \|  \| \___ \  | | | |/ _ \ '_ ` _ \ / _ \ ");
            Console.WriteLine(@" / ___ \ V  V /  ___) |  ___) | |\  |___) | | |_| |  __/ | | | | | (_) |");
            Console.WriteLine(@"/_/   \_\_/\_/  |____/  |____/|_| \_|____/  |____/ \___|_| |_| |_|\___/ ");
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
                        await _topicManagerConsole.DisplayAsync();
                        break;
                    case "2":
                        await _subscriptionManagerConsole.DisplayAsync();
                        break;
                    case "3":
                        await _publicationManagerConsole.DisplayAsync();
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
            menu.AppendLine("1 - Manage Topics");
            menu.Append("2 - Manage Subscriptions".PadRight(30, ' '));
            menu.AppendLine("3 - Manage Publications");
            return menu.ToString();
        }
    }
}