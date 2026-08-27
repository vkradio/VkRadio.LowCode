using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Core;

/// <summary>
/// Description of an artefact generatiopn target
/// </summary>
public class Target : IUnique
{
    private readonly List<Target> _dependsOn = [];
    private readonly List<Guid> _dependsOnIds;
    private readonly List<Target> _dependants = [];

    /// <summary>
    /// Unique identifier of a target
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Full path to a folder with generated artefacts
    /// </summary>
    public string? OutputPath { get; private set; }

    /// <summary>
    /// This target depends on results of generation for other targets
    /// </summary>
    public List<Target> DependsOn => _dependsOn;

    /// <summary>
    /// Other targets that depends on generation for this target
    /// </summary>
    public List<Target> Dependants => _dependants;

    /// <summary>
    /// Artefact generation project
    /// </summary>
    public ArtefactGenerationProject Project { get; private set; } = default!;

    /// <summary>
    /// Artefact Generator
    /// </summary>
    public ArtefactGenerator ArtefactGenerator { get; private set; }

    /// <summary>
    /// Generation Succeeded
    /// </summary>
    public bool GenerationSucceeded { get; set; } = true;

    /// <summary>
    /// Parent (upper level) target
    /// </summary>
    //public Target? ParentTarget { get; private set; }

    /// <summary>
    /// Type of a generated artefact
    /// </summary>
    public ArtefactTypeEnum Type { get; private set; }

    public Guid? UseOutputPathFromTargetId { get; private set; }

    protected Target(
        Guid id,
        ArtefactTypeEnum type,
        ArtefactGenerationProject project,
        XElement xelTarget,
        string? outputPath,
        Guid? useOutputPathFromTargetId,
        IEnumerable<Guid> dependsOnIds,
        Func<ArtefactTypeEnum, DomainModel, Target, ArtefactGenerator> artefactGeneratorConstructor
    )
    {
        Id = id;
        Type = type;
        Project = project;
        ArtefactGenerator = Core.ArtefactGenerator.CreateConcrete(
            this,
            type,
            project.MetaModel,
            xelTarget,
            artefactGeneratorConstructor
        );
        OutputPath = outputPath;
        UseOutputPathFromTargetId = useOutputPathFromTargetId;
        _dependsOnIds = [.. dependsOnIds];
    }

    //public void WireToProject(ArtefactGenerationProject project) => Project = project;

    //public virtual Task InitializeAfterLoad() => Task.CompletedTask;

    /// <summary>
    /// Load target from XML node
    /// </summary>
    /// <param name = "project">Project</param>
    /// <param name="xelTarget">XML node with description of a generation target</param>
    /// <param name="artefactGeneratorConstructor">constructor for concrete ArtefactGenerator</param>
    /// <returns>Artefact generation target</returns>
    public static Target LoadFromXElement(ArtefactGenerationProject project, XElement xelTarget, Func<ArtefactTypeEnum, DomainModel, Target, ArtefactGenerator> artefactGeneratorConstructor)
    {
        var id = new Guid(xelTarget.Element("Id")!.Value);
        var xelOutputPath = xelTarget.Element("OutputPath")!;
        string? outputPath = null;

        if (xelOutputPath is not null)
        {
            outputPath = Path.Combine(project.ProjectRootPath, xelOutputPath.Value);
        }

        List<Guid> dependsOnIds = [];
        var xelDependencies = xelTarget.Element("DependsOn")!;
        Guid? useOutputPathFromTargetId = null;

        if (xelDependencies is not null)
        {
            foreach (var xelDepId in xelDependencies.Elements("TargetId"))
            {
                var dependencyId = new Guid(xelDepId.Value);
                dependsOnIds.Add(dependencyId);

                if (outputPath is null)
                {
                    var xat = xelDepId.Attribute("useOutputPath");

                    if (xat is not null && (xat.Value ?? string.Empty).Equals("True", StringComparison.InvariantCultureIgnoreCase))
                    {
                        useOutputPathFromTargetId = dependencyId;
                    }
                }
            }
        }

        var artefactTypeStr = xelTarget.Element("ArtefactType")!.Value;
        ArtefactTypeEnum? artefactType = artefactTypeStr.Parse();

        if (!artefactType.HasValue)
        {
            throw new Exception($"ArtefactType \"{artefactTypeStr}\" not parsed.");
        }

        Target target = new Target(
            id,
            artefactType.Value,
            project,
            xelTarget,
            outputPath,
            useOutputPathFromTargetId,
            dependsOnIds,
            artefactGeneratorConstructor
        );

        return target;
    }

    /// <summary>
    /// Generate target artefacts
    /// </summary>
    /// <returns>null - if success, otherwise return error message</returns>
    public string GenerateArtefacts() { return ArtefactGenerator.Generate(); }

    /// <summary>
    /// Deferred dependency linkage
    /// </summary>
    /// <param name="allTargets">All project targets</param>
    public void DeferredLinkDependencies(List<Target> allTargets)
    {
        _dependsOn.Clear();

        foreach (Guid depId in _dependsOnIds)
        {
            var dep = allTargets.First(x => x.Id == depId);
            _dependsOn.Add(dep);
            dep._dependants.Add(this);
        }

        if (OutputPath is null && UseOutputPathFromTargetId.HasValue)
        {
            OutputPath = allTargets.First(x => x.Id == UseOutputPathFromTargetId.Value).OutputPath;
        }
    }
}
