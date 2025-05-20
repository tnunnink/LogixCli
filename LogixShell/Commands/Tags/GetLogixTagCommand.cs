using System.Management.Automation;
using JetBrains.Annotations;
using L5Sharp.Core;

namespace LogixShell;

/// <summary>
/// Represents a PowerShell cmdlet for retrieving tags from a Logix project,
/// with options to filter based on tag name, scope, or data type.
/// </summary>
[PublicAPI]
[Cmdlet(VerbsCommon.Get, "LogixTag")]
[OutputType(typeof(Tag))]
public class GetLogixTagCommand : Cmdlet
{
    #region Parameters

    /// <summary>
    /// Gets or sets the collection of Logix projects to be processed.
    /// This property accepts input from the pipeline and is used to retrieve and filter tags from the provided projects.
    /// </summary>
    [Parameter(Position = 0, ValueFromPipeline = true)]
    public L5X[] Project { get; set; } = [];

    /// <summary>
    /// Gets or sets the name parameter used to filter tags within the Logix project.
    /// This property supports wildcard characters for pattern matching and allows case-insensitive filtering of tag names.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the scope of the tags to be retrieved from the Logix project.
    /// This property allows filtering of tags based on their program-level scope, enabling targeted retrieval.
    /// </summary>
    [Parameter]
    public string? Scope { get; set; }

    /// <summary>
    /// Gets or sets the data type filter for the tags to be retrieved.
    /// This property allows specifying a data type to narrow down the query results
    /// by matching tags with the corresponding data type in the provided projects.
    /// </summary>
    [Parameter]
    public string? DataType { get; set; }

    /// <summary>
    /// Gets or sets a switch determining whether to recursively process child tag members
    /// when analyzing or filtering tags within a Logix project file.
    /// When enabled, nested tags or members are included in the operation.
    /// </summary>
    [Parameter]
    public SwitchParameter Recurse { get; set; }

    #endregion


    /// <inheritdoc />
    protected override void ProcessRecord()
    {
        foreach (var project in Project)
        {
            ProcessProject(project);
        }
    }

    /// <summary>
    /// Processes the provided Logix project to filter and output tags based on specified Name, Scope, or DataType criteria.
    /// </summary>
    /// <param name="project">The Logix project to be processed, represented as an L5X object.</param>
    private void ProcessProject(L5X project)
    {
        var tags = Recurse
            ? project.Query<Tag>().SelectMany(t => t.Members())
            : project.Query<Tag>();

        tags = tags
            .TryFilterText(x => x.TagName, Name)
            .TryFilterText(x => x.Scope.Program, Scope)
            .TryFilterText(x => x.DataType, DataType);

        foreach (var tag in tags)
        {
            WriteObject(tag);
        }
    }
}