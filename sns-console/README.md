![aws-sns-dotnet-github](https://user-images.githubusercontent.com/33935506/114360671-66c74800-9bc9-11eb-9e1c-466a8c0309b1.png)

# SNS (Simple Noticiation Service) README

This project demonstrates how to integrate a .NET Core console application with SNS using the Amazon SDK.

* Learn how to setup Dependency Injection (DI) inside a .NET Core console application
  * Setup AWS services
  * Setup logging services
  * Setup application dependencies
* Learn what the Amazon SDK Nuget packages are required to integrate with SNS

---

## Feature Overview

This application provides the functionality required to manage SNS topics, subscriptions, and publications.

The following features have been implemented:

* [Manage Topics](#manage-topics)
  * Create topic
  * List and find topics  
  * Delete topic
* [Manage Subscriptions](manage-subscriptions)
  * Create an email subscription
  * Cancel a subscription
  * List subscriptions
* [Manage Publications](#manage-publications)
  * An example showing how to publish a 'Game Ranking' to a 'game-ranking' topic

---

## Getting Started

### Configure User Secrets

```powershell
# change directory to Sns.ConsoleApp
cd ./Sns.ConsoleApp

# initialize user secrets
dotnet user-secrets init

# add AWS configuration options to user secrets
dotnet user-secrets set "AWS:Profile" "<YOUR_CHOSEN_PROFILE_NAME>"
dotnet user-secrets set "AWS:Region" "<YOUR_CHOSEN_REGION_NAME>"

# verify that secrets were added correctly and successfully
dotnet user-secrets list

AWS:Region = <YOUR_CHOSEN_REGION_NAME>
AWS:Profile = <YOUR_CHOSEN_PROFILE_NAME>

# clear secrets if you no longer need them
dotnet user-secrets clear
```

### Start Application

```powershell
# change directory to S3.ConsoleApp
cd ./S3.ConsoleApp

# run app
dotnet run
```

![aws-sns-demo](https://user-images.githubusercontent.com/33935506/114331020-32d32f00-9b97-11eb-8041-3a9a923df51b.png)

---

## Feature Walkthrough

### Manage Topics

#### Create Topic

![aws-sns-create-topic](https://user-images.githubusercontent.com/33935506/114331773-dec94a00-9b98-11eb-9876-1b4e57def6c6.png)

```csharp
// FILE: SnsTopicService.cs

public Task<string> CreateTopicAsync(string topicName, IDictionary<string, string>? attributes = null)
{
    if (string.IsNullOrWhiteSpace(topicName))
        throw new ArgumentException($"A non-null/empty '{topicName}' is required.", nameof(topicName));

    return createTopicAsync();

    async Task<string> createTopicAsync()
    {
        var response = await _sns.CreateTopicAsync(topicName);

        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new InvalidOperationException($"Topic creation failed for topic '{topicName}'.");

        if (attributes != null && attributes.Any())
        {
            foreach (var attribute in attributes)
            {
                var request = new SetTopicAttributesRequest
                {
                    AttributeName = attribute.Key,
                    AttributeValue = attribute.Value,
                    TopicArn = response.TopicArn
                };

                var setTopicResponse = await _sns.SetTopicAttributesAsync(request);

                if (setTopicResponse.HttpStatusCode != HttpStatusCode.OK)
                    throw new InvalidOperationException($"Unable to set attributes for topic '{topicName}'");
            }
        }

        return response.TopicArn;
    }
}
```

#### Get Topic

![image](https://user-images.githubusercontent.com/33935506/114333141-b7c04780-9b9b-11eb-81ff-99f4d20237e8.png)

```csharp
// FILE: SnsTopicService.cs

public Task<TopicDetail?> GetTopicAsync(string topicName)
{
    if (string.IsNullOrWhiteSpace(topicName))
        throw new ArgumentException($"A non-null/empty '{topicName}' is required.", nameof(topicName));

    return getTopicAsync();

    async Task<TopicDetail?> getTopicAsync()
    {
        var topic = await _sns.FindTopicAsync(topicName);

        if (topic == null)
            return default;

        var topicArn = new TopicArn(topic.TopicArn);

        return new TopicDetail
        {
            Attributes = await GetTopicAttributesAsync(topic.TopicArn),
            Name = topicArn.TopicName,
            Region = topicArn.Region
        };
    }
}
```

#### List Topics

![aws-sns-get-topics](https://user-images.githubusercontent.com/33935506/114333544-a9266000-9b9c-11eb-91d9-ab296bcac3cf.png)

```csharp
// FILE: SnsTopicService.cs

public async Task<IReadOnlyList<TopicDetail>> GetAllTopicsAsync()
{
    var topics = new List<TopicDetail>();
    var request = new ListTopicsRequest();
    ListTopicsResponse response;
    do
    {
        response = await _sns.ListTopicsAsync(request);

        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new InvalidOperationException($"Unable to list all topics.");

        foreach (var topic in response.Topics)
        {
            var topicArn = new TopicArn(topic.TopicArn);

            topics.Add(new TopicDetail
            {
                Attributes = await GetTopicAttributesAsync(topic.TopicArn),
                Name = topicArn.TopicName,
                Region = topicArn.Region
            });
        }

        request.NextToken = response.NextToken;

    } while (response.NextToken != null);

    return topics;
}
```

#### Delete Topic

![aws-sns-delete-topic](https://user-images.githubusercontent.com/33935506/114334091-f2c37a80-9b9d-11eb-9bc5-427e72ff282c.png)

```csharp
// FILE: SnsTopicService.cs

public Task DeleteTopicAsync(string topicName)
{
    if (string.IsNullOrWhiteSpace(topicName))
        throw new ArgumentException($"A non-null/empty '{topicName}' is required.", nameof(topicName));

    return deleteTopicAsync();

    async Task deleteTopicAsync()
    {
        var topic = await _sns.FindTopicAsync(topicName);

        if (topic == null)
            throw new InvalidOperationException($"A topic having name '{topicName}' does not exist.");

        var response = await _sns.DeleteTopicAsync(topic.TopicArn);

        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new InvalidOperationException($"Unable to delete topic '{topicName}'.");
    }
}
```

### Manage Subscriptions

#### List Subscriptions

![aws-sns-list-subscriptions](https://user-images.githubusercontent.com/33935506/114340518-d4647b80-9bab-11eb-9a31-b723ff5e2f7c.png)

```csharp
// FILE: SubscriptionService.cs

public Task<IReadOnlyList<Subscription>> ListSubscriptionsByTopicAsync(string topicName)
{
    if (string.IsNullOrWhiteSpace(topicName))
        throw new ArgumentException($"A non-null/empty '{topicName}' is required.", nameof(topicName));

    return listSubscriptionsAsync();

    async Task<IReadOnlyList<Subscription>> listSubscriptionsAsync()
    {
        var topic = await _sns.FindTopicAsync(topicName);

        if (topic == null)
            throw new ArgumentException($"The topic '{topicName}' does not exist.");

        var request = new ListSubscriptionsByTopicRequest
        {
            TopicArn = topic.TopicArn
        };

        var subscriptions = new List<Subscription>();

        ListSubscriptionsByTopicResponse response;
        do
        {
            response = await _sns.ListSubscriptionsByTopicAsync(request);

            if (response.HttpStatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException($"Unable to list subscriptions for topic '{topicName}'");

            subscriptions.AddRange(response.Subscriptions);
            request.NextToken = response.NextToken;
        } while (response.NextToken != null);

        return subscriptions;
    }
}
```

#### Create Email Subscription

STEP 1: Create Email Subscription

![aws-sns-create-email-subscription](https://user-images.githubusercontent.com/33935506/114340517-d4647b80-9bab-11eb-858a-47da12596b74.png)

STEP 2: Confirmation Email Sent (Mailinator used for demo purposes)

![aws-sns-mailinator1](https://user-images.githubusercontent.com/33935506/114348066-257b6c00-9bba-11eb-9fad-4527826edb0c.png)

STEP 3: Confirm Subscription

![aws-sns-mailinator2](https://user-images.githubusercontent.com/33935506/114348067-26ac9900-9bba-11eb-938b-c635ddfa1563.png)

STEP 4: Email Subscription Confirmed

![aws-sns-mailinator3](https://user-images.githubusercontent.com/33935506/114348068-27452f80-9bba-11eb-8ab0-1ad661cc5e87.png)

```csharp
// FILE: SubscriptionService.cs

public Task<Subscription> CreateEmailSubscriptionAsync(string topicName, string emailAddress)
{
    if (string.IsNullOrWhiteSpace(topicName))
        throw new ArgumentException($"A non-null/empty '{topicName}' is required.", nameof(topicName));

    if (string.IsNullOrWhiteSpace(emailAddress))
        throw new ArgumentException($"A non-null/empty '{emailAddress}' is required.", nameof(emailAddress));

    return subscribeAsync();

    async Task<Subscription> subscribeAsync()
    {
        var topic = await _sns.FindTopicAsync(topicName);

        if (topic == null)
            throw new ArgumentException($"The topic '{topicName}' does not exist.");

        var subscribeRequest = new SubscribeRequest
        {
            Endpoint = emailAddress,
            Protocol = "email",
            TopicArn = topic.TopicArn
        };

        var subscribeResponse = await _sns.SubscribeAsync(subscribeRequest);

        return new Subscription
        {
            SubscriptionArn = subscribeResponse.SubscriptionArn,
            TopicArn = topic.TopicArn
        };
    }
}
```

#### Cancel Subscription

![aws-sns-cancel-subscription](https://user-images.githubusercontent.com/33935506/114340513-d3334e80-9bab-11eb-8373-8fe889ef1c6a.png)

```csharp
// FILE: SubscriptionService.cs

public Task<SubscriptionCancellation> CancelSubscriptionAsync(string subscriptionArn)
{
    if (string.IsNullOrWhiteSpace(subscriptionArn))
        throw new ArgumentException($"A non-null/empty '{subscriptionArn}' is required.", nameof(subscriptionArn));

    return cancelSubscriptionAsync();

    async Task<SubscriptionCancellation> cancelSubscriptionAsync()
    {
        try
        {
            var subscriptionAttributesResponse = await _sns.GetSubscriptionAttributesAsync(subscriptionArn);
        }
        catch (NotFoundException)
        {
            throw new NotFoundException($"A subscription having Arn '{subscriptionArn}' does not exist.");
        }

        var unsubscribeRequest = new UnsubscribeRequest
        {
            SubscriptionArn = subscriptionArn
        };

        var unsubscribeResponse = await _sns.UnsubscribeAsync(unsubscribeRequest);

        if (unsubscribeResponse.HttpStatusCode != HttpStatusCode.OK)
            throw new InvalidOperationException($"Subscription cancellation failed for subject ARN '{subscriptionArn}'.");

        return new SubscriptionCancellation
        {
            SubscriptionArn = subscriptionArn
        };
    }
}
```

### Manage Publications

#### Publish Game Ranking

STEP 1: Create `game-ranking` Topic

Before publishing an event, you need to create a **game-ranking** topic using the **Topic Manager**.

STEP 2: Publish Event To `game-ranking` Topic

![aws-sns-publish](https://user-images.githubusercontent.com/33935506/114349615-4218a380-9bbc-11eb-86d9-fd34d01488c8.png)

STEP 3: View Email Notification

![aws-sns-publish-mailinator1](https://user-images.githubusercontent.com/33935506/114349618-4349d080-9bbc-11eb-8572-f7bc0c9e7f35.png)

![aws-sns-publish-mailinator2](https://user-images.githubusercontent.com/33935506/114349620-4349d080-9bbc-11eb-873a-0eface9556ed.png)

```csharp
// FILE: PublicationService.cs

public Task<PublishConfirmation> PublishMessageAsync(string topicName, T message, string subject)
{
    if (string.IsNullOrWhiteSpace(topicName))
        throw new ArgumentException($"A non-null/empty '{topicName}' is required.", nameof(topicName));

    if (message == null)
        throw new ArgumentNullException(nameof(message), $"A non-null '{message}' is required.");

    if (string.IsNullOrWhiteSpace(subject))
        throw new ArgumentException($"A non-null/empty '{subject}' is required.", nameof(subject));

    return publishAsync();

    async Task<PublishConfirmation> publishAsync()
    {
        var topic = await _sns.FindTopicAsync(topicName);

        if (topic == null)
            throw new ArgumentException($"The topic '{topicName}' does not exist.");

        var request = new PublishRequest
        {
            Message = JsonSerializer.Serialize(message),
            Subject = subject,
            TopicArn = topic.TopicArn
        };

        var response = await _sns.PublishAsync(request);

        if (response.HttpStatusCode != HttpStatusCode.OK)
            throw new InvalidOperationException($"Publish failed for topic '{topicName}'");

        return new PublishConfirmation
        {
            MessageId = response.MessageId
        };
    }
}
```

---

## Notable Nuget Packages

* [AWSSDK.SimpleNotificationService] - Amazon Simple Notification Service (Amazon SNS) is a fast, flexible, fully managed push messaging service.

* [AWSSDK.Extensions.NETCore.Setup] - Extensions for the AWS SDK for .NET to integrate with .NET Core configuration and dependency injection frameworks.

---

[AWSSDK.SimpleNotificationService]: https://www.nuget.org/packages/AWSSDK.SimpleNotificationService/
[AWSSDK.Extensions.NETCore.Setup]: https://www.nuget.org/packages/AWSSDK.Extensions.NETCore.Setup/