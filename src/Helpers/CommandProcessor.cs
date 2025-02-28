using GrokCLI.Helpers;

namespace GrokCLI
{
    public class CommandProcessor
    {
        public void ParseArgs(string[] args)
        {
            Logger.Info("Args: " + (args?.Length > 0 ? string.Join(", ", args) : "No arguments provided"));

            if (args == null || args.Length == 0)
            {
                Logger.Error("No command provided.");
                return;
            }

            string command = args[0].ToLowerInvariant(); // Safely access first argument

            switch (command)
            {
                case "ratelimit":
                    // Handle the ratelimit command
                    var rateLimitService = new GetRateLimitService();
                    rateLimitService.Execute().Wait(); // Or use async/await properly in a real app
                    break;

                // Add other commands as needed
                default:
                    Logger.Error($"Unknown command: {command}");
                    break;
            }
        }

        public void ProcessArgs(string[] args)
        {
            try
            {
                ParseArgs(args);
            }
            catch (Exception ex)
            {
                Logger.Error($"Error processing arguments: {ex.Message}");
                throw;
            }
        }
    }
}