using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using DynamoDb.ConsoleApp.Repositories;

namespace DynamoDb.ConsoleApp.Managers.Tables
{
    public sealed class DynamoDbTableManager : IDynamoDbTableManager
    {
        private readonly ITableRepository _tableRepository;

        public DynamoDbTableManager(ITableRepository tableRepository)
        {
            _tableRepository = tableRepository ?? throw new ArgumentNullException(nameof(tableRepository));
        }

        public Task CreateTableAsync(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"A non-null/empty table name is required.", nameof(tableName));

            return _tableRepository.CreateTableAsync(tableName);
        }

        public Task CreateTableAndWaitUntilTableReadyAsync(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"A non-null/empty table name is required.", nameof(tableName));

            return _tableRepository.CreateTableAndWaitUntilTableReadyAsync(tableName);
        }

        public Task DeleteTableAsync(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"A non-null/empty table name is required.", nameof(tableName));

            return _tableRepository.DeleteTableAsync(tableName);
        }

        public Task<bool> ExistsAsync(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"A non-null/empty table name is required.", nameof(tableName));

            return _tableRepository.ExistsAsync(tableName);
        }

        public Task<TableDescription> GetTableAsync(string tableName)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                throw new ArgumentException($"A non-null/empty table name is required.", nameof(tableName));

            return _tableRepository.GetTableAsync(tableName);
        }

        public async Task<IReadOnlyList<string>> GatAllTablesAsync()
        {
            return await _tableRepository.GetAllTablesAsync();
        }
    }
}