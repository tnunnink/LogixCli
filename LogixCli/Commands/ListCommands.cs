using Cocona;
using JetBrains.Annotations;
using L5Sharp.Core;
using LogixCli.Common;
using LogixCli.Services;
using Spectre.Console;

namespace LogixCli.Commands;

[PublicAPI]
public class ListCommands(IProjectManager manager, IAnsiConsole console)
{
    /// <summary>
    /// Lists all tags in the project or within a specific scope using the provided filters and output format.
    /// </summary>
    /// <param name="scope">Optional scope to filter tags (e.g., 'Program:MainProgram').</param>
    /// <param name="filter">Optional filter pattern to match tag names.</param>
    /// <param name="tagName">Filter by specific tag name.</param>
    /// <param name="dataType">Filter tags by their data type.</param>
    /// <param name="format">Output format for the list of tags. Options are: table, json, csv. Default is 'table'.</param>
    [Command("tags", Description = "Lists all tags in the project or within a specific scope.")]
    public void Tags(
        [Option('s', Description = "Optional scope to filter tags (e.g., 'Program:MainProgram')")]
        string? scope = null,
        [Option('n', Description = "Optional filter pattern to match tag names")]
        string? filter = null,
        [Option('t', Description = "Filter by data type")]
        string? tagName = null,
        [Option('f', Description = "Optional filter condition for various tag properties")]
        string? dataType = null,
        [Option("output", Description = "Output format: table, json, csv")]
        string format = "table")
    {
        if (!manager.TryGetProject(out var project)) return;

        var tags = project.Query<Tag>().SelectMany(t => t.Members());

        if (!string.IsNullOrEmpty(scope))
        {
            tags = tags.Where(t => t.Scope.Path.Like(scope));
        }

        if (!string.IsNullOrEmpty(tagName))
        {
            tags = tags.Where(t => t.TagName.ToString().Like(tagName));
        }

        if (!string.IsNullOrEmpty(dataType))
        {
            tags = tags.Where(t => t.DataType.ToString().Equals(dataType, StringComparison.OrdinalIgnoreCase));
        }

        switch (format.ToLower())
        {
            /*case "xml":
                OutputXml(tags);
                break;*/
            /*case "json":
                OutputJson(tags);
                break;*/
            /*case "csv":
                OutputCsv(tags);
                break;*/
            default:
                OutputTable(tags);
                break;
        }
    }

    private void OutputTable(IEnumerable<Tag> tags)
    {
        var table = new Table()
            .AddColumn("TagName")
            .AddColumn("Scope")
            .AddColumn("DataType")
            .AddColumn("Value")
            .AddColumn("Description");

        foreach (var tag in tags)
            table.AddRow(tag.TagName.ToString().EscapeMarkup(),
                tag.Scope.Container.EscapeMarkup(),
                tag.DataType.EscapeMarkup(),
                tag.Value.ToString().EscapeMarkup(),
                tag.Description.EscapeMarkup()
            );

        console.Write(table);
    }
}