namespace DynamoDb.ConsoleApp.Services.Books
{
    public sealed class BookForUpdate
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}