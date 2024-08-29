using Cocona;
using JetBrains.Annotations;
using L5Shell.Console.Services;
using Spectre.Console;

namespace L5Shell.Console.Commands;

[PublicAPI]
public class ProjectCommands(IProjectManager manager, IAnsiConsole console)
{
    [Command("load", Description = "Loads L5X to the staging directory to be processed by commands.")]
    public void Load([Argument(Description = "The path to the L5X file to load.")] string file)
    {
        manager.LoadProject(file);
    }

    [Command("save", Description = "Saves the staged L5X to the specified output file.")]
    public void Save([Argument(Description = "The path to save the staged L5X file to.")] string file)
    {
        manager.SaveProject(file);
    }

    [Command("staged", Description = "Shows the current staged L5X file.")]
    public void Staged()
    {
        if (!manager.TryGetProject(out var project))
        {
            console.Markup("[red]No L5X file is currently staged for processing.[/]");
        }

        var info = project.Info;
        console.MarkupInterpolated($"[blue]{info.TargetName} is currently staged.[/]");
    }
}