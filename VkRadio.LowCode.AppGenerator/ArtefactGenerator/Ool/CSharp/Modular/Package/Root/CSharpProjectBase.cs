using System;

using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular.Component.ProjectRoot;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular.Package.Model;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular.Package.Root
{
    public class CSharpProjectBase: CSharpProjectAbstract
    {
        public CSharpProjectBase(CSharpSolution in_solution, Guid in_projectGuid)
            : base(in_solution, "base", in_projectGuid)
        {
            ModelPackage = new ModelPackage(this);
            _subpackages.Add(ModelPackage.Name, ModelPackage);

            ProjectFile = new ProjectFileBase(this);
            _components.Add(ProjectFile.Name, ProjectFile);
        }

        new public CSharpSolution ParentPackage { get { return (CSharpSolution)_parentPackage; } }
        public ModelPackage ModelPackage { get; private set; }
    };
}
