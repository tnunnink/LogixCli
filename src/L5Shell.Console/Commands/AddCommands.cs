using Cocona;
using JetBrains.Annotations;
using L5Sharp.Core;
using L5Shell.Console.Services;

namespace L5Shell.Console.Commands;

[PublicAPI]
public class AddCommands(IProjectManager manager /*, ILogger<AddCommands> logger*/)
{
    [Command("add-tag", Description = "Adds a tag to the specified L5X file.")]
    public void AddTag(
        [Argument(Description = "Specifies the name of the tag.")]
        string name,
        [Argument(Description = "Specifies the data type of the tag.")]
        string dataType,
        [Option('s', Description = "Specifies the scope name of the tag.")]
        string? scope = default,
        [Option('d', Description = "Specifies the description of the tag.")]
        string? desc = default
    )
    {
        if (!manager.TryGetProject(out var project)) return;

        var tag = new Tag(name, dataType, desc);

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