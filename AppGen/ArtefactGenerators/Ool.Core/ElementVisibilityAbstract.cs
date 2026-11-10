namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;

public abstract class ElementVisibilityAbstract
{
    protected ElementVisibilityEnum _value;

    public ElementVisibilityEnum Value { get => _value; protected set => _value = value; }

    public new abstract string ToString();
}
