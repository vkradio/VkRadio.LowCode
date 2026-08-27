using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Component;

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
