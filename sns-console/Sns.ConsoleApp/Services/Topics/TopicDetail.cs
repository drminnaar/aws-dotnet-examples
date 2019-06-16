using System.Collections.Generic;

namespace Sns.ConsoleApp.Services.Topics
{
    public sealed class TopicDetail
    {
        public string Name { get; set; }
        public string Region { get; set; }
        public IReadOnlyDictionary<string, string> Attributes { get; set; }
    }
}