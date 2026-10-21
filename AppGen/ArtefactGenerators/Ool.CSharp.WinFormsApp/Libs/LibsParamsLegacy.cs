using System.Reflection.Emit;
using System.Xml.Linq;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.LowCode.Libs.Utils;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Libs;

public class LibsParamsLegacy : LibsParams
{
    public LibsParamsLegacy(XElement initFromTargetXElement, ArtefactGeneratorCSharpClassic generator)
        : base(initFromTargetXElement, generator)
    {
        OrmLibProjectName = $"orm_{generator.DotNetVersion}";

        var ormProjectFileName = OrmLibProjectName + ".csproj";

        if (initFromTargetXElement.Element("OrmLibProjectDir") is null)
        {
            throw new GeneratorException("C# Target has no <OrmLibProjectDir> value");
        }

        OrmLibProjectDir = Path.Combine(generator.Target.Project.ProjectRootPath, initFromTargetXElement.Element("OrmLibProjectDir")!.Value, ormProjectFileName);

        if (!File.Exists(OrmLibProjectDir))
        {
            throw new GeneratorException($"File \"{OrmLibProjectDir}\" not exists");
        }
    }

    public string OrmLibProjectDir { get; private set; }

    /// <summary>
    /// Ormlib project name without any .csproj extension and path (as displayed in VS solution tree)
    /// </summary>
    public string OrmLibProjectName { get; private set; }

    public string GetOrmLibProjectFilePath() => Path.Combine(OrmLibProjectDir, OrmLibProjectName + ".csproj");


    public static readonly string C_ORMLIB_PROJECT_GUID_STRING = "BC2581BB-BC55-4E13-9AED-69CE482E092D";

    public override IEnumerable<string> GenerateProjectReferenceLines(string projectPackageFullPath)
    {
        var result = new List<string>
        {
            $"    <ProjectReference Include=\"{FileHelper.GetRelativePath(projectPackageFullPath + "\\", OrmLibProjectDir)}\">",
            $"      <Project>{{{C_ORMLIB_PROJECT_GUID_STRING.ToLower()}}}</Project>",
            $"      <Name>{OrmLibProjectName}</Name>",
            "    </ProjectReference>"
        };

        return result;
    }

    public override IEnumerable<string> GenerateSolutionProjectsEntryLines(string solutionPackageFullPath)
    {
        var result = new List<string>
        {
            $"Project(\"{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}\") = \"{OrmLibProjectName}\", \"{FileHelper.GetRelativePath(solutionPackageFullPath + "\\", OrmLibProjectDir)}\", \"{{{C_ORMLIB_PROJECT_GUID_STRING}}}\"",
            $"EndProject"
        };

        return result;
    }

    public override IEnumerable<string> GenerateSolutionProjectsConfigBatchLines(Func<Guid, IEnumerable<string>> generateBatchForProject) =>
        generateBatchForProject(new Guid(C_ORMLIB_PROJECT_GUID_STRING));
}
