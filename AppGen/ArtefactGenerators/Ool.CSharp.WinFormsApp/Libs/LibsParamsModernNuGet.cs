using System.Xml.Linq;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Libs;

public class LibsParamsModernNuGet : LibsParamsModern
{
    #region Internals
    private const string DELETEME = "DELETEME";

    private IEnumerable<string> GenerateNuGetProjectReference(string libShortName, bool legacyProjectFileFormat) =>
        legacyProjectFileFormat
            ? new List<string>
            {
                $"    <PackageReference Include=\"VkRadio.LowCode.Libs.{libShortName}\">",
                SolutionWideVersionManagement ? DELETEME : $"      <Version>*</Version>",
                $"    </PackageReference>"
            }.Where(x => x != DELETEME)

            : new List<string>
            {
                $"    <PackageReference Include=\"VkRadio.LowCode.Libs.{libShortName}\"{(SolutionWideVersionManagement ? string.Empty : " Version=\"*\"")} />"
            };
    #endregion

    public bool SolutionWideVersionManagement { get; private set; }

    public LibsParamsModernNuGet(XElement xelLibs, ArtefactGeneratorCSharpClassic generator)
        : base(xelLibs.Parent!, generator)
    {
        var xelSolutionWideVersionManagement = xelLibs.Element("SolutionWideVersionManagement");

        if (xelSolutionWideVersionManagement is not null &&
            bool.TryParse(xelSolutionWideVersionManagement.Value, out var solutionWideVersionManagement))
        {
            SolutionWideVersionManagement = solutionWideVersionManagement;
        }
    }

    public override IEnumerable<string> GenerateNuGetPackageReferenceLines(bool legacyProjectFileFormat = false) =>
        GenerateNuGetProjectReference(C_PROJ_NAME_WIN_FORMS, legacyProjectFileFormat)
        .Concat(GenerateNuGetProjectReference(C_PROJ_NAME_ORM, legacyProjectFileFormat))
        .Concat(GenerateNuGetProjectReference(C_PROJ_NAME_ORM_MS_SQL, legacyProjectFileFormat))
        .Concat(GenerateNuGetProjectReference(C_PROJ_NAME_UTILS, legacyProjectFileFormat));

    public override IEnumerable<string> GenerateProjectReferenceLines(string projectPackageFullPath, bool legacyProjectFileFormat = false) => Enumerable.Empty<string>();

    public override IEnumerable<string> GenerateSolutionProjectsEntryLines(string solutionPackageFullPath) => Enumerable.Empty<string>();

    public override IEnumerable<string> GenerateSolutionProjectsConfigBatchLines(Func<Guid, IEnumerable<string>> generateBatchForProject) => Enumerable.Empty<string>();

    public override IEnumerable<string> GenerateNestedProjectsPreSolutionLines() => Enumerable.Empty<string>();
}
