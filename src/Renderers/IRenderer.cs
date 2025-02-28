namespace GrokCLI.Renderers;

using System.Threading.Tasks;

public interface IRenderer
{
    Task Render(string jsonInput);
}