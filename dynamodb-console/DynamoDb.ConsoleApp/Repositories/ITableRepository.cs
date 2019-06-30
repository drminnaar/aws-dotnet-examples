using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;

namespace DynamoDb.ConsoleApp.Repositories
{
    public interface ITableRepository
    {
         Task CreateTableAsync(
            string tableName,
            IReadOnlyList<KeySchemaElement> keySchema,
            IReadOnlyList<AttributeDefinition> attributes,
            ProvisionedThroughput provisionedThroughput);

         Task CreateTableAndWaitUntilTableReadyAsync(
             string tableName,
            IReadOnlyList<KeySchemaElement> keySchema,
            IReadOnlyList<AttributeDefinition> attributes,
            ProvisionedThroughput provisionedThroughput);

         Task DeleteTableAsync(string tableName);

         Task<bool> IsExistingTableAsync(string tableName);

         Task<IReadOnlyList<string>> GetTableNameListAsync();

         Task<TableDescription> DescribeTableAsync(string tableName);

         Task<IReadOnlyList<TableDescription>> DescribeTablesAsync(params string[] tableNames);

         Task WaitUntilTableReadyAsync(string tableName);
    }
}
