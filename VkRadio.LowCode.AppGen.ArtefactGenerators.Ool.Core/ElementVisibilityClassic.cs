namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;

public class ElementVisibilityClassic : ElementVisibilityAbstract
{
    public override string ToString()
    {
        var result = _value switch
        {
            ElementVisibilityEnum.Private => "private",
            ElementVisibilityEnum.Protected => "protected",
            ElementVisibilityEnum.Public => "public",
            _ => throw new ApplicationException($"ElementVisibilityEnum value not supported: {_value}.")
        };

        return result;
    }
    
    public static ElementVisibilityClassic Private => new() { Value = ElementVisibilityEnum.Private };

    public static ElementVisibilityClassic Protected => new() { Value = ElementVisibilityEnum.Protected };

    public static ElementVisibilityClassic Public => new() { Value = ElementVisibilityEnum.Public };
}
