using System;
using System.Text;
using System.Threading.Tasks;
using DynamoDb.ConsoleApp.Services.Books;
using Newtonsoft.Json;

namespace DynamoDb.ConsoleApp.Consoles
{
    public sealed class BookManagerConsole : ConsoleBase
    {
        private readonly IBooksManager _booksManager;
        private const ConsoleColor DefaultForegroundColor = ConsoleColor.White;
        private const ConsoleColor ForegroundColor = ConsoleColor.Magenta;

        public BookManagerConsole(IBooksManager booksManager)
        {
            _booksManager = booksManager ?? throw new ArgumentNullException(nameof(booksManager));
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
                        await ExecuteMenuActionAsync(GetAllBooksAsync);
                        break;
                    case "2":
                        await ExecuteMenuActionAsync(GetBookAsync);
                        break;
                    case "3":
                        await ExecuteMenuActionAsync(AddBookAsync);
                        break;
                    case "4":
                        await ExecuteMenuActionAsync(UpdateBookAsync);
                        break;
                    case "5":
                        await ExecuteMenuActionAsync(DeleteBookAsync);
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

        private async Task GetAllBooksAsync()
        {
            var books = await _booksManager.GetAllBooksAsync();

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine();
            Console.WriteLine(JsonConvert.SerializeObject(books, Formatting.Indented));
            Console.ForegroundColor = ForegroundColor;
        }

        private async Task GetBookAsync()
        {
            var bookId = Guid.Parse(Prompt("Enter book id: "));
            var book = await _booksManager.GetBookAsync(bookId);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine();
            Console.WriteLine(JsonConvert.SerializeObject(book, Formatting.Indented));
            Console.ForegroundColor = ForegroundColor;
        }        

        private async Task AddBookAsync()
        {
            var bookForCreate = new BookForCreate();
            bookForCreate.Title = Prompt("Enter book title: ");
            bookForCreate.Description = Prompt("Enter book description: ");

            await _booksManager.AddBookAsync(bookForCreate);

            Console.WriteLine();
            Console.WriteLine($"Added new book '{bookForCreate.Title}' successfully");
        }

        private async Task UpdateBookAsync()
        {            
            var bookId = Guid.Parse(Prompt("Enter book id: "));

            var bookForUpdate = new BookForUpdate();
            bookForUpdate.Title = Prompt("Enter book title: ");
            bookForUpdate.Description = Prompt("Enter book description: ");            

            await _booksManager.UpdateBookAsync(bookId, bookForUpdate);

            Console.WriteLine();
            Console.WriteLine($"Book having id '{bookId}' updated succesfully.");
        }

        private async Task DeleteBookAsync()
        {
            var bookId = Guid.Parse(Prompt("Enter book id: "));

            await _booksManager.DeleteBookAsync(bookId);

            Console.WriteLine();
            Console.WriteLine($"Book having id '{bookId}' was deleted successfully.");
        }

        private static string GetMenu()
        {
            var menu = new StringBuilder();
            menu.AppendLine();
            menu.Append("0 - Back".PadRight(30, ' '));
            menu.AppendLine("1 - List books");
            menu.Append("2 - Get book by id".PadRight(30, ' '));
            menu.AppendLine("3 - Add book");
            menu.Append("4 - Update book".PadRight(30, ' '));
            menu.AppendLine("5 - Delete book");
            menu.AppendLine();
            return menu.ToString();
        }
    }
}