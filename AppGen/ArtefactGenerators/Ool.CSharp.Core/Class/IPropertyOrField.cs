using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class;

public interface IPropertyOrField
{
    string Name { get; }

    CSharpTypeBase Type { get; }
}
