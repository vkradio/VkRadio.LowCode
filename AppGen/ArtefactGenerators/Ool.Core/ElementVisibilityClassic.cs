namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;

public class ElementVisibilityClassic : ElementVisibilityAbstract
{
    public static ElementVisibilityClassic Private;

    public static ElementVisibilityClassic Protected;

    public static ElementVisibilityClassic Public;

    static ElementVisibilityClassic()
    {
        Private = new() { Value = ElementVisibilityEnum.Private };

        Protected = new() { Value = ElementVisibilityEnum.Protected };

        Public = new() { Value = ElementVisibilityEnum.Public };
    }

    public override string ToString()
    {
        var result = _value switch
        {
            ElementVisibilityEnum.Private => "private",

            ElementVisibilityEnum.Protected => "protected",

            ElementVisibilityEnum.Public => "public",

            _ => throw new ApplicationException($"{nameof(ElementVisibilityEnum)} value not supported: {_value}.")
        };

        return result;
    }
}
