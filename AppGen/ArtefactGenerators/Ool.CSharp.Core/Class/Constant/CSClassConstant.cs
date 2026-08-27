using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Class;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Constant;

public class CSClassConstant : ClassConstant
{
    string _typeKeyword;
    bool _trueConst = true;

    public CSClassConstant(string typeKeyword)
        : this(typeKeyword, ElementVisibilityClassic.Private, true)
    {
    }

    public CSClassConstant(string typeKeyword, ElementVisibilityAbstract visibility, bool trueConst)
    {
        _typeKeyword = typeKeyword;
        _visibility = visibility;
        _trueConst = trueConst;
    }

    protected override string GenerateTextConcrete()
    {
        return string.Format(
            "    {0}{1} {2} {3} = {4};",
            _visibility.Value != ElementVisibilityEnum.Private ? _visibility.ToString() + " " : string.Empty,
            _trueConst ? "const" : "static readonly",
            _typeKeyword,
            _name,
            _value
        );
    }
}
