using CliFx;
using CliFx.Infrastructure;

namespace Noken.Assets.Tools;

public abstract class Command : ICommand
{
    protected readonly Config Config;

    public Command()
    {
        Config = Config.Read();
    }

    protected abstract ValueTask<int> Execute();

    public async ValueTask ExecuteAsync(IConsole console)
    {
        Environment.ExitCode = await Execute();
    }
}
