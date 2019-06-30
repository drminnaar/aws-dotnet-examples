using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamoDb.ConsoleApp.Services.Books
{
    public interface IBooksManager
    {
        Task AddBookAsync(BookForCreate bookForCreate);
        Task DeleteBookAsync(Guid bookId);
        Task<Book> GetBookAsync(Guid bookId);
        Task<IReadOnlyList<Book>> GetAllBooksAsync();
        Task UpdateBookAsync(Guid bookId, BookForUpdate bookForUpdate);
    }
}