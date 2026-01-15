using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Component;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Component.Properties;

public abstract class AssemblyInfoAbstract : ComponentWPredefinedCode
{
    public AssemblyInfoAbstract(PropertiesPackageAbstract package)
    {
        Package = package;
        _emitUtf8Bom = true;
        Name = "AssemblyInfo.cs";
    }
}
