using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Method;

public class CSParameter : ParameterTyped
{
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
