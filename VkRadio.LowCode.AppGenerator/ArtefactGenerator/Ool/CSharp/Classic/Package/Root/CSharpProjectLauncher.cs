using System;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Root
{
    public class CSharpProjectLauncher: CSharpProjectAbstract
    {
        public CSharpProjectLauncher(CSharpSolution in_miniSolution, Guid in_projectGuid)
            : base(in_miniSolution, "launcher", in_projectGuid)
        {
        }
    };
}
