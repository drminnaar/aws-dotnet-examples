using System;
using System.Text;
using System.Threading.Tasks;
using DynamoDb.ConsoleApp.Managers.Tables;
using Newtonsoft.Json;

namespace DynamoDb.ConsoleApp.Consoles
{
    public sealed class TableManagerConsole
    {
        private readonly IDynamoDbTableManager _dynamoDbService;
        private const ConsoleColor DefaultForegroundColor = ConsoleColor.White;
        private const ConsoleColor ForegroundColor = ConsoleColor.Cyan;

        public TableManagerConsole(IDynamoDbTableManager dynamoDbService)
        {
            _dynamoDbService = dynamoDbService ?? throw new ArgumentNullException(nameof(dynamoDbService));
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

                if (selection == "1")
                {
                    var tables = await _dynamoDbService.GatAllTablesAsync();
                    Console.WriteLine(JsonConvert.SerializeObject(tables));
                }
                else if (selection == "2")
                {
                    Console.Write("Enter table name: ");
                    var tableName = Console.ReadLine();
                    var table = await _dynamoDbService.GetTableAsync(tableName);
                    if (table != null)
                        Console.WriteLine(JsonConvert.SerializeObject(table, Formatting.Indented));
                }
                else if (selection == "3")
                {
                    Console.Write("Enter table name: ");
                    var tableName = Console.ReadLine();
                    var exists = await _dynamoDbService.ExistsAsync(tableName);
                    Console.WriteLine($"Exists? {exists}");
                }
                else if (selection == "4")
                {
                    Console.Write("Enter table name: ");
                    var tableName = Console.ReadLine();
                    await _dynamoDbService.CreateTableAsync(tableName);
                    Console.WriteLine($"Created table '{tableName}'");

                }
                else if (selection == "5")
                {
                    Console.Write("Enter table name: ");
                    var tableName = Console.ReadLine();
                    await _dynamoDbService.DeleteTableAsync(tableName);
                    Console.WriteLine($"Deleted table '{tableName}'");
                }
            } while (true);

            Console.ForegroundColor = DefaultForegroundColor;
        }

        private static string GetMenu()
        {
            var menu = new StringBuilder();
            menu.AppendLine();
            menu.Append("0 - Back".PadRight(30, ' '));
            menu.AppendLine("1 - List tables");
            menu.Append("2 - Get table".PadRight(30, ' '));
            menu.AppendLine("3 - Exists table");
            menu.Append("4 - Create table".PadRight(30, ' '));
            menu.AppendLine("5 - Delete table");
            return menu.ToString();
        }
    }
}