using System;

using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular.Component.ProjectRoot;
using PackNS = ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Package;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular.Package.Root
{
    public abstract class CSharpProjectAbstract: PackNS.Package
    {
        public CSharpProjectAbstract(CSharpSolution in_solution, string in_name, Guid in_projectGuid)
            : base(in_solution, in_name)
        {
            ProjectGuid = in_projectGuid;
            RootNamespace = $"{NameHelper.NameToUnderscoreSeparatedName(in_solution.DomainModel.Names)}_{_name}";
        }

        public Guid ProjectGuid { get; private set; }
        public ProjectFile ProjectFile { get; protected set; }
        public string RootNamespace { get; private set; }
    };
}
