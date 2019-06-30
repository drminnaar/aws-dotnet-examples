using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamoDb.ConsoleApp.Repositories
{
    public interface IEntityRepository<T> where T : class
    {
        Task DeleteAsync(object hashKey);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetAsync(object hashKey);
        Task SaveAsync(T entity);
    }
}
