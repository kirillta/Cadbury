using Microsoft.Extensions.Logging;

namespace Floxdc.Cadbury
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            
            var core = new Core(loggerFactory);
            core.Run(args).Wait();
        }
    }
}
