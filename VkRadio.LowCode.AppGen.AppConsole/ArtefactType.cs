namespace VkRadio.LowCode.AppGenerator;

/// <summary>
/// Artefact type
/// </summary>
public class ArtefactType
{
    private ArtefactTypeCodeEnum _code;

    /// <summary>
    /// Code of generated artefact type
    /// </summary>
    public ArtefactTypeCodeEnum Code { get { return _code; } set { _code = value; } }

    public static ArtefactTypeCodeEnum Parse(string value)
    {
        return value switch
        {
            C_TYPE_MYSQL => ArtefactTypeCodeEnum.MySql,
            C_TYPE_MSSQL => ArtefactTypeCodeEnum.MsSql,
            C_TYPE_SQLITE => ArtefactTypeCodeEnum.SQLite,
            C_TYPE_CSHARP => ArtefactTypeCodeEnum.CSharp,
            C_TYPE_CSHARP_APPLICATION => ArtefactTypeCodeEnum.CSharpApplication,
            C_TYPE_CSHARP_PROJECT_MODEL => ArtefactTypeCodeEnum.CSharpProjectModel,
            C_TYPE_PHP_ZF => ArtefactTypeCodeEnum.PhpZf,
            C_TYPE_CSHARP_OLD_VERSION_SAVE => ArtefactTypeCodeEnum.CSharpOldVersionSave,
            C_TYPE_CSHARP_PROJECT_VERSION => ArtefactTypeCodeEnum.CSharpProjectVersion,
            C_TYPE_INNO_SETUP => ArtefactTypeCodeEnum.InnoSetup,
            C_TYPE_MSBUILD => ArtefactTypeCodeEnum.MSBuild,
            _ => throw new ApplicationException(string.Format("Unknown ArtefactType code: {0}.", value)),
        };
    }

    public static string ToStringLiteral(ArtefactTypeCodeEnum code)
    {
        return code switch
        {
            ArtefactTypeCodeEnum.MySql => C_TYPE_MYSQL,
            ArtefactTypeCodeEnum.MsSql => C_TYPE_MSSQL,
            ArtefactTypeCodeEnum.SQLite => C_TYPE_SQLITE,
            ArtefactTypeCodeEnum.CSharp => C_TYPE_CSHARP,
            ArtefactTypeCodeEnum.CSharpApplication => C_TYPE_CSHARP_APPLICATION,
            ArtefactTypeCodeEnum.CSharpProjectModel => C_TYPE_CSHARP_PROJECT_MODEL,
            ArtefactTypeCodeEnum.PhpZf => C_TYPE_PHP_ZF,
            ArtefactTypeCodeEnum.CSharpOldVersionSave => C_TYPE_CSHARP_OLD_VERSION_SAVE,
            ArtefactTypeCodeEnum.CSharpProjectVersion => C_TYPE_CSHARP_PROJECT_VERSION,
            ArtefactTypeCodeEnum.InnoSetup => C_TYPE_INNO_SETUP,
            ArtefactTypeCodeEnum.MSBuild => C_TYPE_MSBUILD,
            _ => throw new ApplicationException($"Тип {Enum.GetName(typeof(ArtefactTypeCodeEnum), code)} не поддерживается."),
        };
    }

    /// <summary>
    /// MySQL database
    /// </summary>
    public const string C_TYPE_MYSQL = "MySQL";
    /// <summary>
    /// MS SQL database (v. 2010)
    /// </summary>
    public const string C_TYPE_MSSQL = "MS SQL";
    /// <summary>
    /// SQLite 3 database
    /// </summary>
    public const string C_TYPE_SQLITE = "SQLite";
    /// <summary>
    /// Package of components in C#
    /// </summary>
    public const string C_TYPE_CSHARP = "C#";
    /// <summary>
    /// C# App (modular style)
    /// </summary>
    public const string C_TYPE_CSHARP_APPLICATION = "C# application";
    /// <summary>
    /// MetaModel defined in C#
    /// </summary>
    public const string C_TYPE_CSHARP_PROJECT_MODEL = "C# project - model";
    /// <summary>
    /// PHP ZF
    /// </summary>
    public const string C_TYPE_PHP_ZF = "PHP ZF";
    /// <summary>
    /// Save the previous version of C# base artefacts
    /// </summary>
    public const string C_TYPE_CSHARP_OLD_VERSION_SAVE = "C# old version save";
    /// <summary>
    /// Calculate a new C# base version
    /// </summary>
    public const string C_TYPE_CSHARP_PROJECT_VERSION = "C# project version";
    /// <summary>
    /// Inno Setup scripts package to create a deployment package
    /// </summary>
    public const string C_TYPE_INNO_SETUP = "Inno Setup";
    /// <summary>
    /// Build a project using MSBuild
    /// </summary>
    public const string C_TYPE_MSBUILD = "MSBuild";
}
