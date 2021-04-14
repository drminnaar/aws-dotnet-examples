# DynamoDb Console Application README

This project demonstrates how to integrate a .NET Core console application with DynamoDb using the Amazon SDK.

* Learn how to setup Dependency Injection (DI) inside a .NET Core console application
  * Setup AWS services
  * Setup logging services
  * Setup application dependencies
* Learn what the Amazon SDK Nuget packages are required to integrate with DynamoDB

Contents:

* [Getting Started](#getting-started)
  * [Configure User Secrets](#configure-user-secrets)
  * [Start Application](#start-application)
* [Feature Overview](#feature-overview)
* [Feature Walkthrough](#feature-walkthrough)
* [Notable Nuget Packages](#notable-nuget-packages)

---

## Getting Started

### Configure User Secrets

```powershell
# change directory to DynamoDb.ConsoleApp
cd ./DynamoDb.ConsoleApp

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
# change directory to DynamoDb.ConsoleApp
cd ./DynamoDb.ConsoleApp

# run app
dotnet run
```

![dynamodb-1](https://user-images.githubusercontent.com/33935506/114658257-a2871c80-9d45-11eb-9024-d5acde7d266e.png)

---

## Feature Overview

This application provides the functionality required to both manage DynamoDb tables, and manage the data stored in a DynamoDb table

The following features have been implemented:

* [Configure App and AWS SDK](#configure-app-and-aws-sdk)
* [Manage Tables](#manage-tables)
  * [Create Table](#create-table)
  * [List Tables](#list-tables)
  * [Describe Table](#describe-table)
  * [Delete Table](#delete-table)
* [Manage Book Table](#manage-book-table)
  * [Add Book](#add-book)
  * [Update Book](#update-book)
  * [Delete Book](#delete-book)
  * [Find Book](#find-book)

---

## Feature Walkthrough

### Configure App and AWS SDK

```csharp
// FILE: ServiceCollectionFactory

internal static IServiceCollection CreateServices(IConfiguration configuration)
{
    var services = new ServiceCollection();

    services.AddLogging(configure => configure.AddConsole());

    // Add AWS services
    services.AddDefaultAWSOptions(configuration.GetAWSOptions());
    services.AddAWSService<IAmazonDynamoDB>();

    // Add repositories
    services.AddSingleton<IEntityRepository<BookEntity>, EntityRepository<BookEntity>>();
    services.AddSingleton<ITableRepository, TableRepository>();

    // Add book related services
    services.AddSingleton<IBooksManager, BooksManager>();
    services.AddSingleton<IBooksTableManager, BooksTableManager>();

    // Add consoles
    services.AddSingleton<DashboardConsole>();
    services.AddSingleton<TableManagerConsole>();
    services.AddSingleton<BookManagerConsole>();

    return services;
}
```

### Manage Tables

![dynamodb-table-manager](https://user-images.githubusercontent.com/33935506/114666725-b1280080-9d52-11eb-8d78-98290b1cc3bb.png)

I created a *DynamoDB* adaptor called `TableRepository`. It's used to facillitate the typical *DynamoDB* CRUD behavior associated with managing a **Table**.

```csharp
// FILE: TableRepository.cs

public sealed class TableRepository : ITableRepository
{
    private readonly IAmazonDynamoDB _dynamoDb;

    public TableRepository(IAmazonDynamoDB amazonDb)
    {
        _dynamoDb = amazonDb ?? throw new ArgumentNullException(nameof(amazonDb));
    }

    public Task CreateTableAsync(
        string tableName,
        IReadOnlyList<KeySchemaElement> keySchema,
        IReadOnlyList<AttributeDefinition> attributes,
        ProvisionedThroughput provisionedThroughput)
    {
        if (string.IsNullOrWhiteSpace(tableName))
            throw new ArgumentException("Table name is required.", nameof(tableName));

        return createTableAsync();

        async Task createTableAsync()
        {
            if (!await IsExistingTableAsync(tableName))
            {
                var request = new CreateTableRequest
                {
                    TableName = tableName,
                    AttributeDefinitions = attributes.ToList(),
                    KeySchema = keySchema.ToList(),
                    ProvisionedThroughput = provisionedThroughput
                };

                await _dynamoDb.CreateTableAsync(request);
            }
        }
    }

    public Task CreateTableAndWaitUntilTableReadyAsync(
        string tableName,
        IReadOnlyList<KeySchemaElement> keySchema,
        IReadOnlyList<AttributeDefinition> attributes,
        ProvisionedThroughput provisionedThroughput)
    {
        if (string.IsNullOrWhiteSpace(tableName))
            throw new ArgumentException($"Table name is required.", nameof(tableName));

        return createTableAndWaitAsync();

        async Task createTableAndWaitAsync()
        {
            await CreateTableAsync(tableName, keySchema, attributes, provisionedThroughput);
            await WaitUntilTableReadyAsync(tableName);
        }
    }

    /// <summary>
    /// Check every 5 seconds for 1 minute until table creation completed.
    /// </summary>
    public Task WaitUntilTableReadyAsync(string tableName)
    {
        if (string.IsNullOrWhiteSpace(tableName))
            throw new ArgumentException($"Table name is required.", nameof(tableName));

        return waitUntilTableReadyAsync();

        async Task waitUntilTableReadyAsync()
        {
            TableStatus? status = null;
            TimeSpan maxWaitTime = TimeSpan.FromMinutes(1);

            do
            {
                await Task.Delay(3000);
                maxWaitTime = maxWaitTime.Subtract(TimeSpan.FromSeconds(5));

                try
                {
                    var response = await _dynamoDb.DescribeTableAsync(new DescribeTableRequest
                    {
                        TableName = tableName
                    });

                    Console.WriteLine($"Table name: {response.Table.TableName}, status: {response.Table.TableStatus}");
                    status = response.Table.TableStatus;
                }
                catch (ResourceNotFoundException)
                {
                    // The table may not be created yet. This operation is eventually consistent
                    // and for now we just ignore the exception
                    Console.WriteLine($"Table '{tableName}' not yet created.");
                }
            } while (status != TableStatus.ACTIVE || maxWaitTime.TotalSeconds <= 0);
        }
    }

    public Task DeleteTableAsync(string tableName)
    {
        if (string.IsNullOrWhiteSpace(tableName))
            throw new ArgumentException($"Table name is required.", nameof(tableName));

        return deleteTableAsync();

        async Task deleteTableAsync()
        {
            if (!await ExistsAsync(tableName))
                throw new TableNotFoundException($"A table having name '{tableName}' does not exist.");

            await _dynamoDb.DeleteTableAsync(tableName);
        }
    }

    public Task<bool> IsExistingTableAsync(string tableName)
    {
        if (string.IsNullOrWhiteSpace(tableName))
            throw new ArgumentException($"Table name is required.", nameof(tableName));

        return ExistsAsync(tableName);
    }

    private async Task<bool> ExistsAsync(string tableName)
    {
        return (await GetTableNameListAsync()).Any(tn => string.Compare(tn, tableName, true) == 0);
    }

    private async Task<IReadOnlyList<string>> FindTableNameMatchesAsync(params string[] tableNames)
    {
        var existingTableNames = await GetTableNameListAsync();
        return existingTableNames.Intersect(tableNames).ToList();
    }

    public Task<TableDescription?> DescribeTableAsync(string tableName)
    {
        if (string.IsNullOrWhiteSpace(tableName))
            throw new ArgumentException($"Table name is required.", nameof(tableName));

        return getTableAsync();

        async Task<TableDescription?> getTableAsync()
        {
            if (!await ExistsAsync(tableName))
                return null;

            return (await _dynamoDb.DescribeTableAsync(tableName)).Table;
        }
    }

    public Task<IReadOnlyList<TableDescription>> DescribeTablesAsync(params string[] tableNames)
    {
        if (tableNames == null || !tableNames.Any())
            throw new ArgumentException($"A list of table names is required.", nameof(tableNames));

        return getTableAsync();

        async Task<IReadOnlyList<TableDescription>> getTableAsync()
        {
            var matchingTableNames = await FindTableNameMatchesAsync(tableNames);

            if (!matchingTableNames.Any())
                return Enumerable.Empty<TableDescription>().ToList();

            var tableDescriptions = new List<TableDescription>(matchingTableNames.Count);

            foreach (var tableName in matchingTableNames)
                tableDescriptions.Add((await _dynamoDb.DescribeTableAsync(tableName)).Table);

            return tableDescriptions;
        }
    }

    public async Task<IReadOnlyList<string>> GetTableNameListAsync()
    {
        var tableNames = new List<string>();
        string? startTableName = null;

        do
        {
            var response = await _dynamoDb.ListTablesAsync(new ListTablesRequest
            {
                ExclusiveStartTableName = startTableName,
                Limit = 10
            });

            tableNames.AddRange(response.TableNames);
            startTableName = response.LastEvaluatedTableName;

        } while (!string.IsNullOrWhiteSpace(startTableName));

        return tableNames;
    }

}
```

#### Create Table

![dynamodb-2-create-books](https://user-images.githubusercontent.com/33935506/114658262-a31fb300-9d45-11eb-88fc-f6a13937ddf9.png)

```csharp
// FILE: TableRepository.cs

public Task CreateTableAsync(
    string tableName,
    IReadOnlyList<KeySchemaElement> keySchema,
    IReadOnlyList<AttributeDefinition> attributes,
    ProvisionedThroughput provisionedThroughput)
{
    if (string.IsNullOrWhiteSpace(tableName))
        throw new ArgumentException($"Table name is required.", nameof(tableName));

    return createTableAsync();

    async Task createTableAsync()
    {
        if (!await IsExistingTableAsync(tableName))
        {
            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = attributes.ToList(),
                KeySchema = keySchema.ToList(),
                ProvisionedThroughput = provisionedThroughput
            };

            await _dynamoDb.CreateTableAsync(request);
        }
    }
}
```

#### List Tables

![dynamodb-3-list-tables](https://user-images.githubusercontent.com/33935506/114658264-a3b84980-9d45-11eb-866e-77aeb99a664b.png)

```csharp
// FILE: TableRepository.cs

public async Task<IReadOnlyList<string>> GetTableNameListAsync()
{
    var tableNames = new List<string>();
    string? startTableName = null;

    do
    {
        var response = await _dynamoDb.ListTablesAsync(new ListTablesRequest
        {
            ExclusiveStartTableName = startTableName,
            Limit = 10
        });

        tableNames.AddRange(response.TableNames);
        startTableName = response.LastEvaluatedTableName;

    } while (!string.IsNullOrWhiteSpace(startTableName));

    return tableNames;
}
```

#### Describe Table

![dynamodb-4-describe-table](https://user-images.githubusercontent.com/33935506/114658265-a450e000-9d45-11eb-9378-6a518e5a9a4e.png)

```csharp
// FILE: TableRepository.cs

public Task<IReadOnlyList<TableDescription>> DescribeTablesAsync(params string[] tableNames)
{
    if (tableNames == null || !tableNames.Any())
        throw new ArgumentException($"A list of table names is required.", nameof(tableNames));

    return getTableAsync();

    async Task<IReadOnlyList<TableDescription>> getTableAsync()
    {
        var matchingTableNames = await FindTableNameMatchesAsync(tableNames);

        if (!matchingTableNames.Any())
            return Enumerable.Empty<TableDescription>().ToList();

        var tableDescriptions = new List<TableDescription>(matchingTableNames.Count);

        foreach (var tableName in matchingTableNames)
            tableDescriptions.Add((await _dynamoDb.DescribeTableAsync(tableName)).Table);

        return tableDescriptions;
    }
}
```

#### Delete Table

```csharp
// FILE: TableRepository.cs

public Task DeleteTableAsync(string tableName)
{
    if (string.IsNullOrWhiteSpace(tableName))
        throw new ArgumentException($"Table name is required.", nameof(tableName));

    return deleteTableAsync();

    async Task deleteTableAsync()
    {
        if (!await ExistsAsync(tableName))
            throw new TableNotFoundException($"A table having name '{tableName}' does not exist.");

        await _dynamoDb.DeleteTableAsync(tableName);
    }
}

private async Task<bool> ExistsAsync(string tableName)
{
    return (await GetTableNameListAsync()).Any(tn => string.Compare(tn, tableName, true) == 0);
}

public async Task<IReadOnlyList<string>> GetTableNameListAsync()
{
    var tableNames = new List<string>();
    string? startTableName = null;

    do
    {
        var response = await _dynamoDb.ListTablesAsync(new ListTablesRequest
        {
            ExclusiveStartTableName = startTableName,
            Limit = 10
        });

        tableNames.AddRange(response.TableNames);
        startTableName = response.LastEvaluatedTableName;

    } while (!string.IsNullOrWhiteSpace(startTableName));

    return tableNames;
}
```

### Manage Book Table

![dynamodb-book-manager](https://user-images.githubusercontent.com/33935506/114666723-b08f6a00-9d52-11eb-849d-4c170c20767f.png)

I created a *DynamoDB* adaptor called `EntityRepository`. It's used to facillitate the typical *DynamoDB* CRUD behavior used to manage an **Entity** (Book in this instance).

```csharp
// FILE: EntityRepository.cs

public sealed class EntityRepository<T> : IEntityRepository<T> where T : class
{
    private readonly IAmazonDynamoDB _dynamoDb;

    public EntityRepository(IAmazonDynamoDB dynamoDb)
    {
        _dynamoDb = dynamoDb ?? throw new ArgumentNullException(nameof(dynamoDb));
    }

    public async Task DeleteAsync(object hashKey)
    {
        using (var context = new DynamoDBContext(_dynamoDb))
        {
            await context.DeleteAsync<T>(hashKey: hashKey);
        }
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        using (var context = new DynamoDBContext(_dynamoDb))
        {                
            return await context
                .ScanAsync<T>(Enumerable.Empty<ScanCondition>())
                .GetRemainingAsync()
                ?? Enumerable.Empty<T>().ToList();
        }
    }

    public async Task<T> GetAsync(object hashKey)
    {
        using (var context = new DynamoDBContext(_dynamoDb))
        {
            return await context.LoadAsync<T>(hashKey);
        }
    }

    public Task SaveAsync(T entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        return saveEntityAsync();

        async Task saveEntityAsync()
        {
            using (var context = new DynamoDBContext(_dynamoDb))
            {
                await context.SaveAsync(entity);
            }
        }
    }
}
```

#### Add Book

![dynamodb-5-add-book](https://user-images.githubusercontent.com/33935506/114658268-a450e000-9d45-11eb-97bf-d3652784ce2e.png)

```csharp
// FILE: BooksManager

public Task AddBookAsync(BookForCreate bookForCreate)
{
    if (bookForCreate is null)
        throw new ArgumentNullException(nameof(bookForCreate));

    return addBookAsync();

    async Task addBookAsync()
    {
        var bookEntity = bookForCreate.ToBookEntity();
        bookEntity.Id = Guid.NewGuid().ToString();

        await _repository.SaveAsync(bookEntity);
    }
}
```

#### Update Book

```csharp
// FILE: BooksManager.cs

public Task UpdateBookAsync(Guid bookId, BookForUpdate bookForUpdate)
{
    if (bookForUpdate is null)
        throw new ArgumentNullException(nameof(bookForUpdate));

    return updateBookAsync();

    async Task updateBookAsync()
    {
        var book = await _repository.GetAsync(bookId.ToString());

        if (book == null)
            throw new InvalidOperationException($"A book having id '{bookId}' could not be found.");

        book.Title = bookForUpdate.Title;
        book.Description = bookForUpdate.Description;

        await _repository.SaveAsync(book);
    }
}

// FILE: EntityRepository.cs

public Task SaveAsync(T entity)
{
    if (entity is null)
        throw new ArgumentNullException(nameof(entity));

    return saveEntityAsync();

    async Task saveEntityAsync()
    {
        using (var context = new DynamoDBContext(_dynamoDb))
        {
            await context.SaveAsync(entity);
        }
    }
}
```

#### Delete Book

```csharp
// FILE: BooksManager.cs

public Task DeleteBookAsync(Guid bookId)
{
    return _repository.DeleteAsync(bookId.ToString());
}

// FILE: EntityRepository.cs

public async Task DeleteAsync(object hashKey)
{
    using (var context = new DynamoDBContext(_dynamoDb))
    {
        await context.DeleteAsync<T>(hashKey: hashKey);
    }
}
```

#### Find Book

![dynamodb-6-list-books](https://user-images.githubusercontent.com/33935506/114658270-a4e97680-9d45-11eb-92c7-fb9167906498.png)

```csharp
// FILE: BooksManager.cs

public async Task<Book> GetBookAsync(Guid bookId)
{
    var bookFromRepo = await _repository.GetAsync(bookId.ToString());
    return bookFromRepo.ToBook();
}

// FILE: EntityRepository.cs

public async Task<T> GetAsync(object hashKey)
{
    using (var context = new DynamoDBContext(_dynamoDb))
    {
        return await context.LoadAsync<T>(hashKey);
    }
}
```

---

## Notable Nuget Packages

* [AWSSDK.DynamoDBv2] - Amazon DynamoDB is a fast and flexible NoSQL database service for all applications that need consistent, single-digit millisecond latency at any scale.
* [AWSSDK.Extensions.NETCore.Setup] - Extensions for the AWS SDK for .NET to integrate with .NET Core configuration and dependency injection frameworks.

---

[AWSSDK.DynamoDBv2]: https://www.nuget.org/packages/AWSSDK.DynamoDBv2/
[AWSSDK.Extensions.NETCore.Setup]: https://www.nuget.org/packages/AWSSDK.Extensions.NETCore.Setup/