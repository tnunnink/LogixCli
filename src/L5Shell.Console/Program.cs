using Cocona;
using L5Shell.Console.Commands;
using L5Shell.Console.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console;

var builder = CoconaApp.CreateBuilder();

builder.Services.AddSingleton<IProjectManager, ProjectManager>();
builder.Services.AddSingleton<IAnsiConsole>(_ =>
{
    var settings = new AnsiConsoleSettings {  };
    return AnsiConsole.Create(settings);
});
builder.Logging.AddConsole();

var app = builder.Build();

app.AddCommands<ProjectCommands>();

app.AddSubCommand("tag", b => b.AddCommands<TagCommands>())
    .WithDescription("Commands for processing tag components in the staged L5X.");

app.Run();