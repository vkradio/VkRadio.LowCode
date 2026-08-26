using System.ComponentModel;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Core;

/// <summary>
/// Types of artefacts to be generated
/// </summary>
public enum ArtefactTypeEnum
{
    /// <summary>
    /// C#
    /// </summary>
    [Description("C#")]
    CSharp,
    /// <summary>
    /// PHP ZF
    /// </summary>
    [Description("PHP ZF")]
    PhpZf,
    /// <summary>
    /// MySQL
    /// </summary>
    [Description("MySQL")]
    MySql,
    /// <summary>
    /// MS SQL
    /// </summary>
    [Description("MS SQL")]
    MsSql,
    /// <summary>
    /// SQLite 3
    /// </summary>
    [Description("SQLite")]
    SQLite,
    /// <summary>
    /// Save the previous version of C# artefacts
    /// </summary>
    [Description("C# old version save")]
    CSharpOldVersionSave,
    /// <summary>
    /// Calculate the version of C# project
    /// </summary>
    [Description("C# project version")]
    CSharpProjectVersion,
    /// <summary>
    /// Inno Setup
    /// </summary>
    [Description("Inno Setup")]
    InnoSetup,
    /// <summary>
    /// MSBuild
    /// </summary>
    [Description("MSBuild")]
    MSBuild
}

public static class ArtefactTypeEnumOperations
{
    public static T? GetAttribute<T>(this Enum value) where T : Attribute
    {
        var type = value.GetType();
        var memberInfo = type.GetMember(value.ToString());
        var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);

        return (attributes?.Length ?? 0) > 0
          ? (T)attributes![0]
          : null;
    }

    public static string ToName(this Enum value)
    {
        var attribute = value.GetAttribute<DescriptionAttribute>();

        return attribute is null
            ? value.ToString()
            : attribute.Description;
    }

    public static ArtefactTypeEnum? Parse(this string value) => Enum
        .GetValues<ArtefactTypeEnum>()
        .Select(x => new { Desc = x.ToName(), Name = x.ToString(), Value = x })
        .Where(x => x.Desc == value || x.Name == value)
        .FirstOrDefault()?.Value;
}
