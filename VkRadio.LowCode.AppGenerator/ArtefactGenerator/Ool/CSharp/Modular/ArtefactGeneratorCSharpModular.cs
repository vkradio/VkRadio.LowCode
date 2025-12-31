using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular.Package.Root;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular
{
    /// <summary>
    /// Generator of artefact package &quot;C# source code&quot;
    /// </summary>
    public class ArtefactGeneratorCSharpModular: ArtefactGeneratorJson
    {
        public ArtefactGeneratorCSharpModular(ArtefactGenerationTargetJson target) : base(target) { }

        public override void Generate()
        {
            // Create model of package of C# source code, based on database schema model.
            var solution = new CSharpSolution(this, Target.Parent.TargetSql.Generator.DBSchemaMetaModel);
            solution.Init();

            // Generate artefacts.
            solution.GeneratePackage();
        }

        public new TargetCSharpSolution Target { get => (TargetCSharpSolution)base.Target; }
    };
}
