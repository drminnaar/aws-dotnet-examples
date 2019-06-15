using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;

namespace DynamoDb.ConsoleApp.Managers.Tables
{
    public interface IDynamoDbTableManager
    {
        Task CreateTableAsync(string tableName);
        Task CreateTableAndWaitUntilTableReadyAsync(string tableName);
        Task DeleteTableAsync(string tableName);
        Task<bool> ExistsAsync(string tableName);
        Task<TableDescription> GetTableAsync(string tableName);
        Task<IReadOnlyList<string>> GatAllTablesAsync();
    }
}