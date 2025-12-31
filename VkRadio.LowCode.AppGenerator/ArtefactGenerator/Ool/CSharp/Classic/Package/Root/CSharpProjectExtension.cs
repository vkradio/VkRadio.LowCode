using System;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Root
{
    public class CSharpProjectExtension: CSharpProjectAbstract
    {
        public CSharpProjectExtension(CSharpSolution in_miniSolution, Guid in_projectGuid)
            : base(in_miniSolution, "extension", in_projectGuid)
        {
        }
    };
}
