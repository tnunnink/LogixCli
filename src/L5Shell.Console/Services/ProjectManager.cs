using L5Sharp.Core;
using Spectre.Console;

namespace L5Shell.Console.Services;

public class ProjectManager(IAnsiConsole console) : IProjectManager
{
    private const Environment.SpecialFolder BaseFolder = Environment.SpecialFolder.ApplicationData;
    private const string SubFolder = "L5Shell";
    private static readonly string StagingPath = Path.Combine(Environment.GetFolderPath(BaseFolder), SubFolder);

    /// <inheritdoc />
    public void LoadProject(string file)
    {
        if (!SourceExists(file)) return;
        if (!SourceValid(file)) return;
        if (!EnsureStageExists()) return;
        if (!PrepareStage()) return;
        if (!StageFile(file)) return;
        console.WriteLine($"{file} successfully staged to working directory.");
    }

    /// <inheritdoc />
    public void SaveProject(string file)
    {
        if (!TryGetStagedFile(out var staged) || staged is null) return;

        try
        {
            if (File.Exists(file))
            {
                var answer = console.Ask<bool>($"{file} already exists. Overwrite the current file?");
                if (answer is false) return;
            }

            File.Move(staged.FullName, file, true);
        }
        catch (Exception e)
        {
            console.WriteException(e);
        }
    }

    /// <inheritdoc />
    public void SaveChanges(L5X project)
    {
        if (!TryGetStagedFile(out var staged) || staged is null) return;

        try
        {
            project.Save(staged.FullName);
        }
        catch (Exception e)
        {
            console.WriteException(e);
        }
    }

    /// <inheritdoc />
    public bool TryGetProject(out L5X project)
    {
        if (!TryGetStagedFile(out var staged) || staged is null)
        {
            project = null!;
            return false;
        }

        try
        {
            project = L5X.Load(staged.FullName);
            return true;
        }
        catch (Exception e)
        {
            console.WriteException(e);
            project = null!;
            return false;
        }
    }

    /// <summary>
    /// Attempts the get the stages file.
    /// If staging file or directory does not exist, will warn the user to call load first and return false.
    /// If the directory and single file exist then returns the file info.
    /// </summary>
    private bool TryGetStagedFile(out FileInfo? file)
    {
        var directory = new DirectoryInfo(StagingPath);

        if (!directory.Exists || directory.GetFiles().Length != 1)
        {
            console.Markup(
                "[red]No L5X is staged for processing. Run 'load {fileName}' to stage a file.[/]");
            file = null;
            return false;
        }

        file = directory.GetFiles()[0];
        return file.Exists;
    }

    /// <summary>
    /// Stages the file by copying it to the known application staging directory.
    /// </summary>
    private bool StageFile(string file)
    {
        var directory = new DirectoryInfo(StagingPath);
        var fileName = Path.GetFileName(file);
        var stagingFile = Path.Combine(directory.FullName, fileName);

        try
        {
            File.Copy(file, stagingFile);
            return true;
        }
        catch (Exception e)
        {
            console.WriteException(e);
            return false;
        }
    }

    /// <summary>
    /// Prepares the staging directory by ensuring no files exist. If any do, ask the user if they want to continue. 
    /// </summary>
    private bool PrepareStage()
    {
        var directory = new DirectoryInfo(StagingPath);
        var files = directory.GetFiles();

        if (files.Length > 0)
        {
            var prompt = new ConfirmationPrompt("An L5X is already staged. Do you want to replace it with new L5X file?");
            var answer = console.Prompt(prompt);
            if (answer is false) return false;
        }

        try
        {
            foreach (var file in files)
                file.Delete();

            return true;
        }
        catch (Exception e)
        {
            console.WriteException(e);
            return false;
        }
    }

    /// <summary>
    /// Ensures the staging directory exists in the local user app data
    /// </summary>
    private bool EnsureStageExists()
    {
        try
        {
            var info = Directory.CreateDirectory(StagingPath);
            return info.Exists;
        }
        catch (Exception e)
        {
            console.WriteException(e);
            return false;
        }
    }

    /// <summary>
    /// Attempts to load the L5X to ensure it is a valid XML file.
    /// </summary>
    private bool SourceValid(string file)
    {
        try
        {
            L5X.Load(file);
            return true;
        }
        catch (Exception e)
        {
            console.WriteException(e);
            return false;
        }
    }

    /// <summary>
    /// Determines if the specified file exists or not.
    /// </summary>
    private bool SourceExists(string file)
    {
        if (File.Exists(file)) return true;
        console.WriteLine($"'{file}' does not exist. Please provide a valid L5X file path.");
        return false;
    }
}