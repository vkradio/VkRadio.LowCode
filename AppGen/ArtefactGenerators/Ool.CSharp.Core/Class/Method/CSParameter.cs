using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Method;

public class CSParameter : ParameterTyped
{
    public CSParameter(string name, CSharpTypeBase? type = null, string? value = null)
        : base(
            name,
            TypeHelper.TypeOrDefault(type).ToString(),
            value
        )
    {
        Type = TypeHelper.TypeOrDefault(type);
    }

    public new CSharpTypeBase Type { get; protected set; }

    public override string ToString()
    {
        var result = Type + " " + Name;

        if (!string.IsNullOrEmpty(Value))
        {
            result += " = " + Value;
        }

        return result;
    }
}
