using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Method;

public class CSConstructor : CSMethod
{
    public CSConstructor(CSClass @class) => _name = @class.Name;

    protected override string GenerateMethodNameString()
    {
        var methodParams = GenerateMethodParamsString();

        var firstWords = new List<string>();

        if (_visibility.Value != ElementVisibilityEnum.Private)
        {
            firstWords.Add(_visibility.ToString());
        }

        if (_isStatic)
        {
            firstWords.Add("static");
        }

        firstWords.Add(_name);

        var firstWordsLine = string.Join(" ", firstWords);

        var addKeywords = string.Empty;

        if (_isStatic)
        {
            addKeywords += "static ";
        }

        var methodNameString = string.Format(
            "    {0}({1}){2}",
            firstWordsLine,
            methodParams,
            BaseText != null ? (" " + BaseText) : string.Empty
        );

        return methodNameString;
    }

    public string BaseText { get; set; }
}
