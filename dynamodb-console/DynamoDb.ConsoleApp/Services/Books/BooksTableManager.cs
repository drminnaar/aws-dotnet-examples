using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using DynamoDb.ConsoleApp.Repositories;

namespace DynamoDb.ConsoleApp.Services.Books
{
    public sealed class BooksTableManager : IBooksTableManager
    {
        private const string TableName = "AwsDotnetExamples.Books";
        private readonly ITableRepository _tableRepository;

        public BooksTableManager(ITableRepository tableRepository)
        {
            _tableRepository = tableRepository ?? throw new ArgumentNullException(nameof(tableRepository));
        }

        public async Task CreateBooksTableAsync()
        {
            var keySchema = new List<KeySchemaElement>
            {
                new KeySchemaElement("Id", KeyType.HASH)
            };

            var attributes = new List<AttributeDefinition>
            {
                new AttributeDefinition("Id", ScalarAttributeType.S)
            };

            var throughput = new ProvisionedThroughput(readCapacityUnits: 5, writeCapacityUnits: 5);

            await _tableRepository.CreateTableAsync(TableName, keySchema, attributes, throughput);
        }

        public Task DeleteBooksTableAsync()
        {
            return _tableRepository.DeleteTableAsync(TableName);
        }

        public Task<TableDescription> DescribeBooksTableAsync()
        {
            return _tableRepository.DescribeTableAsync(TableName);
        }

        public Task WaitUntilBooksTableReadyAsync()
        {
            return _tableRepository.WaitUntilTableReadyAsync(TableName);
        }
    }
}