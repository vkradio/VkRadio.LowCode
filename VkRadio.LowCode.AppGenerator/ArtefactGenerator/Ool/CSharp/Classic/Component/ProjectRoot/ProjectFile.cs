using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Component;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Component.ProjectRoot
{
    public abstract class ProjectFile: ComponentWPredefinedCode
    {
        public ProjectFile(CSharpProjectAbstract in_project)
        {
            Package = in_project;
            _emitUtf8Bom = true;
            Name = in_project.RootNamespace + ".csproj";
        }
    };
}
