using System;

namespace DynamoDb.ConsoleApp.Services.Books
{
    [Serializable]
    public sealed class BookForCreate
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}