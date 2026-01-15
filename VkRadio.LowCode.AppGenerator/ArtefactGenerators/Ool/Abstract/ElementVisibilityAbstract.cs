namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract;

public abstract class ElementVisibilityAbstract
{
    protected ElementVisibilityEnum _value;

    public ElementVisibilityEnum Value { get { return _value; } set { _value = value; } }

    public new abstract string ToString();
}
