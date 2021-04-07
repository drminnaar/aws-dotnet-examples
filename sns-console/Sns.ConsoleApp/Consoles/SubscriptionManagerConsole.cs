using Sns.ConsoleApp.Services.Subscriptions;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sns.ConsoleApp.Consoles
{
    public sealed class SubscriptionManagerConsole
    {
        private readonly ISubscriptionService _subscriptionService;
        private const ConsoleColor DefaultForegroundColor = ConsoleColor.White;
        private const ConsoleColor ForegroundColor = ConsoleColor.Cyan;
        private const ConsoleColor ErrorForegroundColor = ConsoleColor.DarkRed;

        public SubscriptionManagerConsole(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService ?? throw new ArgumentNullException(nameof(subscriptionService));
        }

        public async Task DisplayAsync()
        {
            Console.ForegroundColor = ForegroundColor;
            do
            {
                Console.WriteLine(GetMenu());
                var selection = Console.ReadLine();

                if (selection == "0")
                    break;

                switch (selection)
                {
                    case "1":
                        await ListSubscriptionsAsync();
                        break;
                    case "2":
                        await CreateEmailSubscriptionAsync();
                        break;
                    case "3":
                        await CancelSubscriptionAsync();
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid selection. Try again.");
                        Console.ForegroundColor = ForegroundColor;
                        break;
                }
            } while (true);

            Console.ForegroundColor = DefaultForegroundColor;
        }

        private async Task ListSubscriptionsAsync()
        {
            try
            {
                Console.Write("Enter a topic name: ");
                var topicName = Console.ReadLine() ?? string.Empty;

                var subscriptions = await _subscriptionService.ListSubscriptionsByTopicAsync(topicName);
                Console.WriteLine(JsonSerializer.Serialize(subscriptions, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ErrorForegroundColor;
                Console.WriteLine(e);
                Console.ForegroundColor = ForegroundColor;
            }
        }

        private async Task CreateEmailSubscriptionAsync()
        {
            try
            {
                Console.Write("Enter a topic name: ");
                var topicName = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter an email: ");
                var email = Console.ReadLine() ?? string.Empty;

                var subscription = await _subscriptionService.CreateEmailSubscriptionAsync(topicName, email);
                Console.WriteLine(JsonSerializer.Serialize(subscription, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ErrorForegroundColor;
                Console.WriteLine(e);
                Console.ForegroundColor = ForegroundColor;
            }
        }

        private async Task CancelSubscriptionAsync()
        {
            try
            {
                Console.Write("Enter a subscription ARN: ");
                var subscriptionArn = Console.ReadLine() ?? string.Empty;

                var cancellation = await _subscriptionService.CancelSubscriptionAsync(subscriptionArn);
                Console.WriteLine(JsonSerializer.Serialize(cancellation, new JsonSerializerOptions { WriteIndented = true }));
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ErrorForegroundColor;
                Console.WriteLine(e);
                Console.ForegroundColor = ForegroundColor;
            }
        }

        private static string GetMenu()
        {
            var menu = new StringBuilder();
            menu.AppendLine();
            menu.Append("0 - Back".PadRight(30, ' '));
            menu.AppendLine("1 - List subscriptions");
            menu.Append("2 - Create email subscription".PadRight(30, ' '));
            menu.AppendLine("3 - Cancel subscription");
            return menu.ToString();
        }
    }
}