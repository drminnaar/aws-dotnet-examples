using System;
using System.Threading.Tasks;
using Sns.ConsoleApp.Services.Publications;

namespace Sns.ConsoleApp.Managers.Games
{
    public sealed class GameRankPublicationManager : IGameRankPublicationManager
    {
        private const string TopicName = "game-rankings";
        private const string PublishSubject = "Game Ranking for {0}";

        private readonly IPublicationService<GameRank> _publishService;

        public GameRankPublicationManager(IPublicationService<GameRank> publishService)
        {
            _publishService = publishService ?? throw new ArgumentNullException(nameof(publishService));
        }

        public Task<PublishConfirmation> PublishGameRankingAsync(GameRank gameRank)
        {
            if (gameRank is null)
                throw new ArgumentNullException(nameof(gameRank));

            return _publishService.PublishMessageAsync(
                TopicName,
                gameRank,
                string.Format(PublishSubject, gameRank.GameName));
        }
    }
}