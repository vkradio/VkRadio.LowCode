using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Model;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Root;

public class CSharpProjectBase : CSharpProjectAbstract
{
    public CSharpProjectBase(CSharpSolution solution, Guid projectGuid)
        : base(solution, "base", projectGuid)
    {
        PropertiesPackage = new PropertiesPackageBase(this);
        _subpackages.Add(PropertiesPackage.Name, PropertiesPackage);

        ModelPackage = new ModelPackage(this);
        _subpackages.Add(ModelPackage.Name, ModelPackage);

        GuiPackage = new GuiPackage(this);
        _subpackages.Add(GuiPackage.Name, GuiPackage);

        ProjectFile = new ProjectFileBase(this);
        _components.Add(ProjectFile.Name, ProjectFile);
    }

    new public CSharpSolution ParentPackage { get { return (CSharpSolution)_parentPackage; } }
    new public PropertiesPackageBase PropertiesPackage { get; private set; }
    public ModelPackage ModelPackage { get; private set; }
    public GuiPackage GuiPackage { get; private set; }
}
