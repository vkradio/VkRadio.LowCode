using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Component.Properties;
using PackNS = VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Package;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;

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

    public new CSharpProjectAbstract ParentPackage { get { return (CSharpProjectAbstract)_parentPackage; } }
    public AssemblyInfoAbstract AssemblyInfo { get; protected set; }
    public Resources Resources { get; private set; }
    public ResourcesDesigner ResourcesDesigner { get; private set; }
}
