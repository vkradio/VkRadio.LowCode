using System;

using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Component.ProjectRoot;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Gui;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Model;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Root
{
    public class CSharpProjectBase: CSharpProjectAbstract
    {
        public CSharpProjectBase(CSharpSolution in_solution, Guid in_projectGuid)
            : base(in_solution, "base", in_projectGuid)
        {
            PropertiesPackage = new PropertiesPackageBase(this);
            _subpackages.Add(PropertiesPackage.Name, PropertiesPackage);

            ModelPackage = new ModelPackage(this);
            _subpackages.Add(ModelPackage.Name, ModelPackage);

            GuiPackage = new GuiPackage(this);
            _subpackages.Add(GuiPackage.Name, GuiPackage);

            ProjectFile = new ProjectFileBase(this);
            _components.Add(ProjectFile.Name, ProjectFile);
        }

        new public CSharpSolution ParentPackage { get { return (CSharpSolution)_parentPackage; } }
        new public PropertiesPackageBase PropertiesPackage { get; private set; }
        public ModelPackage ModelPackage { get; private set; }
        public GuiPackage GuiPackage { get; private set; }
    };
}
