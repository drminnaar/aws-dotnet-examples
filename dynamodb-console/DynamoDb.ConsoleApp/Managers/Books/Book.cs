using System;
using Amazon.DynamoDBv2.DataModel;
using DynamoDb.ConsoleApp.Repositories;

namespace DynamoDb.ConsoleApp.Managers.Books
{
    [DynamoDBTable("Books")]
    public sealed class Book : EntityBase
    {
        [DynamoDBProperty]
        public string Title { get; set; }

        [DynamoDBProperty]
        public string Description { get; set; }

        [DynamoDBProperty]
        public DateTime CreationDate { get; set; }
    }
}