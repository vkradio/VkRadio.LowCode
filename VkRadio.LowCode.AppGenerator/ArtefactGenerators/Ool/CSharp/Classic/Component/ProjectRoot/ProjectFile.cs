using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Component;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Component.ProjectRoot;

public abstract class ProjectFile : ComponentWPredefinedCode
{
    public ProjectFile(CSharpProjectAbstract project)
    {
        Package = project;
        _emitUtf8Bom = true;
        Name = project.RootNamespace + ".csproj";
    }
}
