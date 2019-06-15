using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamoDb.ConsoleApp.Repositories
{
    public interface IEntityRepository<T> where T : EntityBase
    {
        Task DeleteEntityAsync(Guid entityId);
        Task<IReadOnlyList<T>> GetAllEntitiesAsync();
        Task<T> GetEntityByIdAsync(Guid entityId);
        Task<Guid> SaveEntityAsync(T entity);
    }
}
