using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Component.ProjectRoot;
using PackNS = VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Package;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;

public abstract class CSharpProjectAbstract : PackNS.Package
{
    public CSharpProjectAbstract(CSharpSolution solution, string name, Guid projectGuid)
        : base(solution, name)
    {
        ProjectGuid = projectGuid;
        RootNamespace = $"{NameHelper.NameToUnderscoreSeparatedName(solution.DomainModel.Names)}_{_name}";
    }

    public Guid ProjectGuid { get; private set; }
    public ProjectFile ProjectFile { get; protected set; }
    public PropertiesPackageAbstract PropertiesPackage { get; protected set; }
    public string RootNamespace { get; private set; }
}
