using Cocona;
using LogixCli.Commands;
using LogixCli.Services;
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

app.AddSubCommand("list", b => b.AddCommands<ListCommands>())
    .WithDescription("Commands to list components from the current project file.");

app.Run();