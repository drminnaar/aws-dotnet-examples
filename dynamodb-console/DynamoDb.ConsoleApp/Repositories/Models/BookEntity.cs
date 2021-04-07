using Amazon.DynamoDBv2.DataModel;

namespace DynamoDb.ConsoleApp.Repositories.Models
{
    [DynamoDBTable("AwsDotnetExamples.Books")]
    public sealed class BookEntity
    {
        [DynamoDBHashKey]
        public string Id { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string Title { get; set; } = string.Empty;

        [DynamoDBProperty]
        public string Description { get; set; } = string.Empty;
    }
}