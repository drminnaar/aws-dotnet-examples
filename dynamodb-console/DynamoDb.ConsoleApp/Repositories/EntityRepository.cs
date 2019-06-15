using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace DynamoDb.ConsoleApp.Repositories
{
    public sealed class EntityRepository<T> : IEntityRepository<T> where T : EntityBase
    {
        private readonly IAmazonDynamoDB _dynamoDb;

        public EntityRepository(IAmazonDynamoDB dynamoDb)
        {
            _dynamoDb = dynamoDb ?? throw new ArgumentNullException(nameof(dynamoDb));
        }

        public async Task DeleteEntityAsync(Guid entityId)
        {
            using (var context = new DynamoDBContext(_dynamoDb))
            {
                await context.DeleteAsync<T>(hashKey: entityId);
            }
        }

        public async Task<IReadOnlyList<T>> GetAllEntitiesAsync()
        {
            using (var context = new DynamoDBContext(_dynamoDb))
            {
                return await context
                    .ScanAsync<T>(Enumerable.Empty<ScanCondition>())
                    .GetRemainingAsync()
                    ?? Enumerable.Empty<T>().ToList();
            }
        }

        public async Task<T> GetEntityByIdAsync(Guid entityId)
        {
            using (var context = new DynamoDBContext(_dynamoDb))
            {
                return await context.LoadAsync<T>(entityId);
            }
        }

        public Task<Guid> SaveEntityAsync(T entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            return saveEntityAsync();

            async Task<Guid> saveEntityAsync()
            {
                using (var context = new DynamoDBContext(_dynamoDb))
                {
                    await context.SaveAsync(entity);
                }

                return entity.Id;
            }
        }
    }
}
