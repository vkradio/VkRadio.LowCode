using System.Xml.Linq;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Libs;

public class LibsParamsModernNuGet : LibsParamsModern
{
    public LibsParamsModernNuGet(XElement xelLibs, ArtefactGeneratorCSharpClassic generator)
        : base(xelLibs.Parent!, generator)
    {
    }

    public override IEnumerable<string> GenerateProjectReferenceLines(string projectPackageFullPath)
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<string> GenerateSolutionProjectsEntryLines(string solutionPackageFullPath)
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<string> GenerateSolutionProjectsConfigBatchLines(Func<Guid, IEnumerable<string>> generateBatchForProject)
    {
        throw new NotImplementedException();
    }

    public override IEnumerable<string> GenerateNestedProjectsPreSolutionLines() => Enumerable.Empty<string>();
}
