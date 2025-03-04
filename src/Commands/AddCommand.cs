namespace GrokCLI.Commands;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

public class  AddCommand : ICommand
{
    // Path to store staging state
    private static readonly string StageFilePath = "staged_files.json";

    public Task Execute(string? parameter = null)
    {
        if (string.IsNullOrEmpty(parameter))
        {
            Console.WriteLine("command: add <file>");
            return;
        }


        switch (command)
        {
            case "add":
                if (args.Length < 2)
                {
                    Console.WriteLine("Usage: <program> add <file>");
                    return;
                }
                AddFile(args[1]);
                break;

            case "status":
                ShowStatus();
                break;

            default:
                Console.WriteLine($"Unknown command: {command}");
                Console.WriteLine("Available commands: add, status");
                break;
        }
    }

    static void AddFile(string filePath)
    {
        // Check if file exists
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Error: File '{filePath}' does not exist");
            return;
        }

        // Load existing staged files
        List<string> stagedFiles = LoadStagedFiles();

        // Add file if not already staged
        string fullPath = Path.GetFullPath(filePath);
        if (!stagedFiles.Contains(fullPath))
        {
            stagedFiles.Add(fullPath);
            SaveStagedFiles(stagedFiles);
            Console.WriteLine($"Added '{fullPath}' to staging area");
        }
        else
        {
            Console.WriteLine($"'{fullPath}' is already staged");
        }
    }

    static void ShowStatus()
    {
        List<string> stagedFiles = LoadStagedFiles();

        if (stagedFiles.Count == 0)
        {
            Console.WriteLine("No files staged");
        }
        else
        {
            Console.WriteLine("Staged files:");
            foreach (string file in stagedFiles)
            {
                Console.WriteLine($"  {file}");
            }
        }
    }

    static List<string> LoadStagedFiles()
    {
        try
        {
            if (File.Exists(StageFilePath))
            {
                string json = File.ReadAllText(StageFilePath);
                return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading staged files: {ex.Message}");
        }
        return new List<string>();
    }

    static void SaveStagedFiles(List<string> files)
    {
        try
        {
            string json = JsonSerializer.Serialize(files, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(StageFilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving staged files: {ex.Message}");
        }
    }


}