using System;

namespace DynamoDb.ConsoleApp.Services.Books
{
    [Serializable]
    public sealed class BookForCreate
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}