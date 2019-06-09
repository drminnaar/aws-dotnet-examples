using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Cognito.MvcApp
{
    internal sealed class Program
    {
        private Program()
        {
        }

        internal static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        internal static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
        }
    }
}
