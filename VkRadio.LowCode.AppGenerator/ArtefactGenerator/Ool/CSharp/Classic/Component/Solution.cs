using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Component;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;
using System;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Component
{
    /// <summary>
    /// File &lt;name&gt;.sln - VStudio&amp;s solution description
    /// </summary>
    public class Solution: ComponentWPredefinedCode
    {
        string[] GetConfigBatch(Guid in_projectId)
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
                result[i] = string.Format(result[i], guidStr);
            return result;
        }

        public Solution(CSharpSolution in_rootPackage)
        {
            var cSharpAppTarget = in_rootPackage.Generator.Target.Parent;
            Package = in_rootPackage;
            _emitUtf8Bom = true;
            var name = NameHelper.NameToUnderscoreSeparatedName(in_rootPackage.DomainModel.Names);
            Name = name + ".sln";
            string vcRelPath = null;
            if (!string.IsNullOrEmpty(cSharpAppTarget.VersionControlWcRoot))
            {
                vcRelPath = FileHelper.GetRelativePath(in_rootPackage.FullPath + "\\", cSharpAppTarget.VersionControlWcRoot);
                if (vcRelPath.Length > 0 && vcRelPath[vcRelPath.Length - 1] == '\\')
                    vcRelPath = vcRelPath.Substring(0, vcRelPath.Length - 1);
            }

            _predefinedCode.Add(string.Empty);
            _predefinedCode.Add($"Microsoft Visual Studio Solution File, Format Version 12.00");
            _predefinedCode.Add($"# Visual Studio 14");
            _predefinedCode.Add($"VisualStudioVersion = 14.0.23107.0");
            _predefinedCode.Add($"MinimumVisualStudioVersion = 10.0.40219.1");
            _predefinedCode.Add($"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{cSharpAppTarget.OrmLibProjectName}\", \"{FileHelper.GetRelativePath(in_rootPackage.FullPath + "\\", cSharpAppTarget.OrmLibProjectDir)}\\{cSharpAppTarget.OrmLibProjectName}.csproj\", \"{{{TargetCSharpAppLegacy.C_ORMLIB_PROJECT_GUID_STRING}}}\"");
            _predefinedCode.Add($"EndProject");
            //if (in_rootPackage.Generator.Target.IsDependantOnSQLite)
            //{
            //    _predefinedCode.Add($"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"orm_sqlite\", \"{FileHelper.GetRelativePath(in_rootPackage.FullPath + "\\", csharpTarget.SQLiteProjectFullPath)}\", \"{{{TargetSQLite.C_SQLITE_PROJECT_GUID.ToString().ToUpper()}}}\"");
            //    _predefinedCode.Add($"EndProject");
            //}
            _predefinedCode.Add($"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{name}_base\", \"base\\{name}_base.csproj\", \"{{{in_rootPackage.BaseProject.ProjectGuid.ToString().ToUpper()}}}\"");
            _predefinedCode.Add($"EndProject");
            _predefinedCode.Add($"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{name}_extension\", \"extension\\{name}_extension.csproj\", \"{{{in_rootPackage.ExtensionProject.ProjectGuid.ToString().ToUpper()}}}\"");
            _predefinedCode.Add($"EndProject");
            _predefinedCode.Add($"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{name}_launcher\", \"launcher\\{name}_launcher.csproj\", \"{{{in_rootPackage.LauncherProject.ProjectGuid.ToString().ToUpper()}}}\"");
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
            _predefinedCode.AddRange(GetConfigBatch(in_rootPackage.BaseProject.ProjectGuid));
            _predefinedCode.AddRange(GetConfigBatch(in_rootPackage.ExtensionProject.ProjectGuid));
            _predefinedCode.AddRange(GetConfigBatch(in_rootPackage.LauncherProject.ProjectGuid));
            _predefinedCode.Add($"\tEndGlobalSection");
            _predefinedCode.Add($"\tGlobalSection(SolutionProperties) = preSolution");
            _predefinedCode.Add($"\t\tHideSolutionNode = FALSE");
            _predefinedCode.Add($"\tEndGlobalSection");
            _predefinedCode.Add($"\tGlobalSection(ExtensibilityGlobals) = postSolution");
            _predefinedCode.Add($"\t\tVisualSVNWorkingCopyRoot = {vcRelPath}");
            _predefinedCode.Add($"\tEndGlobalSection");
            _predefinedCode.Add($"EndGlobal");
        }
    };
}
