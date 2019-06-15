using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamoDb.ConsoleApp.Managers.Books
{
    public interface IBookManager
    {
         Task<Guid> AddBookAsync(Book book);
         Task DeleteBookAsync(Guid bookId);
         Task<Book> GetBookAsync(Guid bookId);
         Task<IReadOnlyList<Book>> GetAllBooksAsync();
         Task UpdateBookAsync(Guid bookId, BookUpdate bookUpdate);
    }
}