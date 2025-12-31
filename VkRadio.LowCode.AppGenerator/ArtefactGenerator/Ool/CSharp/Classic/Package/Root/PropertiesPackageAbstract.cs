using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Component.Properties;
using PackNS = ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Package;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Root
{
    public abstract class PropertiesPackageAbstract: PackNS.Package
    {
        public PropertiesPackageAbstract(CSharpProjectAbstract in_project)
            : base(in_project, "Properties")
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
    };
}
