using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DynamoDb.ConsoleApp.Repositories;
using Microsoft.Extensions.Logging;

namespace DynamoDb.ConsoleApp.Managers.Books
{
    public sealed class BookManager : IBookManager
    {
        private readonly ILogger<BookManager> _logger;
        private readonly IEntityRepository<Book> _repository;

        public BookManager(IEntityRepository<Book> repository, ILogger<BookManager> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task<Guid> AddBookAsync(Book book)
        {
            if (book is null)
                throw new ArgumentNullException(nameof(book));

            return addBookAsync();

            async Task<Guid> addBookAsync()
            {
                book.Id = Guid.NewGuid();
                book.CreationDate = DateTime.UtcNow;
                return await _repository.SaveEntityAsync(book);
            }
        }

        public Task DeleteBookAsync(Guid bookId)
        {
            return _repository.DeleteEntityAsync(bookId);
        }

        public Task<Book> GetBookAsync(Guid bookId)
        {
            return _repository.GetEntityByIdAsync(bookId);
        }

        public Task<IReadOnlyList<Book>> GetAllBooksAsync()
        {
            return _repository.GetAllEntitiesAsync();
        }

        public Task UpdateBookAsync(Guid bookId, BookUpdate bookUpdate)
        {
            if (bookUpdate is null)
                throw new ArgumentNullException(nameof(bookUpdate));

            return updateBookAsync();

            async Task updateBookAsync()
            {
                var book = await _repository.GetEntityByIdAsync(bookId);

                if (book == null)
                    throw new InvalidOperationException($"A book having id '{book.Id}' could not be found.");

                book.Title = bookUpdate.Title;
                book.Description = bookUpdate.Description;

                await _repository.SaveEntityAsync(book);
            }
        }
    }
}