using VkRadio.LowCode.AppGenerator.ArtefactGenerators.Ool.CSharp.Classic.Package.Root;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;

public class CSharpProjectExtension : CSharpProjectAbstract
{
    public CSharpProjectExtension(CSharpSolution miniSolution, Guid projectGuid)
        : base(miniSolution, "extension", projectGuid)
    {
    }
}
