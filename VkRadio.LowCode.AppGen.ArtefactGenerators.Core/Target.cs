namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Core;

/// <summary>
/// Description of an artefact generatiopn target
/// </summary>
public class Target
{
    /// <summary>
    /// Artefact generation project
    /// </summary>
    /// 
    public Project Project { get; private set; } = default!;
    /// <summary>
    /// Parent (upper level) target
    /// </summary>
    //public ArtefactGenerationTarget ParentTarget { get; private set; }

    /// <summary>
    /// Unique identifier of a target
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Full path to a folder with generated artefacts
    /// </summary>
    public string? OutputPath { get; private set; }

    /// <summary>
    /// Type of a generated artefact
    /// </summary>
    public ArtefactTypeEnum Type { get; private set; }

    protected Target(Guid id, string? outputPath)
    {
        Id = id;
        OutputPath = outputPath;
    }

    public void WireToProject(Project project) => Project = project;

    public virtual Task InitializeAfterLoad() => Task.CompletedTask;
}
