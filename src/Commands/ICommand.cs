namespace GrokCLI.Helpers;

public interface ICommand
{
    Task Execute(string? parameter = null);
}