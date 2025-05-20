using System.Management.Automation;
using JetBrains.Annotations;
using L5Sharp.Core;

namespace LogixShell;

/// <summary>
/// Creates a new Logix tag with specified properties.
/// </summary>
/// <remarks>
/// Creates tags with specified name and data type. Supports atomic, array, and complex types.
/// </remarks>
/// <example>
/// <code>
/// New-LogixTag -Name "Motor1_Running" -DataType "BOOL"
/// New-LogixTag -Name "Counters" -DataType "DINT" -Dimensions 10
/// Import-Csv tags.csv | New-LogixTag
/// </code>
/// </example>
[PublicAPI]
[Cmdlet(VerbsCommon.New, "LogixTag")]
[OutputType(typeof(Tag))]
public class NewLogixTagCommand : Cmdlet
{
    #region Parameters

    /// <summary>
    /// Gets or sets the name of the Logix tag to create.
    /// </summary>
    [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
    [Alias("TagName")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the data type of the tag to create.
    /// </summary>
    [Parameter(Position = 1, Mandatory = true, ValueFromPipelineByPropertyName = true)]
    [Alias("Type")]
    public string DataType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description for the tag.
    /// </summary>
    [Parameter]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the dimensions for array tags.
    /// </summary>
    /// <remarks>
    /// For one-dimensional arrays, provide a single value. For multi-dimensional arrays, provide multiple values.
    /// </remarks>
    [Parameter]
    public string[] Dimensions { get; set; } = [];

    /// <summary>
    /// Gets or sets the initial value for the tag.
    /// </summary>
    /// <remarks>
    /// Must be compatible with the tag's data type. If not specified, uses default value.
    /// </remarks>
    [Parameter]
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the external access level for the tag.
    /// </summary>
    /// <remarks>
    /// Options: ReadWrite (default), ReadOnly, None
    /// </remarks>
    [Parameter]
    [Alias("Access")]
    public ExternalAccess ExternalAccess { get; set; } = ExternalAccess.ReadWrite;

    /// <summary>
    /// Gets or sets whether the tag is constant (cannot be modified at runtime).
    /// </summary>
    /// <remarks>
    /// When true, tag value is read-only during program execution.
    /// </remarks>
    [Parameter]
    public SwitchParameter Constant { get; set; }

    #endregion

    /// <summary>
    /// Processes each pipeline object and creates a new Logix tag.
    /// </summary>
    /// <inheritdoc />
    protected override void ProcessRecord()
    {
        try
        {
            var tag = new Tag(Name, DataType, Description)
            {
                ExternalAccess = ExternalAccess,
                Constant = Constant
            };

            WriteObject(tag);
        }
        catch (Exception ex)
        {
            WriteError(new ErrorRecord(ex, "TagCreationError", ErrorCategory.InvalidOperation, Name));
        }
    }
}