namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Core;

/// <summary>
/// Types of artefacts to be generated
/// </summary>
public enum ArtefactTypeEnum
{
    /// <summary>
    /// C#
    /// </summary>
    CSharp,
    /// <summary>
    /// PHP ZF
    /// </summary>
    PhpZf,
    /// <summary>
    /// MySQL
    /// </summary>
    MySql,
    /// <summary>
    /// MS SQL
    /// </summary>
    MsSql,
    /// <summary>
    /// SQLite 3
    /// </summary>
    SQLite,
    /// <summary>
    /// Save the previous version of C# artefacts
    /// </summary>
    CSharpOldVersionSave,
    /// <summary>
    /// Calculate the version of C# project
    /// </summary>
    CSharpProjectVersion,
    /// <summary>
    /// Inno Setup
    /// </summary>
    InnoSetup,
    /// <summary>
    /// MSBuild
    /// </summary>
    MSBuild
}
