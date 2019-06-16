using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sns.ConsoleApp.Services.Topics
{
    public interface ITopicService
    {
        Task<string> CreateTopicAsync(string topicName, IDictionary<string, string> attributes = null);
        Task DeleteTopicAsync(string topicName);
        Task<IReadOnlyList<TopicDetail>> GetAllTopicsAsync();
        Task<TopicDetail> GetTopicAsync(string topicName);
    }
}