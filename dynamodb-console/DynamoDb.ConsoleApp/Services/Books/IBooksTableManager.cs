using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;

namespace DynamoDb.ConsoleApp.Services.Books
{
    public interface IBooksTableManager
    {
         Task CreateBooksTableAsync();
         Task DeleteBooksTableAsync();
         Task<TableDescription?> DescribeBooksTableAsync();
         Task WaitUntilBooksTableReadyAsync();
    }
}