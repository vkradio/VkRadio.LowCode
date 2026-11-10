using Ardalis.GuardClauses;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Class;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Constant;

public class CSClassConstant : ClassConstant
{
    public CSClassConstant(CSharpTypeBase type, bool trueConst, string name, string value, XmlComment? docComment = null, ElementVisibilityAbstract? visibility = null)
        : base(name, value, docComment, visibility ?? ElementVisibilityClassic.Private)
    {
        Type = Guard.Against.Null(type, nameof(type));
        TrueConst = trueConst;
    }

    public new XmlComment? DocComment { get => (XmlComment?)base.DocComment; set => base.DocComment = value; }

    public CSharpTypeBase Type { get; private set; }

    public bool TrueConst { get; private set; }

    protected override string GenerateTextConcrete()
    {
        return string.Format(
            "{0}{1}{2} {3} {4} = {5};",
            CSharpHelper.C_TAB,
            Visibility.Value != ElementVisibilityEnum.Private ? Visibility.ToString() + " " : string.Empty,
            TrueConst ? "const" : "static readonly",
            Type,
            Name,
            Value
        );
    }
}
