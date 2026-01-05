using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Class;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Field;

/// <summary>
/// C# class field
/// </summary>
public class CSClassField : ClassField
{
    public string TypeKeyword { get; set; }

    protected override string GenerateTextConcrete()
    {
        var addKeywords = string.Empty;

        if (_isStatic)
        {
            addKeywords += "static ";
        }

        return string.Format(
            "    {0}{1}{2} {3}{4};",
            (_visibility.Value != ElementVisibilityEnum.Private ? _visibility.ToString() + " " : string.Empty),
            addKeywords,
            TypeKeyword,
            _name,
            string.IsNullOrEmpty(_initialValue) ? string.Empty : " = " + _initialValue
        );
    }
}
