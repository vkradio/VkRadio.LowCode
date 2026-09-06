using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Component;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Root;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Component.ProjectRoot;

public abstract class ProjectFile : ComponentWPredefinedCode
{
    public ProjectFile(CSharpProjectAbstract project)
    {
        Package = project;
        _emitUtf8Bom = true;
        Name = project.RootNamespace + ".csproj";
    }
}
