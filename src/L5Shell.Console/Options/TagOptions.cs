using Cocona;
using JetBrains.Annotations;
using L5Sharp.Core;
using L5Shell.Console.Common;

namespace L5Shell.Console.Options;

[PublicAPI]
public class TagOptions : ICommandParameterSet
{
    [Option("datatype", Description = "Specifies the DataType property of the tag.")]
    [HasDefaultValue]
    public string? DataType { get; set; }

    [Option("tagname", Description = "Specifies the TagName property of the tag.")]
    [HasDefaultValue]
    public string? TagName { get; set; }

    [Option("desc", Description = "Specifies the Description property of the tag.")]
    [HasDefaultValue]
    public string? Description { get; set; }

    [Option("radix", Description = "Specifies the Radix property of the tag.")]
    [HasDefaultValue]
    public string? Radix { get; set; }

    [Option("access", Description = "Specifies the ExternalAccess property of the tag.")]
    [HasDefaultValue]
    public string? ExternalAccess { get; set; }

    [Option("type", Description = "Specifies the TagType property of the tag.")]
    [HasDefaultValue]
    public string? TagType { get; set; }

    [Option("scope", Description = "Specifies the Scope property of the tag.")]
    [HasDefaultValue]
    public string? Scope { get; set; }

    [Option("container", Description = "Specifies the Container (scope name) property of the tag.")]
    [HasDefaultValue]
    public string? Container { get; set; }

    [Option("alias", Description = "Specifies the AliasFor property of the tag.")]
    [HasDefaultValue]
    public string? AliasFor { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    public bool Filter(Tag tag)
    {
        return PassesTagName(tag)
               && HasDescription(tag)
               && HasDataType(tag)
               && HasRadix(tag);
    }

    private bool PassesTagName(Tag tag) => TagName is null || tag.TagName.ToString().Like(TagName);
    private bool HasDescription(Tag tag) => Description is null || tag.Description?.Like(Description) is true;
    private bool HasDataType(Tag tag) => DataType is null || tag.DataType.Like(DataType);
    private bool HasRadix(Tag tag) => Radix is null || tag.Radix.Name == Radix;
}