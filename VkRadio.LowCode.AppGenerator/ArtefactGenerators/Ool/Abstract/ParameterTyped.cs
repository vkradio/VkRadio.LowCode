namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract;

public class ParameterTyped: ParameterSimple
{
    protected string _type;

    public string Type { get { return _type; } set { _type = value; } }
}
