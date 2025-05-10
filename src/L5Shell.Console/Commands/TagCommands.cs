using Cocona;
using JetBrains.Annotations;
using L5Sharp.Core;
using L5Shell.Console.Options;
using L5Shell.Console.Services;
using Spectre.Console;

namespace L5Shell.Console.Commands;

[PublicAPI]
public class TagCommands(IProjectManager manager, IAnsiConsole console)
{
    [Command("get", Description = "Gets a Tag with the specified name.")]
    public void GetTag([Argument(Description = "Specifies the name of the tag to get.")] string name)
    {
        if (!manager.TryGetProject(out var project)) return;
        var component = project.Get<Tag>(name);
        console.Write(component.Serialize().ToString());
    }

    [Command("add", Description = "Adds a tag to the specified L5X file.")]
    public void AddTag(
        [Argument(Description = "Specifies the name of the tag.")]
        string name,
        [Argument(Description = "Specifies the data type of the tag.")]
        string dataType,
        [Option(Description = "Specifies the scope name of the tag.")]
        string? scope = default,
        [Option(Description = "Specifies the description of the tag.")]
        string? desc = default
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

    [Command("list", Description = "Lists")]
    public void ListTags(
        TagOptions options,
        [Option] int depth = 0,
        [Option] int count = 100,
        [Option] string? format = default)
    {
        if (!manager.TryGetProject(out var project)) return;

        var tags = project.Query<Tag>()
            .SelectMany(t => t.Members())
            .Where(t => t.TagName.Depth <= depth)
            .Where(options.Filter)
            .Take(count)
            .ToList();

        var table = GenerateTable(tags);
        console.Write(table);
    }

    [Command("import", Description = "Imports tags into the specified scope of the staged L5X.")]
    public void ImportTags(string importFile)
    {
    }

    private static Table GenerateTable(IEnumerable<Tag> tags)
    {
        var table = new Table()
            .AddColumn("TagName")
            .AddColumn("Scope")
            .AddColumn("DataType")
            .AddColumn("Value")
            .AddColumn("Description");
        
        foreach (var tag in tags)
            table.AddRow(tag.TagName.ToString().EscapeMarkup(),
                tag.Container.EscapeMarkup(),
                tag.DataType.EscapeMarkup(),
                tag.Value.ToString().EscapeMarkup(),
                tag.Description.EscapeMarkup()
            );

        return table;
    }
}