![aws-sqs-dotnet-github](https://user-images.githubusercontent.com/33935506/114374222-849ba980-9bd7-11eb-9c19-f976ddeef2ab.png)

# SQS (Simple Queue Service) README

This project demonstrates how to integrate a .NET Core console application with SQS using the Amazon SDK.

* Learn how to setup Dependency Injection (DI) inside a .NET Core console application
  * Setup AWS services
  * Setup logging services
  * Setup application dependencies
* Learn what the Amazon SDK Nuget packages are required to integrate with SQS

Contents:

* [Feature Overview](#feature-overview)
* [Feature Walkthrough](#feature-walkthrough)
* [Getting Started](#getting-started)
* [Notable Nuget Packages](#notable-nuget-packages)

## Feature Overview

This application provides the functionality required to manage SQS queues.

The following features have been implemented:

* [Manage Queues](#manage-queues)
  * [Create Queue](#create-queue)
  * [List Queues](#list-queues)
  * [Delete Queue](#delete-queue)
  * [Get Queue Url](#get-queue-url)
* [Manage Game Ranking Queue](#manage-game-ranking-queue)
  * [Enqueue Game Ranking](#enqueue-game-ranking)
  * [Dequeue Game Ranking](#dequeue-game-ranking)

---

## Feature Walkthrough

### Manage Queues

#### Create Queue

![aws-sqs-createqueue](https://user-images.githubusercontent.com/33935506/114371322-7c8e3a80-9bd4-11eb-898c-147164c461d3.png)

```csharp
// FILE: QueueManager.cs

public async Task<string> CreateQueueAsync(string queueName)
{
    var createResponse = await _sqs.CreateQueueAsync(queueName);
    return createResponse.QueueUrl;
}
```

#### List Queues

![aws-sqs-listqueues](https://user-images.githubusercontent.com/33935506/114371319-7bf5a400-9bd4-11eb-82fd-a3e2342bebb0.png)

```csharp
// FILE: QueueManager.cs

public async Task<IReadOnlyList<string>> ListAllQueuesAsync()
{
    var response = await _sqs.ListQueuesAsync(string.Empty);            
    return response.QueueUrls;
}
```

#### Delete Queue

![aws-sqs-deletequeue](https://user-images.githubusercontent.com/33935506/114371315-7b5d0d80-9bd4-11eb-919c-9824ab2b3c92.png)

```csharp
// FILE: QueueManager.cs

public async Task<string> DeleteQueueAsync(string queueName)
{
    var getUrlResponse = await _sqs.GetQueueUrlAsync(queueName);
    var deleteResponse = await _sqs.DeleteQueueAsync(getUrlResponse.QueueUrl);
    return getUrlResponse.QueueUrl;
}
```

#### Get Queue Url

![aws-sqs-getqueueurl](https://user-images.githubusercontent.com/33935506/114371317-7b5d0d80-9bd4-11eb-8603-3db30313bcda.png)

```csharp
// FILE: QueueManager.cs

public async Task<string> GetQueueUrlAsync(string queueName)
{
    var getUrlResponse = await _sqs.GetQueueUrlAsync(queueName);
    return getUrlResponse.QueueUrl;
}
```

### Manage Game Ranking Queue

#### Enqueue Game Ranking

![aws-sqs-enqueue](https://user-images.githubusercontent.com/33935506/114371314-7ac47700-9bd4-11eb-9077-c8d18ea7ea78.png)

```csharp
// FILE: GameRankQueueManager.cs

public async Task<string> EnqueueGameRankAsync(string queueName, GameRank gameRank)
{
    var getUrlResponse = await _sqs.GetQueueUrlAsync(queueName);

    var sendMessageResponse = await _sqs.SendMessageAsync(
        getUrlResponse.QueueUrl,
        JsonSerializer.Serialize(gameRank));

    return sendMessageResponse.MessageId;
}
```

#### Dequeue Game Ranking

![aws-sqs-dequeue](https://user-images.githubusercontent.com/33935506/114371309-79934a00-9bd4-11eb-86db-705abad438e1.png)

```csharp
// FILE: GameRankQueueManager.cs

public async Task<IReadOnlyList<GameRank>> DequeueGameRanksAsync(string queueName)
{
    var getUrlResponse = await _sqs.GetQueueUrlAsync(queueName);
    var receiveMessageResponse = await _sqs.ReceiveMessageAsync(getUrlResponse.QueueUrl);

    var gameRankings = new List<GameRank>();

    foreach (var message in receiveMessageResponse.Messages)
    {
        await _sqs.DeleteMessageAsync(getUrlResponse.QueueUrl, message.ReceiptHandle);
        gameRankings.Add(JsonSerializer.Deserialize<GameRank>(message.Body)!);
    }

    return gameRankings;
}
```

---

## Getting Started

### Configure User Secrets

```powershell
# change directory to Sqs.ConsoleApp
cd ./Sqs.ConsoleApp

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
# change directory to Sqs.ConsoleApp
cd ./Sqs.ConsoleApp

# run app
dotnet run
```

![aws-sqs-overview](https://user-images.githubusercontent.com/33935506/114371324-7c8e3a80-9bd4-11eb-8e8d-5d4a126c9d2e.png)

---

### Notable Nuget Packages

* [AWSSDK.SQS] - Amazon Simple Notification Service (Amazon SQS) is a fast, flexible, fully managed push messaging service.

* [AWSSDK.Extensions.NETCore.Setup] - Extensions for the AWS SDK for .NET to integrate with .NET Core configuration and dependency injection frameworks.

---

[AWSSDK.SQS]: https://www.nuget.org/packages/AWSSDK.SQS/
[AWSSDK.Extensions.NETCore.Setup]: https://www.nuget.org/packages/AWSSDK.Extensions.NETCore.Setup/