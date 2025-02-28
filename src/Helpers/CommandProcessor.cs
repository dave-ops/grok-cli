using System;
using System.Collections.Generic;
using System.IO;
using GrokCLI.Commands;
using GrokCLI.Utils;

namespace GrokCLI.Helpers
{
    public static class CommandProcessor
    {
        // Dictionary to map command names to their corresponding command types
        private static readonly Dictionary<string, Type> CommandTypes = new()
        {
            { GrokCommand.CommandName, typeof(GrokCommand) },
            { UploadCommand.CommandName, typeof(UploadCommand) },
            { RateLimitCommand.CommandName, typeof(RateLimitCommand) }
        };

        public static async Task ProcessArgs(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Logger.Info("No args provided, defaulting to 'grok' with default message");
                await ExecuteCommand(GrokCommand.CommandName, null);
                return;
            }

            string cmd = args[0].ToLowerInvariant();
            string? parameter = args.Length > 1 ? args[1] : null;

            await ExecuteCommand(cmd, parameter);
        }

        private static async Task ExecuteCommand(string cmd, string? parameter)
        {
            if (CommandTypes.TryGetValue(cmd, out Type? commandType))
            {
                try
                {
                    var command = (ICommand)Activator.CreateInstance(commandType)!;
                    await command.Execute(parameter);
                }
                catch (Exception ex)
                {
                    Logger.Error($"Error executing command {cmd}: {ex.Message}");
                }
            }
            else
            {
                Logger.Error($"Unknown command: {cmd}");
            }
        }
    }
}