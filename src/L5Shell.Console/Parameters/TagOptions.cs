using Cocona;
using JetBrains.Annotations;

namespace L5Shell.Console.Parameters;

[PublicAPI]
public class TagOptions : ICommandParameterSet
{
    [Option("desc", Description = "Specifies the description of the tag.")]
    [HasDefaultValue]
    public string? Description { get; set; }
}