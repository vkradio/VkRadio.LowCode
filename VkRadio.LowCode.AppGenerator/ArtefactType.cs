using System;

namespace VkRadio.LowCode.AppGenerator;

/// <summary>
/// Тип артефактов
/// </summary>
public class ArtefactType
{
    ArtefactTypeCodeEnum _code;

    /// <summary>
    /// Тип кода генерируемых артефактов
    /// </summary>
    public ArtefactTypeCodeEnum Code { get { return _code; } set { _code = value; } }

    /// <summary>
    /// Парсинг стркового значения кода типа артефактов в его представление типа ArtefactTypeCodeEnum
    /// </summary>
    /// <param name="in_value">Строковый код типа артефакта</param>
    /// <returns>Тип артефакта ArtefactTypeCodeEnum</returns>
    public static ArtefactTypeCodeEnum Parse(string in_value)
    {
        switch (in_value)
        {
            case C_TYPE_MYSQL:
                return ArtefactTypeCodeEnum.MySql;
            case C_TYPE_MSSQL:
                return ArtefactTypeCodeEnum.MsSql;
            case C_TYPE_SQLITE:
                return ArtefactTypeCodeEnum.SQLite;
            case C_TYPE_CSHARP:
                return ArtefactTypeCodeEnum.CSharp;
            case C_TYPE_CSHARP_APPLICATION:
                return ArtefactTypeCodeEnum.CSharpApplication;
            case C_TYPE_CSHARP_PROJECT_MODEL:
                return ArtefactTypeCodeEnum.CSharpProjectModel;
            case C_TYPE_PHP_ZF:
                return ArtefactTypeCodeEnum.PhpZf;
            case C_TYPE_CSHARP_OLD_VERSION_SAVE:
                return ArtefactTypeCodeEnum.CSharpOldVersionSave;
            case C_TYPE_CSHARP_PROJECT_VERSION:
                return ArtefactTypeCodeEnum.CSharpProjectVersion;
            case C_TYPE_INNO_SETUP:
                return ArtefactTypeCodeEnum.InnoSetup;
            case C_TYPE_MSBUILD:
                return ArtefactTypeCodeEnum.MSBuild;
            default:
                throw new ApplicationException(string.Format("Unknown ArtefactType code: {0}.", in_value));
        }
    }
    public static string ToStringLiteral(ArtefactTypeCodeEnum in_code)
    {
        switch (in_code)
        {
            case ArtefactTypeCodeEnum.MySql:
                return C_TYPE_MYSQL;
            case ArtefactTypeCodeEnum.MsSql:
                return C_TYPE_MSSQL;
            case ArtefactTypeCodeEnum.SQLite:
                return C_TYPE_SQLITE;
            case ArtefactTypeCodeEnum.CSharp:
                return C_TYPE_CSHARP;
            case ArtefactTypeCodeEnum.CSharpApplication:
                return C_TYPE_CSHARP_APPLICATION;
            case ArtefactTypeCodeEnum.CSharpProjectModel:
                return C_TYPE_CSHARP_PROJECT_MODEL;
            case ArtefactTypeCodeEnum.PhpZf:
                return C_TYPE_PHP_ZF;
            case ArtefactTypeCodeEnum.CSharpOldVersionSave:
                return C_TYPE_CSHARP_OLD_VERSION_SAVE;
            case ArtefactTypeCodeEnum.CSharpProjectVersion:
                return C_TYPE_CSHARP_PROJECT_VERSION;
            case ArtefactTypeCodeEnum.InnoSetup:
                return C_TYPE_INNO_SETUP;
            case ArtefactTypeCodeEnum.MSBuild:
                return C_TYPE_MSBUILD;
            default:
                throw new ApplicationException($"Тип {Enum.GetName(typeof(ArtefactTypeCodeEnum), in_code)} не поддерживается.");
        }
    }

    /// <summary>
    /// Строковый код артефакта - скрипта генерации БД MySQL
    /// </summary>
    public const string C_TYPE_MYSQL = "MySQL";
    /// <summary>
    /// Строковый код артефакта - скрипта генерации БД MS SQL (версии 2010)
    /// </summary>
    public const string C_TYPE_MSSQL = "MS SQL";
    /// <summary>
    /// Строковый код артефакта - скрипта генерации БД SQLite 3
    /// </summary>
    public const string C_TYPE_SQLITE = "SQLite";
    /// <summary>
    /// Строковый код артефактов - пакета компонентов на C#
    /// </summary>
    public const string C_TYPE_CSHARP = "C#";
    /// <summary>
    /// Строковый код артефактов - приложение на языке C# (в новом модульном стиле)
    /// </summary>
    public const string C_TYPE_CSHARP_APPLICATION = "C# application";
    /// <summary>
    /// Строковый код артефактов - проект МПОБ на языке C#
    /// </summary>
    public const string C_TYPE_CSHARP_PROJECT_MODEL = "C# project - model";
    /// <summary>
    /// Строковый код артефактов - пакета компонентов на PHP с использованием ZF
    /// </summary>
    public const string C_TYPE_PHP_ZF = "PHP ZF";
    /// <summary>
    /// Строковый код артефактов - сохранения предыдущей версии артефактов проекта C# base
    /// </summary>
    public const string C_TYPE_CSHARP_OLD_VERSION_SAVE = "C# old version save";
    /// <summary>
    /// Строковый код артефактов - вычисление новой версии проекта C# base
    /// </summary>
    public const string C_TYPE_CSHARP_PROJECT_VERSION = "C# project version";
    /// <summary>
    /// Строковый код артефактов - пакет скриптов Inno Setup для сборки пакета развертывания
    /// </summary>
    public const string C_TYPE_INNO_SETUP = "Inno Setup";
    /// <summary>
    /// Строковый код артефактов - сборка проекта с помощью MSBuild
    /// </summary>
    public const string C_TYPE_MSBUILD = "MSBuild";
};
