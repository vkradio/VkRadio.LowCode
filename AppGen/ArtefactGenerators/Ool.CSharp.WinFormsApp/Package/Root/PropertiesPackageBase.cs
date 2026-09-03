using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Component.Properties;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Root;

public class PropertiesPackageBase : PropertiesPackageAbstract
{
    public PropertiesPackageBase(CSharpProjectBase project)
        : base(project)
    {
        base.AssemblyInfo = new AssemblyInfoBase(this);
        _components.Add(AssemblyInfo.Name, AssemblyInfo);
    }

    public new CSharpProjectBase ParentPackage => (CSharpProjectBase)_parentPackage;

    public new AssemblyInfoBase AssemblyInfo => (AssemblyInfoBase)base.AssemblyInfo;
}
