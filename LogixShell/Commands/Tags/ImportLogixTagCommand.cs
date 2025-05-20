using System.Management.Automation;
using JetBrains.Annotations;
using L5Sharp.Core;

namespace LogixShell;

[PublicAPI]
[Cmdlet(VerbsData.Import, "LogixTag")]
[OutputType(typeof(Tag))]
public class ImportLogixTagCommand : Cmdlet
{
    #region Parameters

    /// <summary>
    /// Gets or sets the file path(s) to the logix project files.
    /// Used to locate and read the content for command processing.
    /// Accepts multiple paths and supports pipeline input for convenience.
    /// </summary>
    [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
    [Alias("FullName", "Path")]
    public string[] FilePath { get; set; } = [];

    /// <summary>
    /// Gets or sets the name of the tag to read. If specified, only tags matching this name will be returned.
    /// Supports wildcard characters for pattern matching.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the scope filter for the tags. If specified, only tags within this scope will be returned.
    /// This allows filtering tags by their program or controller scope.
    /// </summary>
    [Parameter]
    public string? Scope { get; set; }

    /// <summary>
    /// Gets or sets the data type filter for the tags. If specified, only tags of this data type will be returned.
    /// This allows filtering tags by their specific data type (e.g., BOOL, DINT, REAL).
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

    #region Internal

    /// <inheritdoc />
    protected override void ProcessRecord()
    {
        foreach (var path in FilePath)
        {
            ProcessFile(path);
        }
    }

    /// <summary>
    /// Processes the specified file path to load and filter Logix program tags based on provided parameters.
    /// </summary>
    /// <param name="path">The full file path of the Logix project to be processed.</param>
    private void ProcessFile(string path)
    {
        try
        {
            if (!File.Exists(path))
            {
                WriteError(Error.FileNotFound(path));
                return;
            }

            var project = L5X.Load(path);

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
        catch (Exception e)
        {
            WriteError(new ErrorRecord(e, "TagReadError", ErrorCategory.ReadError, path));
        }
    }

    #endregion
}