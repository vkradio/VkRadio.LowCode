using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Method;

public class CSConstructor : CSMethod
{
    public CSConstructor(
        CSClass cSharpClass,
        XmlComment? docComment = null,
        ElementVisibilityAbstract? visibility = null,
        string? baseText = null,
        IEnumerable<string>? bodyStrings = null,
        IEnumerable<ParameterSimple>? parameters = null,
        bool isStatic = false,
        bool hintSingleLine = false
    ) : base(
        cSharpClass,
        cSharpClass.Name,
        docComment,
        visibility,
        null,
        null,
        bodyStrings,
        parameters,
        isStatic,
        hintSingleLine
    )
    {
        BaseText = baseText;
    }

    protected override string GenerateMethodNameString()
    {
        var methodParams = GenerateMethodParamsString();

        var firstWords = new List<string>();

        if (Visibility.Value != ElementVisibilityEnum.Private)
        {
            firstWords.Add(Visibility.ToString());
        }

        if (IsStatic)
        {
            firstWords.Add("static");
        }

        firstWords.Add(Name);

        var firstWordsLine = string.Join(" ", firstWords);

        var addKeywords = string.Empty;

        if (IsStatic)
        {
            addKeywords += "static ";
        }

        var methodNameString = string.Format(
            "{0}{1}({2}){3}",
            CSharpHelper.C_TAB,
            firstWordsLine,
            methodParams,
            string.IsNullOrEmpty(BaseText) ? string.Empty : (" " + BaseText)
        );

        return methodNameString;
    }

    protected override string[] GenerateTextConcrete()
    {
        var classBodyText = base.GenerateTextConcrete();

        if (IsStatic && Class.Constants.Values.Any(x => !x.TrueConst))
        {
            if (HintSingleLineBody)
            {
                throw new InvalidOperationException("Unable to add initialization for static class fields to a single-line static constructor");
            }

            var lines = classBodyText
                .Take(classBodyText.Length - 1)
                .ToList();

            if (lines.Count != 1)
            {
                lines.Add(string.Empty);
            }

            lines.AddRange(Class
                .Constants
                .Values
                .Where(x => !x.TrueConst)
                .Select(x => $"{CSharpHelper.C_TAB}{CSharpHelper.C_TAB}{x.Name} = {x.Value};")
            );

            lines.Add($"{CSharpHelper.C_TAB}}}");

            return lines.ToArray();
        }
        else
        {
            return classBodyText;
        }
    }

    public string? BaseText { get; private set; }
}
