using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Root;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;

public class CSharpProjectExtension : CSharpProjectAbstract
{
    public CSharpProjectExtension(CSharpSolution miniSolution, Guid projectGuid)
        : base(miniSolution, "extension", projectGuid)
    {
    }
}
