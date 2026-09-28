using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Component.ProjectRoot;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Gui;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Model;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Root;

public class CSharpProjectBase : CSharpProjectAbstract
{
    public CSharpProjectBase(CSharpSolution solution, Guid projectGuid, NaturalLanguageEnum preferNaturalLanguageForComments)
        : base(solution, "base", projectGuid)
    {
        PropertiesPackage = new PropertiesPackageBase(this);
        _subpackages.Add(PropertiesPackage.Name, PropertiesPackage);

        ModelPackage = new ModelPackage(this, preferNaturalLanguageForComments);
        _subpackages.Add(ModelPackage.Name, ModelPackage);

        GuiPackage = new GuiPackage(this, preferNaturalLanguageForComments);
        _subpackages.Add(GuiPackage.Name, GuiPackage);

        ProjectFile = new ProjectFileBase(this);
        _components.Add(ProjectFile.Name, ProjectFile);
    }

    new public CSharpSolution ParentPackage => (CSharpSolution)_parentPackage;

    new public PropertiesPackageBase PropertiesPackage { get; private set; }

    public ModelPackage ModelPackage { get; private set; }

    public GuiPackage GuiPackage { get; private set; }
}
