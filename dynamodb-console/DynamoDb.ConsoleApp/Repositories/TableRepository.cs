using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace DynamoDb.ConsoleApp.Repositories
{
    public sealed class TableRepository : ITableRepository
    {
        private readonly IAmazonDynamoDB _amazonDb;

        public TableRepository(IAmazonDynamoDB amazonDb)
        {
            _amazonDb = amazonDb ?? throw new ArgumentNullException(nameof(amazonDb));
        }

        public Task CreateTableAsync(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"A non-null/empty table name is required.", nameof(tableName));

            return createTableAsync();

            async Task createTableAsync()
            {
                if (!await ExistsAsync(tableName))
                {
                    var request = new CreateTableRequest
                    {
                        TableName = tableName,
                        AttributeDefinitions = new List<AttributeDefinition>()
                        {
                            new AttributeDefinition("Id", ScalarAttributeType.S)
                        },
                        KeySchema = new List<KeySchemaElement>()
                        {
                            new KeySchemaElement("Id", KeyType.HASH)
                        },
                        ProvisionedThroughput = new ProvisionedThroughput(readCapacityUnits: 10, writeCapacityUnits: 5)
                    };

                    await _amazonDb.CreateTableAsync(request);
                }
            }
        }

        public Task CreateTableAndWaitUntilTableReadyAsync(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"A non-null/empty table name is required.", nameof(tableName));

            return createTableAndWaitAsync();

            async Task createTableAndWaitAsync()
            {
                await CreateTableAsync(tableName);
                await WaitUntilTableReadyAsync(tableName);
            }
        }

        /// <summary>
        /// Check every 5 seconds for 1 minute until table creation completed.
        /// </summary>
        public async Task WaitUntilTableReadyAsync(string tableName)
        {
            TableStatus status = null;
            TimeSpan maxWaitTime = TimeSpan.FromMinutes(1);

            do
            {
                await Task.Delay(3000);
                maxWaitTime = maxWaitTime.Subtract(TimeSpan.FromSeconds(5));

                try
                {
                    var response = await _amazonDb.DescribeTableAsync(new DescribeTableRequest
                    {
                        TableName = tableName
                    });

                    Console.WriteLine($"Table name: {response.Table.TableName}, status: {response.Table.TableStatus}");
                    status = response.Table.TableStatus;
                }
                catch (ResourceNotFoundException)
                {
                    // The table may not be created yet. This operation is eventually consistent
                    // and for now we just ignore the exception
                    Console.WriteLine($"Table '{tableName}' not yet created.");
                }
            } while (status != TableStatus.ACTIVE || maxWaitTime.TotalSeconds <= 0);
        }

        public Task DeleteTableAsync(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"A non-null/empty table name is required.", nameof(tableName));

            return deleteTableAsync();

            async Task deleteTableAsync()
            {
                if (!await ExistsAsync(tableName))
                    throw new InvalidOperationException($"A table having name '{tableName}' does not exist.");
                
                await _amazonDb.DeleteTableAsync(tableName);
            }
        }

        public Task<bool> ExistsAsync(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"A non-null/empty table name is required.", nameof(tableName));

            return existsAsync();

            async Task<bool> existsAsync()
            {
                var response = await _amazonDb.ListTablesAsync();
                return tableName == response.TableNames.FirstOrDefault(t => t == tableName);
            }
        }

        public Task<TableDescription> GetTableAsync(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"A non-null/empty table name is required.", nameof(tableName));

            return getTableAsync();

            async Task<TableDescription> getTableAsync()
            {
                if (!await ExistsAsync(tableName))
                    return null;

                var tableInfo = await _amazonDb.DescribeTableAsync(tableName);

                return tableInfo.Table;
            }
        }

        public async Task<IReadOnlyList<string>> GetAllTablesAsync()
        {
            var response = await _amazonDb.ListTablesAsync();
            return response.TableNames;
        }

    }
}
