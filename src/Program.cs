using GrokCLI.Helpers;

namespace GrokCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Info("Starting grok...");
            var processor = new CommandProcessor();
            processor.ProcessArgs(args);
        }
    }
}