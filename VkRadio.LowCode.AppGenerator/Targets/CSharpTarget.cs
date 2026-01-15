namespace VkRadio.LowCode.AppGenerator.Targets;

public class CSharpTarget : ArtefactGenerationTarget
{
    public MsSqlTarget DependencyTargetMsSql { get; private set; } = default!;

    public CSharpTarget(
        Guid id,
        string? outputPath
    )
        : base(id, outputPath ?? throw new ArgumentNullException(nameof(outputPath)))
    {
    }

    public override Task InitializeAfterLoad()
    {
        base.InitializeAfterLoad();

        DependencyTargetMsSql = Project
            .Targets
            .FirstOrDefault(x => x.Type == ArtefactTypeCodeEnum.MsSql) as MsSqlTarget
                ?? throw new ApplicationException($"{ArtefactTypeCodeEnum.MsSql} dependency Target not found for {Type} Target Id {Id}");

        return Task.CompletedTask;
    }
}
