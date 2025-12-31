using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Component;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Component.Properties
{
    public abstract class AssemblyInfoAbstract: ComponentWPredefinedCode
    {
        public AssemblyInfoAbstract(PropertiesPackageAbstract in_package)
        {
            Package = in_package;
            _emitUtf8Bom = true;
            Name = "AssemblyInfo.cs";
        }
    };
}
