using System.Management.Automation;
using JetBrains.Annotations;
using L5Sharp.Core;

namespace LogixShell;

[PublicAPI]
[Cmdlet(VerbsData.Import, "LogixProject")]
[OutputType(typeof(L5X))]
public class ImportLogixProjectCommand : Cmdlet
{
    /// <summary>
    /// Gets or sets the file path(s) to the logix project files.
    /// Used to locate and import the content for command processing.
    /// Accepts multiple paths and supports pipeline input for convenience.
    /// </summary>
    [Parameter(Position = 0, Mandatory = true, ValueFromPipeline = true)]
    [Alias("FullName", "Path")]
    public string[] FilePath { get; set; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether to enable indexing when importing the Logix project file(s).
    /// Indexing organizes the project elements for optimized searching and performance.
    /// </summary>
    public bool Index { get; set; }


    protected override void ProcessRecord()
    {
        foreach (var path in FilePath)
        {
            ProcessFile(path);
        }
    }

    /// <summary>
    /// Processes the specified file path by validating and loading the file into a Logix project.
    /// </summary>
    /// <param name="filePath">The full path of the file to be processed.</param>
    private void ProcessFile(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                WriteError(Error.FileNotFound(filePath));
                return;
            }

            var option = Index ? L5XOptions.Index : L5XOptions.None;
            var project = L5X.Load(filePath, option);

            WriteObject(project);
        }
        catch (Exception e)
        {
            WriteError(new ErrorRecord(e, "ProjectLoadError", ErrorCategory.OpenError, FilePath));
        }
    }
}