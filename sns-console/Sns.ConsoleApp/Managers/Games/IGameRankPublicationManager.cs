using System.Threading.Tasks;
using Sns.ConsoleApp.Services.Publications;

namespace Sns.ConsoleApp.Managers.Games
{
    public interface IGameRankPublicationManager
    {
         Task<PublishConfirmation> PublishGameRankingAsync(GameRank gameRank);
    }
}