using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular.Package.Root;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular;

/// <summary>
/// Generator of artefact package &quot;C# source code&quot;
/// </summary>
public class ArtefactGeneratorCSharpModular : ArtefactGenerator
{
    public ArtefactGeneratorCSharpModular(ArtefactGenerationTarget target) : base(target) { }

    public override string? Generate()
    {
        // Create model of package of C# source code, based on database schema model.
        var solution = new CSharpSolution(this, Target.Parent.TargetSql.Generator.DBSchemaMetaModel);
        solution.Init();

        // Generate artefacts.
        solution.GeneratePackage();

        return null;
    }

    public new TargetCSharpSolution Target { get => (TargetCSharpSolution)base.Target; }
}
