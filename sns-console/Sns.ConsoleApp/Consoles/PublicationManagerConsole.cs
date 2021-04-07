using System;
using System.Text;
using System.Threading.Tasks;
using Sns.ConsoleApp.Managers.Games;

namespace Sns.ConsoleApp.Consoles
{
    public sealed class PublicationManagerConsole
    {
        private readonly IGameRankPublicationManager _publishManager;
        private const ConsoleColor DefaultForegroundColor = ConsoleColor.White;
        private const ConsoleColor ForegroundColor = ConsoleColor.Magenta;
        private const ConsoleColor ErrorForegroundColor = ConsoleColor.DarkRed;

        public PublicationManagerConsole(IGameRankPublicationManager publishManager)
        {
            _publishManager = publishManager ?? throw new ArgumentNullException(nameof(publishManager));
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
                        await PublishGameRankAsync();
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

        private async Task PublishGameRankAsync()
        {
            try
            {
                Console.WriteLine("\nCreate Game Rank Publication");

                Console.Write("\nEnter use id: ");
                var userId = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter game name: ");
                var gameName = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter game rating: ");
                var gameRating = double.Parse(Console.ReadLine() ?? string.Empty);

                var gameRank = new GameRank
                {
                    GameName = gameName,
                    GameRating = gameRating,
                    UserId = userId
                };

                var response = await _publishManager.PublishGameRankingAsync(gameRank);
                Console.WriteLine($"\nPublished game ranking successfully: {response.MessageId}");
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
            menu.AppendLine("1 - Publish new game ranking");
            return menu.ToString();
        }
    }
}