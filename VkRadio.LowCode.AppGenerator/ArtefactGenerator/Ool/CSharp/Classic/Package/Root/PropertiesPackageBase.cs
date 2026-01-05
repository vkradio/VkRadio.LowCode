using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Component.Properties;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;

public class PropertiesPackageBase : PropertiesPackageAbstract
{
    public PropertiesPackageBase(CSharpProjectBase project)
        : base(project)
    {
        base.AssemblyInfo = new AssemblyInfoBase(this);
        _components.Add(AssemblyInfo.Name, AssemblyInfo);
    }

    public new CSharpProjectBase ParentPackage { get { return (CSharpProjectBase)_parentPackage; } }
    public new AssemblyInfoBase AssemblyInfo { get { return (AssemblyInfoBase)base.AssemblyInfo; } }
}
