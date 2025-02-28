namespace GrokCLI.Commands;

public interface ICommand
{
    Task Execute(string? parameter = null);
}

