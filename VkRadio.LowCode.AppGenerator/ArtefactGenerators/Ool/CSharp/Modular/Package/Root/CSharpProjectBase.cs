using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular.Component.ProjectRoot;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular.Package.Model;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular.Package.Root;

public class CSharpProjectBase : CSharpProjectAbstract
{
    public CSharpProjectBase(CSharpSolution solution, Guid projectGuid)
        : base(solution, "base", projectGuid)
    {
        ModelPackage = new ModelPackage(this);
        _subpackages.Add(ModelPackage.Name, ModelPackage);

        ProjectFile = new ProjectFileBase(this);
        _components.Add(ProjectFile.Name, ProjectFile);
    }

    new public CSharpSolution ParentPackage { get { return (CSharpSolution)_parentPackage; } }
    public ModelPackage ModelPackage { get; private set; }
}
