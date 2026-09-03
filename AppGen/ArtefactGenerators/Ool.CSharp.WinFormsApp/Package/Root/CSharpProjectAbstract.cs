using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Component.ProjectRoot;
using VkRadio.LowCode.AppGen.Domain.Names;
using PackNS = VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Package;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Root;

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
