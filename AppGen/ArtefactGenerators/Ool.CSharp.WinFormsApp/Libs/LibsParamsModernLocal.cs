using System.Linq;
using System.Xml.Linq;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.LowCode.Libs.Utils;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Libs;

public class LibsParamsModernLocal : LibsParamsModern
{
    #region Internals
    private string GetRelativePathToProject(string fullPathToRootPackage, string projectName)
    {
        var fullPathToProject = Path.Combine(LibsPath, projectName, $"{projectName}.csproj");

        var relPathToProject = FileHelper.GetRelativePath(fullPathToRootPackage + "\\", fullPathToProject);

        return relPathToProject;
    }

    private IEnumerable<string> GenerateProjectReference(string projectPackageFullPath, string projectName, Guid projectGuid) => new List<string>
    {
        $"    <ProjectReference Include=\"{GetRelativePathToProject(projectPackageFullPath, projectName)}\">",
        $"      <Project>{{{projectGuid.ToString().ToLower()}}}</Project>",
        $"      <Name>{projectName}</Name>",
        $"    </ProjectReference>"
    };

    private IEnumerable<string> GenerateSolutionProjectEntryLines(Guid type, string name, string path, Guid projectId) => new List<string>
    {
        $"Project(\"{{{type.ToString().ToUpperInvariant()}}}\") = \"{name}\", \"{path}\", \"{{{projectId.ToString().ToUpperInvariant()}}}\"",
        $"EndProject"
    };

    private string GenerateNestedProjectsPreSolutionLine(Guid projectId) =>
        $"\t\t{{{projectId.ToString().ToUpperInvariant()}}} = {{{C_PROJ_GUID_LIBS.ToString().ToUpperInvariant()}}}";
    #endregion

    public const string C_FOLDER_NAME_LIBS = "Libs";

    public static Guid C_PROJ_GUID_LIBS { get; private set; }

    public static Guid C_PROJ_GUID_WIN_FORMS { get; private set; }

    public static Guid C_PROJ_GUID_ORM { get; private set; }

    public static Guid C_PROJ_GUID_ORM_MS_SQL { get; private set; }

    public static Guid C_PROJ_GUID_UTILS { get; private set; }

    public static Guid C_PROJECT_TYPE_FOLDER { get; private set; }

    public static Guid C_PROJECT_TYPE_PROJECT { get; private set; }

    static LibsParamsModernLocal()
    {
        C_PROJ_GUID_LIBS = new Guid("02EA681E-C7D8-13C7-8484-4AC65E1B71E8");

        C_PROJ_GUID_WIN_FORMS = new Guid("414f2f70-eda1-4b9a-691d-1a612d4168f0");
        C_PROJ_GUID_ORM = new Guid("8dd8b7c5-f889-c0d5-1e97-c91eee23b7ac");
        C_PROJ_GUID_ORM_MS_SQL = new Guid("0821a3f8-b65b-c59e-ecdc-7cfadb22d9d4");
        C_PROJ_GUID_UTILS = new Guid("45ec0255-af7b-d16a-39ac-74045ec5175e");

        C_PROJECT_TYPE_FOLDER = new Guid("2150E333-8FDC-42A3-9474-1A3956D46DE8");
        C_PROJECT_TYPE_PROJECT = new Guid("FAE04EC0-301F-11D3-BF4B-00C04F79EFBC");
    }

    public LibsParamsModernLocal(XElement xelLibs, ArtefactGeneratorCSharpClassic generator)
        : base(xelLibs.Parent!, generator)
    {
        var xelLocalDir = xelLibs.Element("LocalDir")
            ?? throw new GeneratorException("Target.Libs.LocalDir element not found for C# generation target");

        if (string.IsNullOrWhiteSpace(xelLocalDir.Value))
        {
            throw new GeneratorException("Target.Libs.LocalDir element value not set for C# generation target");
        }

        var isAbsolute = !xelLocalDir.Value.StartsWith(".");

        LibsPath = isAbsolute
            ? xelLocalDir.Value
            : Path.Combine(generator.Target.Project.ProjectRootPath, xelLocalDir.Value);

        if (!Directory.Exists(LibsPath))
        {
            throw new GeneratorException($"Directory \"{LibsPath}\" not exists");
        }
    }

    public string LibsPath { get; private set; }

    public override IEnumerable<string> GenerateProjectReferenceLines(string projectPackageFullPath) => 
        GenerateProjectReference(projectPackageFullPath, C_PROJ_NAME_WIN_FORMS, C_PROJ_GUID_WIN_FORMS)
        .Concat(GenerateProjectReference(projectPackageFullPath, C_PROJ_NAME_ORM_MS_SQL, C_PROJ_GUID_ORM_MS_SQL))
        .Concat(GenerateProjectReference(projectPackageFullPath, C_PROJ_NAME_ORM, C_PROJ_GUID_ORM))
        .Concat(GenerateProjectReference(projectPackageFullPath, C_PROJ_NAME_UTILS, C_PROJ_GUID_UTILS));

    public override IEnumerable<string> GenerateSolutionProjectsEntryLines(string solutionPackageFullPath) =>
        GenerateSolutionProjectEntryLines(C_PROJECT_TYPE_FOLDER, C_FOLDER_NAME_LIBS, C_FOLDER_NAME_LIBS, C_PROJ_GUID_LIBS)
        .Concat(GenerateSolutionProjectEntryLines(C_PROJECT_TYPE_PROJECT, C_PROJ_NAME_UTILS, GetRelativePathToProject(solutionPackageFullPath, C_PROJ_NAME_UTILS), C_PROJ_GUID_UTILS))
        .Concat(GenerateSolutionProjectEntryLines(C_PROJECT_TYPE_PROJECT, C_PROJ_NAME_ORM, GetRelativePathToProject(solutionPackageFullPath, C_PROJ_NAME_ORM), C_PROJ_GUID_ORM))
        .Concat(GenerateSolutionProjectEntryLines(C_PROJECT_TYPE_PROJECT, C_PROJ_NAME_ORM_MS_SQL, GetRelativePathToProject(solutionPackageFullPath, C_PROJ_NAME_ORM_MS_SQL), C_PROJ_GUID_ORM_MS_SQL))
        .Concat(GenerateSolutionProjectEntryLines(C_PROJECT_TYPE_PROJECT, C_PROJ_NAME_WIN_FORMS, GetRelativePathToProject(solutionPackageFullPath, C_PROJ_NAME_WIN_FORMS), C_PROJ_GUID_WIN_FORMS));

    public override IEnumerable<string> GenerateSolutionProjectsConfigBatchLines(Func<Guid, IEnumerable<string>> generateBatchForProject) =>
        generateBatchForProject(C_PROJ_GUID_UTILS)
        .Concat(generateBatchForProject(C_PROJ_GUID_ORM))
        .Concat(generateBatchForProject(C_PROJ_GUID_ORM))
        .Concat(generateBatchForProject(C_PROJ_GUID_ORM_MS_SQL))
        .Concat(generateBatchForProject(C_PROJ_GUID_WIN_FORMS));

    public override IEnumerable<string> GenerateNestedProjectsPreSolutionLines() => new List<string>
    {
        GenerateNestedProjectsPreSolutionLine(C_PROJ_GUID_UTILS),
        GenerateNestedProjectsPreSolutionLine(C_PROJ_GUID_ORM),
        GenerateNestedProjectsPreSolutionLine(C_PROJ_GUID_ORM_MS_SQL),
        GenerateNestedProjectsPreSolutionLine(C_PROJ_GUID_WIN_FORMS)
    };
}
