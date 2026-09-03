using Ool.CSharp.WinFormsApp.Package.Root;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Root;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;

public class CSharpProjectLauncher : CSharpProjectAbstract
{
    public CSharpProjectLauncher(CSharpSolution miniSolution, Guid projectGuid)
        : base(miniSolution, "launcher", projectGuid)
    {
    }
}
