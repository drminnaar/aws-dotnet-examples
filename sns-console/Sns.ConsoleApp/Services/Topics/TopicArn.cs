namespace Sns.ConsoleApp.Services.Topics
{
    public sealed class TopicArn
    {
        public TopicArn(string topicArn)
        {
            OriginalTopicArn = topicArn;
            parseTopicArn();

            void parseTopicArn()
            {
                var tokens = topicArn.Split(':');
                Service = tokens[2];
                Region = tokens[3];
                AccountId = tokens[4];
                TopicName = tokens[5];

                // For topic having subscription
                if (tokens.Length == 7)
                    SubscriptionId = tokens[6];
            }
        }

        public string AccountId { get; set; } = string.Empty;
        public string Format { get; set; } = "arn:aws:sns:region:account-id:topicname";
        public string FormatWithSubscription { get; set; } = "arn:aws:sns:region:account-id:topicname:subscriptionid";
        public string Region { get; set; } = string.Empty;
        public string Service { get; set; } = string.Empty;
        public string SubscriptionId { get; set; } = string.Empty;
        public string TopicName { get; set; } = string.Empty;
        public string OriginalTopicArn { get; } = string.Empty;

        public override string ToString()
        {
            return OriginalTopicArn;
        }
    }
}