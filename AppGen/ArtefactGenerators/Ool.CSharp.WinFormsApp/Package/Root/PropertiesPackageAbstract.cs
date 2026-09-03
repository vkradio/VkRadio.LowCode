using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Component.Properties;
using PackNS = VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Package;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Root;

public abstract class PropertiesPackageAbstract : PackNS.Package
{
    public PropertiesPackageAbstract(CSharpProjectAbstract project)
        : base(project, "Properties")
    {
        Resources = new Resources(this);
        _components.Add(Resources.Name, Resources);

        ResourcesDesigner = new ResourcesDesigner(this);
        _components.Add(ResourcesDesigner.Name, ResourcesDesigner);
    }

    public new CSharpProjectAbstract ParentPackage => (CSharpProjectAbstract)_parentPackage;

    public AssemblyInfoAbstract AssemblyInfo { get; protected set; }

    public Resources Resources { get; private set; }

    public ResourcesDesigner ResourcesDesigner { get; private set; }
}
