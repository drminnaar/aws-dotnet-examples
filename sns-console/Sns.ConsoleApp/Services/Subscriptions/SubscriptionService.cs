using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace Sns.ConsoleApp.Services.Subscriptions
{
    public sealed class SubscriptionService : ISubscriptionService
    {
        private readonly IAmazonSimpleNotificationService _sns;

        public SubscriptionService(IAmazonSimpleNotificationService sns)
        {
            _sns = sns ?? throw new ArgumentNullException(nameof(sns));
        }

        public Task<SubscriptionCancellation> CancelSubscriptionAsync(string subscriptionArn)
        {
            if (string.IsNullOrWhiteSpace(subscriptionArn))
                throw new ArgumentException($"A non-null/empty '{subscriptionArn}' is required.", nameof(subscriptionArn));

            return cancelSubscriptionAsync();

            async Task<SubscriptionCancellation> cancelSubscriptionAsync()
            {
                try
                {
                    var subscriptionAttributesResponse = await _sns.GetSubscriptionAttributesAsync(subscriptionArn);
                }
                catch (NotFoundException)
                {
                    throw new NotFoundException($"A subscription having Arn '{subscriptionArn}' does not exist.");
                }

                var unsubscribeRequest = new UnsubscribeRequest
                {
                    SubscriptionArn = subscriptionArn
                };

                var unsubscribeResponse = await _sns.UnsubscribeAsync(unsubscribeRequest);

                if (unsubscribeResponse.HttpStatusCode != HttpStatusCode.OK)
                    throw new InvalidOperationException($"Subscription cancellation failed for subject ARN '{subscriptionArn}'.");

                return new SubscriptionCancellation
                {
                    SubscriptionArn = subscriptionArn
                };
            }
        }

        public Task<Subscription> CreateEmailSubscriptionAsync(string topicName, string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(topicName))
                throw new ArgumentException($"A non-null/empty '{topicName}' is required.", nameof(topicName));

            if (string.IsNullOrWhiteSpace(emailAddress))
                throw new ArgumentException($"A non-null/empty '{emailAddress}' is required.", nameof(emailAddress));

            return subscribeAsync();

            async Task<Subscription> subscribeAsync()
            {
                var topic = await _sns.FindTopicAsync(topicName);

                if (topic == null)
                    throw new ArgumentException($"The topic '{topicName}' does not exist.");

                var subscribeRequest = new SubscribeRequest
                {
                    Endpoint = emailAddress,
                    Protocol = "email",
                    TopicArn = topic.TopicArn
                };

                var subscribeResponse = await _sns.SubscribeAsync(subscribeRequest);

                return new Subscription
                {
                    SubscriptionArn = subscribeResponse.SubscriptionArn,
                    TopicArn = topic.TopicArn
                };
            }
        }

        public Task<IReadOnlyList<Subscription>> ListSubscriptionsByTopicAsync(string topicName)
        {
            if (string.IsNullOrWhiteSpace(topicName))
                throw new ArgumentException($"A non-null/empty '{topicName}' is required.", nameof(topicName));

            return listSubscriptionsAsync();

            async Task<IReadOnlyList<Subscription>> listSubscriptionsAsync()
            {
                var topic = await _sns.FindTopicAsync(topicName);

                if (topic == null)
                    throw new ArgumentException($"The topic '{topicName}' does not exist.");

                var request = new ListSubscriptionsByTopicRequest
                {
                    TopicArn = topic.TopicArn
                };

                ListSubscriptionsByTopicResponse response = null;

                var subscriptions = new List<Subscription>();

                do
                {
                    response = await _sns.ListSubscriptionsByTopicAsync(request);

                    if (response.HttpStatusCode != HttpStatusCode.OK)
                        throw new InvalidOperationException($"Unable to list subscriptions for topic '{topicName}'");

                    subscriptions.AddRange(response.Subscriptions);
                    request.NextToken = response.NextToken;
                } while (response.NextToken != null);

                return subscriptions;
            }
        }
    }
}