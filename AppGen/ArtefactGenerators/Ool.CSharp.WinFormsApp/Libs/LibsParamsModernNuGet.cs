using System.Xml.Linq;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Libs;

public class LibsParamsModernNuGet : LibsParamsModern
{
    public LibsParamsModernNuGet(XElement initFromTargetXElement, ArtefactGeneratorCSharpClassic generator)
        : base(initFromTargetXElement, generator)
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
}
