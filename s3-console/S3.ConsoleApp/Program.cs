using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using S3.ConsoleApp.Services;

namespace S3.ConsoleApp
{
    internal sealed class Program
    {
        private Program()
        {
        }

        internal async static Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json")
                .Build();

            var services = new ServiceCollection();
            services.AddLogging(configure => configure.AddConsole());
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
            services.AddTransient<IS3Service, S3Service>();

            var provider = services.BuildServiceProvider();
            var client = provider.GetRequiredService<IS3Service>();

            var defaultForegroundColor = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"    ___        ______    ____ _____   ____                       ");
            Console.WriteLine(@"   / \ \      / / ___|  / ___|___ /  |  _ \  ___ _ __ ___   ___  ");
            Console.WriteLine(@"  / _ \ \ /\ / /\___ \  \___ \ |_ \  | | | |/ _ \ '_ ` _ \ / _ \ ");
            Console.WriteLine(@" / ___ \ V  V /  ___) |  ___) |__) | | |_| |  __/ | | | | | (_) |");
            Console.WriteLine(@"/_/   \_\_/\_/  |____/  |____/____/  |____/ \___|_| |_| |_|\___/ ");
            Console.ForegroundColor = defaultForegroundColor;

            do
            {
                Console.WriteLine();
                Console.WriteLine(getOptions());
                var selection = Console.ReadLine();

                if (selection == "q")
                {
                    break;
                }
                else if (selection == "ls")
                {
                    var buckets = await client.ListAllBucketsAsync();
                    if (buckets != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(JsonConvert.SerializeObject(buckets, Formatting.Indented));
                    }
                }
                else if (selection == "mb")
                {
                    Console.Write("Enter bucket name: ");
                    var bucketName = Console.ReadLine();
                    var createdBucket = await client.CreateBucketAsync(bucketName);
                    if (createdBucket != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine(JsonConvert.SerializeObject(createdBucket, Formatting.Indented));
                    }
                }
                else if (selection == "rb")
                {
                    Console.Write("Enter bucket name: ");
                    var bucketName = Console.ReadLine();
                    var deletedBucket = await client.DeleteBucketAsync(bucketName);
                    if (deletedBucket != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(JsonConvert.SerializeObject(deletedBucket, Formatting.Indented));
                    }
                }

                Console.ForegroundColor = defaultForegroundColor;

            } while (true);

            string getOptions()
            {
                var options = new StringBuilder();
                options.AppendLine("q - Quit");
                options.AppendLine("ls - List all buckets");
                options.AppendLine("mb - Create bucket");
                options.AppendLine("rb - Delete bucket");
                return options.ToString();
            }

            Console.WriteLine("Done!");
        }
    }
}
