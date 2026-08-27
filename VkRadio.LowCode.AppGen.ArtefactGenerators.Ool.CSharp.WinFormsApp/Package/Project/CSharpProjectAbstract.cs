using PackNS = VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Package;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Component.ProjectRoot;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Root;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Project;

public abstract class CSharpProjectAbstract: PackNS.Package
{
    protected Guid _projectGuid;
    protected ProjectFile _projectFile;
    protected PropertiesPackageAbstract _propertiesPackage;
    protected string _rootNamespace;

    public CSharpProjectAbstract(CSharpSolution miniSolution, string name, Guid projectGuid)
        : base(miniSolution, name)
    {
        _projectGuid = projectGuid;
        _rootNamespace = $"{NameHelper.NameToUnderscoreSeparatedName(miniSolution.DomainModel.Names)}_{_name}";
    }

    public Guid ProjectGuid { get { return _projectGuid; } }
    public ProjectFile ProjectFile { get { return _projectFile; } }
    public PropertiesPackageAbstract PropertiesPackage { get { return _propertiesPackage; } }
    public string RootNamespace { get { return _rootNamespace; } }
}
