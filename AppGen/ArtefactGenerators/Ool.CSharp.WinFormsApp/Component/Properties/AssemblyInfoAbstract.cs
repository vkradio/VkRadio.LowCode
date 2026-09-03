using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Component;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Root;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Component.Properties;

public abstract class AssemblyInfoAbstract : ComponentWPredefinedCode
{
    public AssemblyInfoAbstract(PropertiesPackageAbstract package)
    {
        Package = package;
        _emitUtf8Bom = true;
        Name = "AssemblyInfo.cs";
    }
}
