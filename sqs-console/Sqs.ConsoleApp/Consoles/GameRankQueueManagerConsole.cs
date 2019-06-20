using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sqs.ConsoleApp.Managers.Games;

namespace Sqs.ConsoleApp.Consoles
{
    public sealed class GameRankQueueManagerConsole
    {
        private readonly IGameRankQueueManager _gameRankQueueManager;
        private const ConsoleColor DefaultForegroundColor = ConsoleColor.White;
        private const ConsoleColor ForegroundColor = ConsoleColor.Magenta;
        private const ConsoleColor ErrorForegroundColor = ConsoleColor.DarkRed;

        public GameRankQueueManagerConsole(IGameRankQueueManager gameRankQueueManager)
        {
            _gameRankQueueManager = gameRankQueueManager ?? throw new ArgumentNullException(nameof(gameRankQueueManager));
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
                        await EnqueueGameRankAsync();
                        break;
                    case "2":
                        await DequeueGameRankAsync();
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

        private async Task EnqueueGameRankAsync()
        {
            try
            {
                Console.WriteLine("\nEnqueue Game Rank");

                Console.Write("\nEnter queue name: ");
                var queueName = Console.ReadLine();

                Console.Write("\nEnter use id: ");
                var userId = Console.ReadLine();

                Console.Write("Enter game name: ");
                var gameName = Console.ReadLine();

                Console.Write("Enter game rating: ");
                var gameRating = double.Parse(Console.ReadLine());

                var gameRank = new GameRank
                {
                    GameName = gameName,
                    GameRating = gameRating,
                    UserId = userId
                };

                var messageId = await _gameRankQueueManager.EnqueueGameRankAsync(queueName, gameRank);
                Console.WriteLine($"\nPublished game ranking successfully: {messageId}");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ErrorForegroundColor;
                Console.WriteLine(e);
                Console.ForegroundColor = ForegroundColor;
            }
        }

        private async Task DequeueGameRankAsync()
        {
            try
            {
                Console.WriteLine("\nDequeue Game Rankings");

                Console.Write("\nEnter queue name: ");
                var queueName = Console.ReadLine();

                var response = await _gameRankQueueManager.DequeueGameRanksAsync(queueName);
                Console.WriteLine(JsonConvert.SerializeObject(response));
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
            menu.AppendLine("1 - Enqueue game ranking");
            menu.Append("2 - Dequeue game rankings".PadRight(30, ' '));
            return menu.ToString();
        }
    }
}