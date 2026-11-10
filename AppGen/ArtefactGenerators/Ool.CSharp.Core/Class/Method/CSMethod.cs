using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;
using AbstClassNS = VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Class;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Method;

/// <summary>
/// C# class method
/// </summary>
public class CSMethod : AbstClassNS.Method
{
    public CSMethod(
        CSClass owningClass,
        string name,
        XmlComment? docComment = null,
        ElementVisibilityAbstract? visibility = null,
        CSharpTypeBase? returnType = null,
        string? additionalKeywords = null,
        IEnumerable<string>? bodyStrings = null,
        IEnumerable<ParameterSimple>? parameters = null,
        bool isStatic = false,
        bool hintSingleLine = false
    ) : base(
        owningClass,
        name,
        docComment,
        visibility,
        bodyStrings,
        parameters,
        isStatic,
        hintSingleLine
    )
    {
        ReturnType = returnType.TypeOrDefault();
        AdditionalKeywords = additionalKeywords;
    }

    protected virtual string GenerateMethodParamsString() => string.Join(", ", Params
        .Values
        .Cast<CSParameter>()
        .Select(x => x.ToString()));

    protected virtual string GenerateMethodNameString()
    {
        var methodParams = GenerateMethodParamsString();

        var addKeywords = string.Empty;

        if (IsStatic)
        {
            addKeywords += "static ";
        }

        if (AdditionalKeywords is not null)
        {
            addKeywords += AdditionalKeywords + " ";
        }

        var methodNameString = string.Format(
            "{0}{1}{2}{3} {4}({5})",
            TabChars,
            Visibility.Value != ElementVisibilityEnum.Private ? Visibility.ToString() + " " : string.Empty,
            addKeywords,
            ReturnType,
            Name,
            methodParams
        );

        return methodNameString;
    }

    public new CSClass Class { get => (CSClass)base.Class; protected set => base.Class = value; }

    public new XmlComment? DocComment { get => (XmlComment?)base.DocComment; protected set => base.DocComment = value; }

    public override string TabChars => CSharpHelper.C_TAB;

    protected override string[] GenerateTextConcrete()
    {
        var text = new List<string>();

        var methodNameString = GenerateMethodNameString();
        
        if (!HintSingleLineBody)
        {
            text.Add(methodNameString);
            text.Add(TabChars + "{");

            foreach (var str in BodyStrings)
            {
                text.Add(TabChars + TabChars + str);
            }

            text.Add(TabChars + "}");
        }
        else
        {
            text.Add(methodNameString + " => " + string.Join(" ", BodyStrings));
        }

        return [.. text];
    }

    public CSharpTypeBase ReturnType { get; private set; }

    public string? AdditionalKeywords { get; private set; }
}
