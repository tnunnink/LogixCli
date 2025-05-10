using L5Shell.Console.Commands;
using L5Shell.Console.Options;
using L5Shell.Console.Services;
using Spectre.Console.Testing;
using Task = System.Threading.Tasks.Task;

namespace L5Shell.Tests;

[TestFixture]
public class TagCommandsTests
{
    [Test]
    public Task List_Defaults_ShouldHaveVerifiedOutput()
    {
        var console = new TestConsole();
        var manager = new ProjectManager(console);
        var commands = new TagCommands(manager, console);

        commands.ListTags(new TagOptions());

        return Verify(console.Output);
    }
}