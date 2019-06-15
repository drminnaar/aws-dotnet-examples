

using System;
using System.Text;
using System.Threading.Tasks;
using DynamoDb.ConsoleApp.Managers.Books;
using Newtonsoft.Json;

namespace DynamoDb.ConsoleApp.Consoles
{
    public sealed class BookManagerConsole
    {
        private readonly IBookManager _bookManager;
        private const ConsoleColor DefaultForegroundColor = ConsoleColor.White;
        private const ConsoleColor ForegroundColor = ConsoleColor.Green;
        private const ConsoleColor ErrorForegroundColor = ConsoleColor.DarkRed;

        public BookManagerConsole(IBookManager bookManager)
        {
            _bookManager = bookManager ?? throw new ArgumentNullException(nameof(bookManager));
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
                        await GetAllBooksAsync();
                        break;
                    case "2":
                        await GetBookAsync();
                        break;
                    case "3":
                        await AddBookAsync();
                        break;
                    case "4":
                        await UpdateBookAsync();
                        break;
                    case "5":
                        await DeleteBookAsync();
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
            try
            {
                var books = await _bookManager.GetAllBooksAsync();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(JsonConvert.SerializeObject(books, Formatting.Indented));
                Console.ForegroundColor = ForegroundColor;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ErrorForegroundColor;
                Console.WriteLine(e);
                Console.ForegroundColor = ForegroundColor;
            }
        }

        private async Task GetBookAsync()
        {
            Console.Write("Enter book id: ");
            var bookIdFromConsole = Console.ReadLine();

            try
            {
                var bookId = Guid.Parse(bookIdFromConsole);
                var book = await _bookManager.GetBookAsync(bookId);
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(JsonConvert.SerializeObject(book, Formatting.Indented));
                Console.ForegroundColor = ForegroundColor;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ErrorForegroundColor;
                Console.WriteLine(e);
                Console.ForegroundColor = ForegroundColor;
            }
        }

        private async Task AddBookAsync()
        {
            var book = new Book();

            Console.WriteLine("Enter book title: ");
            book.Title = Console.ReadLine();

            Console.WriteLine("Enter book description: ");
            book.Description = Console.ReadLine();

            try
            {
                var bookId = await _bookManager.AddBookAsync(book);
                Console.WriteLine($"Added new book successfully. New book id: {bookId}");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ErrorForegroundColor;
                Console.WriteLine(e);
                Console.ForegroundColor = ForegroundColor;
            }
        }

        private async Task UpdateBookAsync()
        {
            Console.Write("Enter book id: ");
            var bookIdFromConsole = Console.ReadLine();

            var bookUpdate = new BookUpdate();

            Console.WriteLine("Enter book title: ");
            bookUpdate.Title = Console.ReadLine();

            Console.WriteLine("Enter book description: ");
            bookUpdate.Description = Console.ReadLine();

            try
            {
                var bookId = Guid.Parse(bookIdFromConsole);
                await _bookManager.UpdateBookAsync(bookId, bookUpdate);
                Console.WriteLine($"Book having id '{bookId}' updated succesfully.");
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ErrorForegroundColor;
                Console.WriteLine(e);
                Console.ForegroundColor = ForegroundColor;
            }
        }

        private async Task DeleteBookAsync()
        {
            Console.Write("Enter book id: ");
            var bookIdFromConsole = Console.ReadLine();

            try
            {
                var bookId = Guid.Parse(bookIdFromConsole);
                await _bookManager.DeleteBookAsync(bookId);
                Console.WriteLine($"Book having id '{bookId}' was deleted successfully.");
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
            menu.AppendLine("1 - List books");
            menu.Append("2 - Get book by id".PadRight(30, ' '));
            menu.AppendLine("3 - Add book");
            menu.Append("4 - Update book".PadRight(30, ' '));
            menu.Append("5 - Delete book");
            return menu.ToString();
        }
    }
}