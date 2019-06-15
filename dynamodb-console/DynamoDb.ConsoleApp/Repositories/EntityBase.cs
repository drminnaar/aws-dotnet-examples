using System;
using Amazon.DynamoDBv2.DataModel;

namespace DynamoDb.ConsoleApp.Repositories
{
    public abstract class EntityBase
    {
        protected EntityBase()
        {            
        }

        [DynamoDBHashKey]
        public Guid Id { get; set; }
    }
}
