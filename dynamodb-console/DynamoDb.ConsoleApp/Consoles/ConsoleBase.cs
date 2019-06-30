using System;
using System.Threading.Tasks;

namespace DynamoDb.ConsoleApp.Consoles
{
    public abstract class ConsoleBase
    {
        protected ConsoleBase()
        {            
        }

        protected static async Task ExecuteMenuActionAsync(Func<Task> menuAction)
        {
            try
            {
                await menuAction();
            }
            catch (Exception e)
            {
                var foregroundColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                Console.ForegroundColor = foregroundColor;
            }
        }

        protected static string Prompt(string question)
        {
            Console.Write(question);
            return Console.ReadLine();
        }
    }
}
