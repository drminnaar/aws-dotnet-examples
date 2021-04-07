using System;

namespace Sns.ConsoleApp.Managers.Games
{
    public sealed class GameRank
    {
        public string GameName { get; set; } = string.Empty;
        public double GameRating { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}