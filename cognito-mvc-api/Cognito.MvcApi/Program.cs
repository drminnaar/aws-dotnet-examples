using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Cognito.MvcApi
{
    internal class Program
    {
        private Program()
        {            
        }

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
        }
    }
}
