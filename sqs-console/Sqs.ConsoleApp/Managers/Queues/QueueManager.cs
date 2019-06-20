using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS;

namespace Sqs.ConsoleApp.Managers.Queues
{
    public sealed class QueueManager : IQueueManager
    {
        private readonly IAmazonSQS _sqs;

        public QueueManager(IAmazonSQS sqs)
        {
            _sqs = sqs ?? throw new ArgumentNullException(nameof(sqs));
        }

        public async Task<string> CreateQueueAsync(string queueName)
        {
            var createResponse = await _sqs.CreateQueueAsync(queueName);
            return createResponse.QueueUrl;
        }

        public async Task<string> DeleteQueueAsync(string queueName)
        {
            var getUrlResponse = await _sqs.GetQueueUrlAsync(queueName);
            var deleteResponse = await _sqs.DeleteQueueAsync(getUrlResponse.QueueUrl);
            return getUrlResponse.QueueUrl;
        }

        public async Task<string> GetQueueUrlAsync(string queueName)
        {
            var getUrlResponse = await _sqs.GetQueueUrlAsync(queueName);
            return getUrlResponse.QueueUrl;
        }

        public async Task<IReadOnlyList<string>> ListAllQueuesAsync()
        {
            var response = await _sqs.ListQueuesAsync(string.Empty);            
            return response.QueueUrls;
        }
    }
}