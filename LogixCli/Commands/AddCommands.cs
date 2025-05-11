using Cocona;
using JetBrains.Annotations;
using L5Sharp.Core;
using LogixCli.Services;
using Spectre.Console;

namespace LogixCli.Commands;

[PublicAPI]
public class AddCommands(IProjectManager manager, IAnsiConsole console)
{
    [Command("add", Description = "Adds a tag to the specified L5X file.")]
    public void AddTag(
        [Argument(Description = "Specifies the name of the tag.")]
        string name,
        [Argument(Description = "Specifies the data type of the tag.")]
        string dataType,
        [Option(Description = "Specifies the scope name of the tag.")]
        string? scope = null,
        [Option(Description = "Specifies the description of the tag.")]
        string? desc = null
    )
    {
        if (!manager.TryGetProject(out var project)) return;

        var tag = new Tag(name, dataType);

        if (scope is not null)
        {
            project.Add(tag, scope);
        }
        else
        {
            project.Add(tag);
        }

        manager.SaveChanges(project);
    }
}