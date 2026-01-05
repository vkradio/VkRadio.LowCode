namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract;

public class ElementVisibilityClassic : ElementVisibilityAbstract
{
    public override string ToString()
    {
        string result = _value switch
        {
            ElementVisibilityEnum.Private => "private",
            ElementVisibilityEnum.Protected => "protected",
            ElementVisibilityEnum.Public => "public",
            _ => throw new ApplicationException(string.Format("ElementVisibilityEnum value not supported: {0}.", _value.ToString())),
        };

        return result;
    }
    
    public static ElementVisibilityClassic Private { get { return new ElementVisibilityClassic() { Value = ElementVisibilityEnum.Private }; } }
    public static ElementVisibilityClassic Protected { get { return new ElementVisibilityClassic() { Value = ElementVisibilityEnum.Protected }; } }
    public static ElementVisibilityClassic Public { get { return new ElementVisibilityClassic() { Value = ElementVisibilityEnum.Public }; } }
}
