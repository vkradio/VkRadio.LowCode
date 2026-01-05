using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Component.ProjectRoot;
using PackNS = VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Package;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package;

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
