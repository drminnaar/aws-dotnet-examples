using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sqs.ConsoleApp.Managers.Games
{
    public interface IGameRankQueueManager
    {
         Task<string> EnqueueGameRankAsync(string queueName, GameRank gameRank);
         Task<IReadOnlyList<GameRank>> DequeueGameRanksAsync(string queueName);
    }
}