namespace VkRadio.LowCode.AppGenerator;

/// <summary>
/// Types of artefacts to be generated
/// </summary>
public enum ArtefactTypeCodeEnum
{
    /// <summary>
    /// C#
    /// </summary>
    CSharp,
    /// <summary>
    /// C# application
    /// </summary>
    CSharpApplication,
    /// <summary>
    /// C# project - model
    /// </summary>
    CSharpProjectModel,
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
    /// Calculate a version number of a C# project
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
