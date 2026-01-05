using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Package;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Component;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;

public class CSharpSolution : ProjectPackage
{
    /// <summary>
    /// Increment ProjectGuid of &quot;base&quot; project to generate ProjectGuid
    /// of &quot;extension&quot; project
    /// </summary>
    /// <param name="guid">ProjectGuid of &quot;base&quot; project</param>
    /// <returns>ProjectGuid of &quot;extension&quot; project</returns>
    private Guid SimpleIncrementGuid(Guid guid)
    {
        var extIdBytes = guid.ToByteArray();
        var byteVal = extIdBytes[extIdBytes.Length - 1];
        byteVal++;
        extIdBytes[extIdBytes.Length - 1] = (byte)(byteVal % 255);
        return new Guid(extIdBytes);
    }

    /// <summary>
    /// Public constructor for partial pre-initializing
    /// </summary>
    /// <param name="cSharpGenerator">C# artefacts generator</param>
    /// <param name="dbSchemaModel">Database schema model</param>
    public CSharpSolution(ArtefactGeneratorCSharpClassic cSharpGenerator, DBSchemaMetaModel dbSchemaModel)
        : base(cSharpGenerator.Target.Project.MetaModel, cSharpGenerator.Target, dbSchemaModel)
        => Generator = cSharpGenerator;

    /// <summary>
    /// Initializing after creation for concrete class
    /// </summary>
    public override void Init()
    {
        var projectId = ArtefactGenerationTarget.Parent.Id;
        BaseProject = new CSharpProjectBase(this, projectId);
        _subpackages.Add(BaseProject.Name, BaseProject);

        projectId = SimpleIncrementGuid(projectId);
        ExtensionProject = new CSharpProjectExtension(this, projectId);
        _subpackages.Add(ExtensionProject.Name, ExtensionProject);

        projectId = SimpleIncrementGuid(projectId);
        LauncherProject = new CSharpProjectLauncher(this, projectId);
        _subpackages.Add(LauncherProject.Name, LauncherProject);

        MiniSolutionDescriptor = new Solution(this);
        _components.Add(MiniSolutionDescriptor.Name, MiniSolutionDescriptor);
    }

    public new TargetCSharpSolutionLegacy ArtefactGenerationTarget { get => (TargetCSharpSolutionLegacy)base.ArtefactGenerationTarget; }
    public ArtefactGeneratorCSharpClassic Generator { get; private set; }
    public Solution MiniSolutionDescriptor { get; private set; }
    public CSharpProjectBase BaseProject { get; private set; }
    public CSharpProjectExtension ExtensionProject { get; private set; }
    public CSharpProjectLauncher LauncherProject { get; private set; }
}
