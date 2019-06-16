using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService.Model;

namespace Sns.ConsoleApp.Services.Subscriptions
{
    public interface ISubscriptionService
    {
        Task<SubscriptionCancellation> CancelSubscriptionAsync(string subscriptionArn);
        Task<Subscription> CreateEmailSubscriptionAsync(string topicName, string emailAddress);
        Task<IReadOnlyList<Subscription>> ListSubscriptionsByTopicAsync(string topicName);
    }
}