using Sqs.ConsoleApp.Managers.Queues;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sqs.ConsoleApp.Consoles
{
    public sealed class QueueManagerConsole
    {
        private readonly IQueueManager _queueManager;
        private const ConsoleColor DefaultForegroundColor = ConsoleColor.White;
        private const ConsoleColor ForegroundColor = ConsoleColor.Cyan;
        private const ConsoleColor ErrorForegroundColor = ConsoleColor.DarkRed;

        public QueueManagerConsole(IQueueManager queueManager)
        {
            _queueManager = queueManager ?? throw new ArgumentNullException(nameof(queueManager));
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
                        await ExecuteMenuActionAsync(CreateQueueAsync);
                        break;
                    case "2":
                        await ExecuteMenuActionAsync(DeleteQueueAsync);
                        break;
                    case "3":
                        await ExecuteMenuActionAsync(ListQueuesAsync);
                        break;
                    case "4":
                        await ExecuteMenuActionAsync(GetQueueUrlAsync);
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

        private async Task CreateQueueAsync()
        {
            Console.Write("\nEnter queue name: ");
            var queueName = Console.ReadLine() ?? string.Empty;

            var queueUrl = await _queueManager.CreateQueueAsync(queueName);

            Console.WriteLine($"Created queue: {queueUrl}");
        }

        private async Task DeleteQueueAsync()
        {
            Console.Write("\nEnter queue name: ");
            var queueName = Console.ReadLine() ?? string.Empty;

            var queueUrl = await _queueManager.DeleteQueueAsync(queueName);
            Console.WriteLine($"Deleted queue '{queueName}' at '{queueUrl}'.");
        }

        private async Task ListQueuesAsync()
        {
            var queues = await _queueManager.ListAllQueuesAsync();
            Console.WriteLine();
            Console.WriteLine(JsonSerializer.Serialize(queues));
        }

        private async Task GetQueueUrlAsync()
        {
            Console.Write("\nEnter queue name: ");
            var queueName = Console.ReadLine() ?? string.Empty;

            var queueUrl = await _queueManager.GetQueueUrlAsync(queueName);
            Console.WriteLine(JsonSerializer.Serialize(queueUrl));
        }

        private static async Task ExecuteMenuActionAsync(Func<Task> menuAction)
        {
            try
            {
                await menuAction();
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
            menu.AppendLine("1 - Create queue");
            menu.Append("2 - Delete queue".PadRight(30, ' '));
            menu.AppendLine("3 - List all queues");
            menu.Append("4 - Get queue url".PadRight(30, ' '));
            return menu.ToString();
        }
    }
}