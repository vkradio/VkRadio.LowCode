using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Component;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular.Package.Root;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular.Component.ProjectRoot
{
    public abstract class ProjectFile: ComponentWPredefinedCode
    {
        public ProjectFile(CSharpProjectAbstract in_project)
        {
            Package = in_project;
            Name = in_project.RootNamespace + ".csproj";
        }
    };
}
