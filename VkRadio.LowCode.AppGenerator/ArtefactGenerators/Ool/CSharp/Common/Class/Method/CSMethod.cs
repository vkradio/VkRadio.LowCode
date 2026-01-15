using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract;
using AbstClassNS = VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Class;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Method;

/// <summary>
/// C# class method
/// </summary>
public class CSMethod : AbstClassNS.Method
{
    protected virtual string GenerateMethodParamsString()
    {
        var methodParams = new string[_params.Count];
        var j = 0;

        foreach (var eachParam in _params)
        {
            var par = (CSParameter)eachParam.Value;
            methodParams[j] = par.ToString();
            j++;
        }

        return string.Join(", ", methodParams);
    }

    protected virtual string GenerateMethodNameString()
    {
        var methodParams = GenerateMethodParamsString();

        var addKeywords = string.Empty;

        if (_isStatic)
        {
            addKeywords += "static ";
        }

        if (AdditionalKeywords is not null)
        {
            addKeywords += AdditionalKeywords + " ";
        }

        var methodNameString = string.Format(
            "    {0}{1}{2} {3}({4})",
            (_visibility.Value != ElementVisibilityEnum.Private ? _visibility.ToString() + " " : string.Empty),
            addKeywords,
            ReturnType,
            _name,
            methodParams
        );

        return methodNameString;
    }

    protected override string[] GenerateTextConcrete()
    {
        var text = new List<string>();

        var methodNameString = GenerateMethodNameString();
        
        if (!_hintSingleLineBody)
        {
            text.Add(methodNameString);
            text.Add("    {");

            foreach (var str in _bodyStrings)
            {
                text.Add("        " + str);
            }

            text.Add("    }");
        }
        else
        {
            text.Add(methodNameString + " => " + string.Join(" ", _bodyStrings));
        }

        return text.ToArray();
    }

    public string ReturnType { get; set; }
    public string AdditionalKeywords { get; set; }
}
