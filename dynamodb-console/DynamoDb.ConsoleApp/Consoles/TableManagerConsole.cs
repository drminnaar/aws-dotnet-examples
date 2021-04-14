using DynamoDb.ConsoleApp.Repositories;
using DynamoDb.ConsoleApp.Services.Books;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DynamoDb.ConsoleApp.Consoles
{
    public sealed class TableManagerConsole : ConsoleBase
    {
        private readonly ITableRepository _tableRepository;
        private readonly IBooksTableManager _booksTableManager;
        private const ConsoleColor DefaultForegroundColor = ConsoleColor.White;
        private const ConsoleColor ForegroundColor = ConsoleColor.Cyan;

        public TableManagerConsole(ITableRepository tableRepository, IBooksTableManager booksTableManager)
        {
            _tableRepository = tableRepository ?? throw new ArgumentNullException(nameof(tableRepository));
            _booksTableManager = booksTableManager ?? throw new ArgumentNullException(nameof(booksTableManager));
        }

        public async Task DisplayAsync()
        {
            Console.ForegroundColor = ForegroundColor;
            do
            {
                var selection = Prompt(GetMenu());

                if (selection == "0") break;

                switch (selection)
                {
                    case "1":
                        await ExecuteMenuActionAsync(GetAllTableNamesAsync);
                        break;
                    case "2":
                        await ExecuteMenuActionAsync(DescribeTableAsync);
                        break;
                    case "3":
                        await ExecuteMenuActionAsync(DescribeAllTablesAsync);
                        break;
                    case "4":
                        await ExecuteMenuActionAsync(DeleteTableAsync);
                        break;
                    case "5":
                        await ExecuteMenuActionAsync(CreateTableAndWaitAsync);
                        break;
                    case "6":
                        await ExecuteMenuActionAsync(DescribeBooksTableAsync);
                        break;
                    case "7":
                        await ExecuteMenuActionAsync(DeleteBooksTableAsync);
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

        private async Task GetAllTableNamesAsync()
        {
            var tableNames = await _tableRepository.GetTableNameListAsync();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            foreach (var tableName in tableNames)
                Console.WriteLine(tableName);
            Console.ForegroundColor = ForegroundColor;
        }

        private async Task DescribeTableAsync()
        {
            var tableName = Prompt("Enter table name: ");
            var tableDescription = await _tableRepository.DescribeTableAsync(tableName);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(JsonSerializer.Serialize(tableDescription, new JsonSerializerOptions { WriteIndented = true }));
            Console.ForegroundColor = ForegroundColor;
        }

        private async Task DescribeAllTablesAsync()
        {
            var tableNames = (await _tableRepository.GetTableNameListAsync()).ToArray();
            var tableDescriptions = await _tableRepository.DescribeTablesAsync(tableNames);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(JsonSerializer.Serialize(tableDescriptions, new JsonSerializerOptions { WriteIndented = true }));
            Console.ForegroundColor = ForegroundColor;
        }

        private async Task DeleteTableAsync()
        {
            var tableName = Prompt("Enter name of table to delete: ");

            await _tableRepository.DeleteTableAsync(tableName);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine($"Deleted table '{tableName}'.");
            Console.ForegroundColor = ForegroundColor;
        }

        private async Task CreateTableAndWaitAsync()
        {
            await _booksTableManager.CreateBooksTableAsync();
            await _booksTableManager.WaitUntilBooksTableReadyAsync();
            var table = await _booksTableManager.DescribeBooksTableAsync();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(JsonSerializer.Serialize(table, new JsonSerializerOptions { WriteIndented = true }));
            Console.ForegroundColor = ForegroundColor;
        }

        private async Task DescribeBooksTableAsync()
        {
            var table = await _booksTableManager.DescribeBooksTableAsync();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(JsonSerializer.Serialize(table, new JsonSerializerOptions { WriteIndented = true }));
            Console.ForegroundColor = ForegroundColor;
        }

        private async Task DeleteBooksTableAsync()
        {
            await _booksTableManager.DeleteBooksTableAsync();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Books Table Deleted");
            Console.ForegroundColor = ForegroundColor;
        }

        private static string GetMenu()
        {
            var menu = new StringBuilder();
            menu.AppendLine();
            menu.Append("0 - Back".PadRight(30, ' '));
            menu.AppendLine("1 - List all table names");
            menu.Append("2 - Describe single table".PadRight(30, ' '));
            menu.AppendLine("3 - Describe all tables");
            menu.Append("4 - Delete table".PadRight(30, ' '));
            menu.AppendLine("5 - Create 'Books' table");
            menu.Append("6 - Describe 'Books' table".PadRight(30, ' '));
            menu.AppendLine("7 - Delete 'Books' table");
            menu.AppendLine();
            return menu.ToString();
        }
    }
}