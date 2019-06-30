using DynamoDb.ConsoleApp.Repositories.Models;

namespace DynamoDb.ConsoleApp.Services.Books
{
    internal static class BookEntityMappings
    {
        internal static BookEntity ToBookEntity(this BookForCreate bookFroCreate)
        {
            return new BookEntity
            {
                Description = bookFroCreate.Description,
                Title = bookFroCreate.Title
            };
        }
    }
}