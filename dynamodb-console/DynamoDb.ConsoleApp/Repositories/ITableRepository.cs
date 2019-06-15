using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;

namespace DynamoDb.ConsoleApp.Repositories
{
    public interface ITableRepository
    {
         Task CreateTableAsync(string tableName);
         Task CreateTableAndWaitUntilTableReadyAsync(string tableName);
         Task DeleteTableAsync(string tableName);
         Task<bool> ExistsAsync(string tableName);
         Task<IReadOnlyList<string>> GetAllTablesAsync();
         Task<TableDescription> GetTableAsync(string tableName);
         Task WaitUntilTableReadyAsync(string tableName);
    }
}
