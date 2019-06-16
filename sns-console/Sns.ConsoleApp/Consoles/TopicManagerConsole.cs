using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sns.ConsoleApp.Services.Topics;

namespace Sns.ConsoleApp.Consoles
{
    public sealed class TopicManagerConsole
    {
        private readonly ITopicService _topicService;
        private const ConsoleColor DefaultForegroundColor = ConsoleColor.White;
        private const ConsoleColor ForegroundColor = ConsoleColor.Green;
        private const ConsoleColor ErrorForegroundColor = ConsoleColor.DarkRed;

        public TopicManagerConsole(ITopicService topicService)
        {
            _topicService = topicService ?? throw new ArgumentNullException(nameof(topicService));
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
                        await CreateTopicAsync();
                        break;
                    case "2":
                        await GetTopicAsync();
                        break;
                    case "3":
                        await GetAllTopicsAsync();
                        break;
                    case "4":
                        await DeleteTopicAsync();
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

        private async Task CreateTopicAsync()
        {
            try
            {
                Console.Write("Enter a topic name: ");
                var topicName = Console.ReadLine();

                Console.Write("Enter attribute name: ");
                var attributeName = Console.ReadLine();

                Console.Write("Enter attribute value: ");
                var attributeValue = Console.ReadLine();

                await _topicService.CreateTopicAsync(
                    topicName,
                    new Dictionary<string, string>()
                    {
                        { attributeName, attributeValue }
                    });
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ErrorForegroundColor;
                Console.WriteLine(e);
                Console.ForegroundColor = ForegroundColor;
            }
        }

        private async Task GetTopicAsync()
        {
            try
            {
                Console.Write("Enter a topic name: ");
                var topicName = Console.ReadLine();

                var topic = await _topicService.GetTopicAsync(topicName);

                if (topic != null)
                {
                    Console.WriteLine(JsonConvert.SerializeObject(topic, Formatting.Indented));
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ErrorForegroundColor;
                Console.WriteLine(e);
                Console.ForegroundColor = ForegroundColor;
            }
        }

        private async Task GetAllTopicsAsync()
        {
            try
            {
                var topics = await _topicService.GetAllTopicsAsync();
                Console.WriteLine(JsonConvert.SerializeObject(topics, Formatting.Indented));
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ErrorForegroundColor;
                Console.WriteLine(e);
                Console.ForegroundColor = ForegroundColor;
            }
        }

        private async Task DeleteTopicAsync()
        {
            try
            {
                Console.Write("Enter topic name: ");
                var topicName = Console.ReadLine();

                await _topicService.DeleteTopicAsync(topicName);
                Console.WriteLine($"Deleted topic '{topicName}' successfully.");
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
            menu.AppendLine("1 - Create topic");
            menu.Append("2 - Get topic".PadRight(30, ' '));
            menu.AppendLine("3 - Get all topics");
            menu.Append("4 - Delete topic".PadRight(30, ' '));
            return menu.ToString();
        }
    }
}