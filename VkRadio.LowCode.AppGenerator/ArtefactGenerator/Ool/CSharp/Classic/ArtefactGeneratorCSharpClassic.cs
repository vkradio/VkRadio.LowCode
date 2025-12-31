using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic
{
    /// <summary>
    /// Generator of artefact package &quot;C# source code&quot;
    /// </summary>
    public class ArtefactGeneratorCSharpClassic: ArtefactGeneratorJson
    {
        public ArtefactGeneratorCSharpClassic(ArtefactGenerationTargetJson target) : base(target) { }

        public override void Generate()
        {
            // Create model of package of C# source code, based on database schema model.
            var solution = new CSharpSolution(this, Target.Parent.TargetSql.Generator.DBSchemaMetaModel);
            solution.Init();

            // Generate artefacts.
            solution.GeneratePackage();
        }

        public new TargetCSharpSolutionLegacy Target { get => (TargetCSharpSolutionLegacy)base.Target; }
    };
}
