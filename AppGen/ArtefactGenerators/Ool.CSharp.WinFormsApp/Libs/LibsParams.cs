using Ardalis.GuardClauses;
using System.Xml.Linq;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Libs;

public abstract class LibsParams
{
    protected LibsParams(XElement initFromTargetXElement, ArtefactGeneratorCSharpClassic generator)
    {
        Generator = Guard.Against.Null(generator, nameof(generator));
    }

    public ArtefactGeneratorCSharpClassic Generator { get; protected set; }

    public abstract IEnumerable<string> GenerateProjectReferenceLines(string projectPackageFullPath);

    public abstract IEnumerable<string> GenerateSolutionProjectsEntryLines(string solutionPackageFullPath);

    public abstract IEnumerable<string> GenerateSolutionProjectsConfigBatchLines(Func<Guid, IEnumerable<string>> generateBatchForProject);

    public abstract IEnumerable<string> GenerateNestedProjectsPreSolutionLines();

    public static LibsParams Deserialize(XElement initFromTargetXElement, ArtefactGeneratorCSharpClassic generator)
    {
        LibsParams result;

        var xelLibs = initFromTargetXElement.Element("Libs");

        if (xelLibs == null)
        {
            result = new LibsParamsLegacy(initFromTargetXElement, generator);
        }
        else
        {
            var xelUseNuGet = xelLibs.Element("UseNuGet");

            if (xelUseNuGet is null ||
                !bool.Parse(xelUseNuGet.Value))
            {
                result = new LibsParamsModernNuGet(initFromTargetXElement, generator);
            }
            else
            {
                result = new LibsParamsModernLocal(initFromTargetXElement, generator);
            }
        }

        return result;
    }
}
