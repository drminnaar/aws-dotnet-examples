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

        public string AccountId { get; set; }
        public string Format { get; set; } = "arn:aws:sns:region:account-id:topicname";
        public string FormatWithSubscription { get; set; } = "arn:aws:sns:region:account-id:topicname:subscriptionid";
        public string Region { get; set; }
        public string Service { get; set; }
        public string SubscriptionId { get; set; }
        public string TopicName { get; set; }
        public string OriginalTopicArn { get; }

        public override string ToString()
        {
            return OriginalTopicArn;
        }
    }
}