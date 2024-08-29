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
    var settings = new AnsiConsoleSettings { };
    return AnsiConsole.Create(settings);
});
builder.Logging.AddConsole();

var app = builder.Build();

app.AddCommands<ProjectCommands>();
app.AddSubCommand("get", b => b.AddCommands<GetCommands>())
    .WithDescription("Commands for getting component data from an L5X file.");

app.Run();