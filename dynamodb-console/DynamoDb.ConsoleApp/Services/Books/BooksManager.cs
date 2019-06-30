using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DynamoDb.ConsoleApp.Repositories;
using DynamoDb.ConsoleApp.Repositories.Models;

namespace DynamoDb.ConsoleApp.Services.Books
{
    public sealed class BooksManager : IBooksManager
    {
        private readonly IEntityRepository<BookEntity> _repository;

        public BooksManager(IEntityRepository<BookEntity> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Task AddBookAsync(BookForCreate bookForCreate)
        {
            if (bookForCreate is null)
                throw new ArgumentNullException(nameof(bookForCreate));

            return addBookAsync();

            async Task addBookAsync()
            {
                var bookEntity = bookForCreate.ToBookEntity();
                bookEntity.Id = Guid.NewGuid().ToString();

                await _repository.SaveAsync(bookEntity);
            }
        }

        public Task DeleteBookAsync(Guid bookId)
        {
            return _repository.DeleteAsync(bookId.ToString());
        }

        public async Task<Book> GetBookAsync(Guid bookId)
        {
            var bookFromRepo = await _repository.GetAsync(bookId.ToString());
            return bookFromRepo.ToBook();
        }

        public async Task<IReadOnlyList<Book>> GetAllBooksAsync()
        {
            var booksFromRepo = await _repository.GetAllAsync();
            return booksFromRepo.ToBooks();
        }

        public Task UpdateBookAsync(Guid bookId, BookForUpdate bookForUpdate)
        {
            if (bookForUpdate is null)
                throw new ArgumentNullException(nameof(bookForUpdate));

            return updateBookAsync();

            async Task updateBookAsync()
            {
                var book = await _repository.GetAsync(bookId.ToString());

                if (book == null)
                    throw new InvalidOperationException($"A book having id '{book.Id}' could not be found.");

                book.Title = bookForUpdate.Title;
                book.Description = bookForUpdate.Description;

                await _repository.SaveAsync(book);
            }
        }   
    }
}