using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SQS;
using Newtonsoft.Json;

namespace Sqs.ConsoleApp.Managers.Games
{
    public sealed class GameRankQueueManager : IGameRankQueueManager
    {
        private readonly IAmazonSQS _sqs;

        public GameRankQueueManager(IAmazonSQS sqs)
        {
            _sqs = sqs ?? throw new ArgumentNullException(nameof(sqs));
        }

        public async Task<string> EnqueueGameRankAsync(string queueName, GameRank gameRank)
        {
            var getUrlResponse = await _sqs.GetQueueUrlAsync(queueName);

            var sendMessageResponse = await _sqs.SendMessageAsync(
                getUrlResponse.QueueUrl,
                JsonConvert.SerializeObject(gameRank));

            return sendMessageResponse.MessageId;
        }

        public async Task<IReadOnlyList<GameRank>> DequeueGameRanksAsync(string queueName)
        {
            var getUrlResponse = await _sqs.GetQueueUrlAsync(queueName);
            var receiveMessageResponse = await _sqs.ReceiveMessageAsync(getUrlResponse.QueueUrl);

            var gameRankings = new List<GameRank>();
            
            foreach (var message in receiveMessageResponse.Messages)
            {
                await _sqs.DeleteMessageAsync(getUrlResponse.QueueUrl, message.ReceiptHandle);
                gameRankings.Add(JsonConvert.DeserializeObject<GameRank>(message.Body));
            }

            return gameRankings;
        }
    }
}