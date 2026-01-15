using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Component;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Component;

/// <summary>
/// File &lt;name&gt;.sln - VStudio&amp;s solution description
/// </summary>
public class Solution : ComponentWPredefinedCode
{
    private string[] GetConfigBatch(Guid in_projectId)
    {
        var result = new string[]
        {
            "\t\t{{{0}}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU",
            "\t\t{{{0}}}.Debug|Any CPU.Build.0 = Debug|Any CPU",
            "\t\t{{{0}}}.Release|Any CPU.ActiveCfg = Release|Any CPU",
            "\t\t{{{0}}}.Release|Any CPU.Build.0 = Release|Any CPU"
        };

        var guidStr = in_projectId.ToString().ToUpper();

        for (var i = 0; i < result.Length; i++)
        {
            result[i] = string.Format(result[i], guidStr);
        }

        return result;
    }

    public Solution(CSharpSolution rootPackage)
    {
        var cSharpAppTarget = rootPackage.Generator.Target.Parent;
        Package = rootPackage;
        _emitUtf8Bom = true;
        var name = NameHelper.NameToUnderscoreSeparatedName(rootPackage.DomainModel.Names);
        Name = name + ".sln";
        string? vcRelPath = null;

        if (!string.IsNullOrEmpty(cSharpAppTarget.VersionControlWcRoot))
        {
            vcRelPath = FileHelper.GetRelativePath(rootPackage.FullPath + "\\", cSharpAppTarget.VersionControlWcRoot);

            if (vcRelPath.Length > 0 && vcRelPath[vcRelPath.Length - 1] == '\\')
            {
                vcRelPath = vcRelPath.Substring(0, vcRelPath.Length - 1);
            }
        }

        _predefinedCode.Add(string.Empty);
        _predefinedCode.Add($"Microsoft Visual Studio Solution File, Format Version 12.00");
        _predefinedCode.Add($"# Visual Studio 14");
        _predefinedCode.Add($"VisualStudioVersion = 14.0.23107.0");
        _predefinedCode.Add($"MinimumVisualStudioVersion = 10.0.40219.1");
        _predefinedCode.Add($"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{cSharpAppTarget.OrmLibProjectName}\", \"{FileHelper.GetRelativePath(rootPackage.FullPath + "\\", cSharpAppTarget.OrmLibProjectDir)}\\{cSharpAppTarget.OrmLibProjectName}.csproj\", \"{{{TargetCSharpAppLegacy.C_ORMLIB_PROJECT_GUID_STRING}}}\"");
        _predefinedCode.Add($"EndProject");
        //if (in_rootPackage.Generator.Target.IsDependantOnSQLite)
        //{
        //    _predefinedCode.Add($"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"orm_sqlite\", \"{FileHelper.GetRelativePath(in_rootPackage.FullPath + "\\", csharpTarget.SQLiteProjectFullPath)}\", \"{{{TargetSQLite.C_SQLITE_PROJECT_GUID.ToString().ToUpper()}}}\"");
        //    _predefinedCode.Add($"EndProject");
        //}
        _predefinedCode.Add($"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{name}_base\", \"base\\{name}_base.csproj\", \"{{{rootPackage.BaseProject.ProjectGuid.ToString().ToUpper()}}}\"");
        _predefinedCode.Add($"EndProject");
        _predefinedCode.Add($"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{name}_extension\", \"extension\\{name}_extension.csproj\", \"{{{rootPackage.ExtensionProject.ProjectGuid.ToString().ToUpper()}}}\"");
        _predefinedCode.Add($"EndProject");
        _predefinedCode.Add($"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{name}_launcher\", \"launcher\\{name}_launcher.csproj\", \"{{{rootPackage.LauncherProject.ProjectGuid.ToString().ToUpper()}}}\"");
        _predefinedCode.Add($"EndProject");
        _predefinedCode.Add($"Global");
        _predefinedCode.Add($"\tGlobalSection(SolutionConfigurationPlatforms) = preSolution");
        _predefinedCode.Add($"\t\tDebug|Any CPU = Debug|Any CPU");
        _predefinedCode.Add($"\t\tRelease|Any CPU = Release|Any CPU");
        _predefinedCode.Add($"\tEndGlobalSection");
        _predefinedCode.Add($"\tGlobalSection(ProjectConfigurationPlatforms) = postSolution");
        _predefinedCode.AddRange(GetConfigBatch(new Guid(TargetCSharpAppLegacy.C_ORMLIB_PROJECT_GUID_STRING)));
        //if (in_rootPackage.Generator.Target.IsDependantOnSQLite)
        //    _predefinedCode.AddRange(GetConfigBatch(TargetSQLite.C_SQLITE_PROJECT_GUID));
        _predefinedCode.AddRange(GetConfigBatch(rootPackage.BaseProject.ProjectGuid));
        _predefinedCode.AddRange(GetConfigBatch(rootPackage.ExtensionProject.ProjectGuid));
        _predefinedCode.AddRange(GetConfigBatch(rootPackage.LauncherProject.ProjectGuid));
        _predefinedCode.Add($"\tEndGlobalSection");
        _predefinedCode.Add($"\tGlobalSection(SolutionProperties) = preSolution");
        _predefinedCode.Add($"\t\tHideSolutionNode = FALSE");
        _predefinedCode.Add($"\tEndGlobalSection");
        _predefinedCode.Add($"\tGlobalSection(ExtensibilityGlobals) = postSolution");
        _predefinedCode.Add($"\t\tVisualSVNWorkingCopyRoot = {vcRelPath}");
        _predefinedCode.Add($"\tEndGlobalSection");
        _predefinedCode.Add($"EndGlobal");
    }
}
