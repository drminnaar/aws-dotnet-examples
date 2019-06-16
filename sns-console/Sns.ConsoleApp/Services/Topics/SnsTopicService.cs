using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace Sns.ConsoleApp.Services.Topics
{
    public sealed class SnsTopicService : ITopicService
    {
        private readonly IAmazonSimpleNotificationService _sns;

        public SnsTopicService(IAmazonSimpleNotificationService sns)
        {
            _sns = sns ?? throw new ArgumentNullException(nameof(sns));
        }

        public Task<string> CreateTopicAsync(string topicName, IDictionary<string, string> attributes = null)
        {
            if (string.IsNullOrWhiteSpace(topicName))
                throw new ArgumentException($"A non-null/empty '{topicName}' is required.", nameof(topicName));

            return createTopicAsync();

            async Task<string> createTopicAsync()
            {
                var response = await _sns.CreateTopicAsync(topicName);

                if (response.HttpStatusCode != HttpStatusCode.OK)
                    throw new InvalidOperationException($"Topic creation failed for topic '{topicName}'.");

                if (attributes != null && attributes.Any())
                {
                    foreach (var attribute in attributes)
                    {
                        var request = new SetTopicAttributesRequest
                        {
                            AttributeName = attribute.Key,
                            AttributeValue = attribute.Value,
                            TopicArn = response.TopicArn
                        };

                        var setTopicResponse = await _sns.SetTopicAttributesAsync(request);

                        if (setTopicResponse.HttpStatusCode != HttpStatusCode.OK)
                            throw new InvalidOperationException($"Unable to set attributes for topic '{topicName}'");
                    }
                }

                return response.TopicArn;
            }
        }

        public Task DeleteTopicAsync(string topicName)
        {
            if (string.IsNullOrWhiteSpace(topicName))
                throw new ArgumentException($"A non-null/empty '{topicName}' is required.", nameof(topicName));

            return deleteTopicAsync();
            
            async Task deleteTopicAsync()
            {
                var topic = await _sns.FindTopicAsync(topicName);

                if (topic == null)
                    throw new InvalidOperationException($"A topic having name '{topicName}' does not exist.");

                var response = await _sns.DeleteTopicAsync(topic.TopicArn);

                if (response.HttpStatusCode != HttpStatusCode.OK)
                    throw new InvalidOperationException($"Unable to delete topic '{topicName}'.");
            }
        }

        public async Task<IReadOnlyList<TopicDetail>> GetAllTopicsAsync()
        {
            var topics = new List<TopicDetail>();
            var request = new ListTopicsRequest();
            ListTopicsResponse response = null;

            do
            {
                response = await _sns.ListTopicsAsync(request);

                if (response.HttpStatusCode != HttpStatusCode.OK)
                    throw new InvalidOperationException($"Unable to list all topics.");

                foreach (var topic in response.Topics)
                {
                    var topicArn = new TopicArn(topic.TopicArn);

                    topics.Add(new TopicDetail
                    {
                        Attributes = await GetTopicAttributesAsync(topic.TopicArn),
                        Name = topicArn.TopicName,
                        Region = topicArn.Region
                    });
                }

                request.NextToken = response.NextToken;

            } while (response.NextToken != null);

            return topics;
        }

        public Task<TopicDetail> GetTopicAsync(string topicName)
        {
            if (string.IsNullOrWhiteSpace(topicName))
                throw new ArgumentException($"A non-null/empty '{topicName}' is required.", nameof(topicName));

            return getTopicAsync();

            async Task<TopicDetail> getTopicAsync()
            {
                var topic = await _sns.FindTopicAsync(topicName);

                if (topic == null)
                    return null;

                var topicArn = new TopicArn(topic.TopicArn);

                return new TopicDetail
                {
                    Attributes = await GetTopicAttributesAsync(topic.TopicArn),
                    Name = topicArn.TopicName,
                    Region = topicArn.Region
                };
            }
        }

        private async Task<Dictionary<string, string>> GetTopicAttributesAsync(string topicArn)
        {
            var request = new GetTopicAttributesRequest
            {
                TopicArn = topicArn
            };

            var response = await _sns.GetTopicAttributesAsync(request);

            if (response.HttpStatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException($"Unable to retrieve attributes for topic Arn '{topicArn}'");

            // The policy attributes are very verbose and I've chosen to
            // exclude them here
            return response
                .Attributes
                .Where(attribute => !attribute.Key.ToLower().Contains("policy"))
                .ToDictionary(x => x.Key, y => y.Value);
        }
    }
}