using Cocona;
using JetBrains.Annotations;
using L5Sharp.Core;
using L5Shell.Console.Services;
using Spectre.Console;

namespace L5Shell.Console.Commands;

[PublicAPI]
public class GetCommands(IProjectManager manager)
{
    [Command("datatype", Description = "Gets a DataType with the specified name.")]
    public void DataType([Argument(Description = "Specifies the name of the tag to get.")] string name)
    {
        if (!manager.TryGetProject(out var project)) return;
        var component = project.Get<DataType>(name);
        AnsiConsole.Write(component.Serialize().ToString());
    }

    [Command("tag", Description = "Gets a Tag with the specified name.")]
    public void Tag([Argument(Description = "Specifies the name of the tag to get.")] string name)
    {
        if (!manager.TryGetProject(out var project)) return;
        var component = project.Get<Tag>(name);
        AnsiConsole.Write(component.Serialize().ToString());
    }
}