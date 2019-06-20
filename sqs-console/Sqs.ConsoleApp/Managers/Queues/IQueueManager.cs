using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sqs.ConsoleApp.Managers.Queues
{
    public interface IQueueManager
    {
         Task<string> CreateQueueAsync(string queueName);
         Task<string> DeleteQueueAsync(string queueName);
         Task<string> GetQueueUrlAsync(string queueName);
         Task<IReadOnlyList<string>> ListAllQueuesAsync();
    }
}