using System;
using System.Collections.Generic;
using System.Linq;
using DynamoDb.ConsoleApp.Repositories.Models;

namespace DynamoDb.ConsoleApp.Services.Books
{
    internal static class BookMappings
    {
        internal static IReadOnlyList<Book> ToBooks(this IEnumerable<BookEntity> bookEntities)        
        {
            return bookEntities.Select(entity => entity.ToBook()).ToList();
        }

        internal static Book ToBook(this BookEntity bookEntity)        
        {
            return new Book
            {
                Description = bookEntity.Description,
                Id = Guid.Parse(bookEntity.Id),
                Title = bookEntity.Title
            };
        }
    }
}