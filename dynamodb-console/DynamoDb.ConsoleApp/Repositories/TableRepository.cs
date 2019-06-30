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
        private readonly IAmazonDynamoDB _dynamoDb;

        public TableRepository(IAmazonDynamoDB amazonDb)
        {
            _dynamoDb = amazonDb ?? throw new ArgumentNullException(nameof(amazonDb));
        }

        public Task CreateTableAsync(
            string tableName,
            IReadOnlyList<KeySchemaElement> keySchema,
            IReadOnlyList<AttributeDefinition> attributes,
            ProvisionedThroughput provisionedThroughput)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"A non-null/empty table name is required.", nameof(tableName));

            return createTableAsync();

            async Task createTableAsync()
            {
                if (!await IsExistingTableAsync(tableName))
                {
                    var request = new CreateTableRequest
                    {
                        TableName = tableName,
                        AttributeDefinitions = attributes.ToList(),
                        KeySchema = keySchema.ToList(),
                        ProvisionedThroughput = provisionedThroughput
                    };

                    await _dynamoDb.CreateTableAsync(request);
                }
            }
        }

        public Task CreateTableAndWaitUntilTableReadyAsync(
            string tableName,
            IReadOnlyList<KeySchemaElement> keySchema,
            IReadOnlyList<AttributeDefinition> attributes,
            ProvisionedThroughput provisionedThroughput)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"A non-null/empty table name is required.", nameof(tableName));

            return createTableAndWaitAsync();

            async Task createTableAndWaitAsync()
            {
                await CreateTableAsync(tableName, keySchema, attributes, provisionedThroughput);
                await WaitUntilTableReadyAsync(tableName);
            }
        }

        /// <summary>
        /// Check every 5 seconds for 1 minute until table creation completed.
        /// </summary>
        public Task WaitUntilTableReadyAsync(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"A non-null/empty table name is required.", nameof(tableName));

            return waitUntilTableReadyAsync();

            async Task waitUntilTableReadyAsync()
            {
                TableStatus status = null;
                TimeSpan maxWaitTime = TimeSpan.FromMinutes(1);

                do
                {
                    await Task.Delay(3000);
                    maxWaitTime = maxWaitTime.Subtract(TimeSpan.FromSeconds(5));

                    try
                    {
                        var response = await _dynamoDb.DescribeTableAsync(new DescribeTableRequest
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
        }

        public Task DeleteTableAsync(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"A non-null/empty table name is required.", nameof(tableName));

            return deleteTableAsync();

            async Task deleteTableAsync()
            {
                if (!await ExistsAsync(tableName))
                    throw new TableNotFoundException($"A table having name '{tableName}' does not exist.");

                await _dynamoDb.DeleteTableAsync(tableName);
            }
        }

        public Task<bool> IsExistingTableAsync(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"A non-null/empty table name is required.", nameof(tableName));

            return ExistsAsync(tableName);
        }

        private async Task<bool> ExistsAsync(string tableName)
        {
            return (await GetTableNameListAsync()).Any(tn => string.Compare(tn, tableName, true) == 0);
        }

        private async Task<IReadOnlyList<string>> FindTableNameMatchesAsync(params string[] tableNames)
        {
            var existingTableNames = await GetTableNameListAsync();
            return existingTableNames.Intersect(tableNames).ToList();
        }

        public Task<TableDescription> DescribeTableAsync(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"A non-null/empty table name is required.", nameof(tableName));

            return getTableAsync();

            async Task<TableDescription> getTableAsync()
            {
                if (!await ExistsAsync(tableName))
                    return null;

                return (await _dynamoDb.DescribeTableAsync(tableName)).Table;
            }
        }

        public Task<IReadOnlyList<TableDescription>> DescribeTablesAsync(params string[] tableNames)
        {
            if (tableNames == null || !tableNames.Any())
                throw new ArgumentException($"A non-null/empty list of table names is required.", nameof(tableNames));

            return getTableAsync();

            async Task<IReadOnlyList<TableDescription>> getTableAsync()
            {
                var matchingTableNames = await FindTableNameMatchesAsync(tableNames);

                if (!matchingTableNames.Any())
                    return Enumerable.Empty<TableDescription>().ToList();

                var tableDescriptions = new List<TableDescription>(matchingTableNames.Count);

                foreach (var tableName in matchingTableNames)
                    tableDescriptions.Add((await _dynamoDb.DescribeTableAsync(tableName)).Table);
                
                return tableDescriptions;
            }
        }

        public async Task<IReadOnlyList<string>> GetTableNameListAsync()
        {
            var tableNames = new List<string>();
            string startTableName = null;

            do
            {
                var response = await _dynamoDb.ListTablesAsync(new ListTablesRequest
                {
                    ExclusiveStartTableName = startTableName,
                    Limit = 10
                });

                tableNames.AddRange(response.TableNames);
                startTableName = response.LastEvaluatedTableName;

            } while (!string.IsNullOrWhiteSpace(startTableName));

            return tableNames;
        }

    }
}
