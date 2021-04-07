using System;

namespace DynamoDb.ConsoleApp.Services.Books
{
    public sealed class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}