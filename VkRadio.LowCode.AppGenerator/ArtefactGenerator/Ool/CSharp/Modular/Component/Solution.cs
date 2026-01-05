using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Component;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular.Package.Root;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular.Component;

/// <summary>
/// File &lt;name&gt;.sln - VStudio&amp;s solution description
/// </summary>
public class Solution : ComponentWPredefinedCode
{
    private string[] GetConfigBatch(Guid projectId) => GetConfigBatch(projectId.ToString().ToUpper());

    private string[] GetConfigBatch(string projectId)
    {
        var result = new string[]
        {
            "\t\t{{{0}}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU",
            "\t\t{{{0}}}.Debug|Any CPU.Build.0 = Debug|Any CPU",
            "\t\t{{{0}}}.Release|Any CPU.ActiveCfg = Release|Any CPU",
            "\t\t{{{0}}}.Release|Any CPU.Build.0 = Release|Any CPU"
        };

        for (var i = 0; i < result.Length; i++)
        {
            result[i] = string.Format(result[i], projectId);
        }

        return result;
    }

    public Solution(CSharpSolution rootPackage)
    {
        DoNotOverwriteIfAlreadyExists = true;

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
        _predefinedCode.Add($"# Visual Studio 15");
        _predefinedCode.Add($"VisualStudioVersion = 15.0.27004.2009");
        _predefinedCode.Add($"MinimumVisualStudioVersion = 10.0.40219.1");
        _predefinedCode.Add($"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{name}_base\", \"base\\{name}_base.csproj\", \"{{{rootPackage.BaseProject.ProjectGuid.ToString().ToUpper()}}}\"");
        _predefinedCode.Add($"EndProject");
        //_predefinedCode.Add($"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{name}_extension\", \"extension\\{name}_extension.csproj\", \"{{{in_rootPackage.ExtensionProject.ProjectGuid.ToString().ToUpper()}}}\"");
        //_predefinedCode.Add($"EndProject");
        _predefinedCode.Add($"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{cSharpAppTarget.OrmLibProjectNameDb}\", \"{FileHelper.GetRelativePath(rootPackage.FullPath + "\\", cSharpAppTarget.GetOrmLibProjectFilePathDb())}\", \"{{{TargetCSharpApp.C_ORMLIB_PROJECT_DB_GUID_STRING}}}\"");
        _predefinedCode.Add($"EndProject");
        _predefinedCode.Add($"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{cSharpAppTarget.OrmLibProjectNameDbConcrete}\", \"{FileHelper.GetRelativePath(rootPackage.FullPath + "\\", cSharpAppTarget.GetOrmLibProjectFilePathDbConcrete())}\", \"{{{TargetCSharpApp.C_ORMLIB_PROJECT_DB_CONCRETE_GUID_STRING}}}\"");
        _predefinedCode.Add($"EndProject");
        _predefinedCode.Add($"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{cSharpAppTarget.OrmLibProjectNameUtil}\", \"{FileHelper.GetRelativePath(rootPackage.FullPath + "\\", cSharpAppTarget.GetOrmLibProjectFilePathUtil())}\", \"{{{TargetCSharpApp.C_ORMLIB_PROJECT_UTIL_GUID_STRING}}}\"");
        _predefinedCode.Add($"EndProject");
        _predefinedCode.Add($"Global");
        _predefinedCode.Add($"\tGlobalSection(SolutionConfigurationPlatforms) = preSolution");
        _predefinedCode.Add($"\t\tDebug|Any CPU = Debug|Any CPU");
        _predefinedCode.Add($"\t\tRelease|Any CPU = Release|Any CPU");
        _predefinedCode.Add($"\tEndGlobalSection");
        _predefinedCode.Add($"\tGlobalSection(ProjectConfigurationPlatforms) = postSolution");
        _predefinedCode.AddRange(GetConfigBatch(rootPackage.BaseProject.ProjectGuid));
        //_predefinedCode.AddRange(GetConfigBatch(in_rootPackage.ExtensionProject.ProjectGuid));
        _predefinedCode.AddRange(GetConfigBatch(TargetCSharpApp.C_ORMLIB_PROJECT_DB_GUID_STRING));
        _predefinedCode.AddRange(GetConfigBatch(TargetCSharpApp.C_ORMLIB_PROJECT_DB_CONCRETE_GUID_STRING));
        _predefinedCode.AddRange(GetConfigBatch(TargetCSharpApp.C_ORMLIB_PROJECT_UTIL_GUID_STRING));
        _predefinedCode.Add($"\tEndGlobalSection");
        _predefinedCode.Add($"\tGlobalSection(SolutionProperties) = preSolution");
        _predefinedCode.Add($"\t\tHideSolutionNode = FALSE");
        _predefinedCode.Add($"\tEndGlobalSection");

        if (!string.IsNullOrEmpty(vcRelPath))
        {
            _predefinedCode.Add($"\tGlobalSection(ExtensibilityGlobals) = postSolution");
            _predefinedCode.Add($"\t\tVisualSVNWorkingCopyRoot = {vcRelPath}");
            _predefinedCode.Add($"\tEndGlobalSection");
            // TODO: Also in this section we need something like
            // SolutionGuid = {C51291D1-5885-418F-A99D-2BA4DD5A6637}
            // but it is unclear how to generate it.
        }
        _predefinedCode.Add($"EndGlobal");
    }
}
