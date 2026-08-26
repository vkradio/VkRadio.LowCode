namespace VkRadio.LowCode.AppGen.ArtefactGenerators.OolCore;

public abstract class ElementVisibilityAbstract
{
    protected ElementVisibilityEnum _value;

    public ElementVisibilityEnum Value { get => _value; set => _value = value; }

    public new abstract string ToString();
}
