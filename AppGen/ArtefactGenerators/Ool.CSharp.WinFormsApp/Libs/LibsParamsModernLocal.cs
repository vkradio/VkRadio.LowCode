using System.Xml.Linq;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Libs;

public class LibsParamsModernLocal : LibsParamsModern
{
    public static Guid C_PROJ_GUID_WIN_FORMS { get; private set; }
    public static Guid C_PROJ_GUID_ORM { get; private set; }
    public static Guid C_PROJ_GUID_ORM_MS_SQL { get; private set; }
    public static Guid C_PROJ_GUID_UTILS { get; private set; }

    static LibsParamsModernLocal()
    {
        C_PROJ_GUID_WIN_FORMS = new Guid("414f2f70-eda1-4b9a-691d-1a612d4168f0");
        C_PROJ_GUID_ORM = new Guid("8dd8b7c5-f889-c0d5-1e97-c91eee23b7ac");
        C_PROJ_GUID_ORM_MS_SQL = new Guid("0821a3f8-b65b-c59e-ecdc-7cfadb22d9d4");
        C_PROJ_GUID_UTILS = new Guid("45ec0255-af7b-d16a-39ac-74045ec5175e");
    }

    public LibsParamsModernLocal(XElement xelLibs, ArtefactGeneratorCSharpClassic generator)
        : base(xelLibs.Parent!, generator)
    {
        var xelLocalDir = xelLibs.Element("LocalDir")
            ?? throw new GeneratorException("Target.Libs.LocalDir element not found for C# generation target");

        LibsPath = xelLocalDir.Value;

        if (string.IsNullOrWhiteSpace(LibsPath))
        {
            throw new GeneratorException("Target.Libs.LocalDir element value not set for C# generation target");
        }

        LibsPathIsAbsolute = !LibsPath.StartsWith(".");
    }

    public string LibsPath { get; private set; }

    public bool LibsPathIsAbsolute { get; private set; }

    public override IEnumerable<string> GenerateProjectReferenceLines(string projectPackageFullPath) => 
        GenerateProjectReference(projectPackageFullPath, C_PROJ_NAME_WIN_FORMS, C_PROJ_GUID_WIN_FORMS)
        .Concat(GenerateProjectReference(projectPackageFullPath, C_PROJ_NAME_ORM_MS_SQL, C_PROJ_GUID_ORM_MS_SQL))
        .Concat(GenerateProjectReference(projectPackageFullPath, C_PROJ_NAME_ORM, C_PROJ_GUID_ORM))
        .Concat(GenerateProjectReference(projectPackageFullPath, C_PROJ_NAME_UTILS, C_PROJ_GUID_UTILS));

    public IEnumerable<string> GenerateProjectReference(string projectPackageFullPath, string projectName, Guid projectGuid)
    {
        var pathToLibs = LibsPathIsAbsolute
            ? LibsPath
            : Path.Combine(projectPackageFullPath, LibsPath);

        var pathToProject = Path.Combine(pathToLibs, projectName, $"{projectName}.csproj");

        var result = new List<string>
        {
            "  <ProjectReference Include=\"" + pathToProject + "\">",
            "    <Project>{" + projectGuid.ToString().ToLower() + "}</Project>",
            "    <Name>" + projectName + "</Name>",
            "  </ProjectReference>"
        };

        return result;
    }

    public override IEnumerable<string> GenerateSolutionProjectsEntryLines(string solutionPackageFullPath)
    {
        //new List<string>
        //{
        //    "Project(\"{2150E333-8FDC-42A3-9474-1A3956D46DE8}\") = \"Libs\", \"Libs\", \"{02EA681E-C7D8-13C7-8484-4AC65E1B71E8}\"",
        //    "EndProject"
        //}

        throw new NotImplementedException();
    }

    public override IEnumerable<string> GenerateSolutionProjectsConfigBatchLines(Func<Guid, IEnumerable<string>> generateBatchForProject)
    {
        throw new NotImplementedException();
    }
}
