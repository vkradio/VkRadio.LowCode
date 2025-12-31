namespace VkRadio.LowCode.AppGenerator;

/// <summary>
/// Empty generator (for example, for composite targets)
/// </summary>
public class DummyGenerator : ArtefactGenerator.ArtefactGeneratorJson
{
    public DummyGenerator() : base(null) { }

    public override void Generate() { }
};
