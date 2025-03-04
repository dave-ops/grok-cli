namespace GrokCLI.Services;

public interface IService
{
    Task<byte[]> Execute(string prompt);

}
