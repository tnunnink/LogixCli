using LogixCli.Commands;
using LogixCli.Services;
using Spectre.Console.Testing;
using Task = System.Threading.Tasks.Task;

namespace LogixCli.Tests;

[TestFixture]
public class ListCommandsTests
{
    [Test]
    public Task Tags_Defaults_ShouldHaveVerifiedOutput()
    {
        var console = new TestConsole();
        var manager = new ProjectManager(console);
        var list = new ListCommands(manager, console);

        list.Tags();

        return Verify(console.Output);
    }
}