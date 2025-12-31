using System;

using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Component.ProjectRoot;
using PackNS = ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Package;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package
{
    public abstract class CSharpProjectAbstract: PackNS.Package
    {
        protected Guid _projectGuid;
        protected ProjectFile _projectFile;
        protected PropertiesPackageAbstract _propertiesPackage;
        protected string _rootNamespace;

        public CSharpProjectAbstract(CSharpSolution in_miniSolution, string in_name, Guid in_projectGuid)
            : base(in_miniSolution, in_name)
        {
            _projectGuid = in_projectGuid;
            _rootNamespace = $"{NameHelper.NameToUnderscoreSeparatedName(in_miniSolution.DomainModel.Names)}_{_name}";
        }

        public Guid ProjectGuid { get { return _projectGuid; } }
        public ProjectFile ProjectFile { get { return _projectFile; } }
        public PropertiesPackageAbstract PropertiesPackage { get { return _propertiesPackage; } }
        public string RootNamespace { get { return _rootNamespace; } }
    };
}
