using Cocona;
using JetBrains.Annotations;
using L5Sharp.Core;
using LogixCli.Services;
using Spectre.Console;

namespace LogixCli.Commands;

[PublicAPI]
public class GetCommands(IProjectManager manager, IAnsiConsole console)
{
    [Command("get", Description = "Gets a Tag with the specified name.")]
    public void GetTag([Argument(Description = "Specifies the name of the tag to get.")] string name)
    {
        if (!manager.TryGetProject(out var project)) return;
        var component = project.Get<Tag>(name);
        console.Write(component.Serialize().ToString());
    }
}