using System.Xml.Linq;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.LowCode.AppGen.Domain;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp;

/// <summary>
/// Generator of artefact package &quot;C# source code&quot;
/// </summary>
public class ArtefactGeneratorCSharpClassic : ArtefactGenerator
{
    public ArtefactGeneratorCSharpClassic(ArtefactTypeEnum type, DomainModel domainModel, Target target)
        : base(type, domainModel, target)
    {
    }

    public override string? Generate()
    {
        // Create model of package of C# source code, based on database schema model.
        var solution = new CSharpSolution(this, Target.Parent.TargetSql.Generator.DBSchemaMetaModel);
        solution.Init();

        // Generate artefacts.
        solution.GeneratePackage();

        return null;
    }

    protected override void InitFromTargetXElement(XElement xelTarget)
    {
        throw new NotImplementedException();
    }

    public new TargetCSharpSolutionLegacy Target { get => (TargetCSharpSolutionLegacy)base.Target; }
}
