namespace VkRadio.LowCode.AppGenerator;

/// <summary>
/// Empty generator (for example, for composite targets)
/// </summary>
public class DummyGenerator : ArtefactGenerator.ArtefactGeneratorBase
{
    public DummyGenerator() : base(null) { }

    public override string? Generate() => null;
}
