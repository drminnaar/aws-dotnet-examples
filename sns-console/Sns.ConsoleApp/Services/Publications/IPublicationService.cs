using System.Threading.Tasks;

namespace Sns.ConsoleApp.Services.Publications
{
    public interface IPublicationService<T>
    {
        Task<PublishConfirmation> PublishMessageAsync(string topicName, T message, string subject);
    }
}