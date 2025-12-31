namespace VkRadio.LowCode.AppGenerator;

/// <summary>
/// Тип кода генерируемых артефактов
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
    /// Сохранение предыдущей версии артефактов C#
    /// </summary>
    CSharpOldVersionSave,
    /// <summary>
    /// Вычисление номера версии проекта C#
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
};
