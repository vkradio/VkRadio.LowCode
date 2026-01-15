using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Component;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular.Package.Root;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular.Component.ProjectRoot;

public abstract class ProjectFile : ComponentWPredefinedCode
{
    public ProjectFile(CSharpProjectAbstract project)
    {
        Package = project;
        Name = project.RootNamespace + ".csproj";
    }
}
