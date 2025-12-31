using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

using ag = ArtefactGenerationProject.ArtefactGenerator;
using ArtefactGenerationProject.Deserialization;

namespace VkRadio.LowCode.AppGenerator;

[JsonConverter(typeof(ArtefactGeneratorTargetJsonConverter))]
public abstract class ArtefactGenerationTargetJson
{
    protected ArtefactGenerationProjectJson _project;
    protected ArtefactGenerationTargetJson _parent;

    [JsonIgnore]
    public virtual ArtefactGenerationProjectJson Project { get => _project; protected set => _project = value; }

    [JsonIgnore]
    public virtual ArtefactGenerationTargetJson Parent { get => _parent; protected set => _parent = value; }

    [JsonProperty]
    public string OutputPath { get; protected set; }

    [JsonProperty]
    public List<ArtefactGenerationTargetJson> Subtargets { get; protected set; } = new List<ArtefactGenerationTargetJson>();

    /// <summary>
    /// Additional actions in inherited classes after project and parent target are linked
    /// </summary>
    protected virtual void AfterDeserializeConcreteRoot() { }
    /// <summary>
    /// Additional actions in inherited classes after subtargets are linked
    /// </summary>
    protected virtual void AfterDeserializeConcreteTree() { }
    /// <summary>
    /// Create concrete target generator
    /// </summary>
    protected abstract void CreateGenerator();

    /// <summary>
    /// Initializing object state after deserialize
    /// </summary>
    /// <param name="project">Project</param>
    /// <param name="parent">Upper level target</param>
    public void AfterDeserialize(ArtefactGenerationProjectJson project, ArtefactGenerationTargetJson parent)
    {
        Project = project;
        Parent = parent;
        if (!string.IsNullOrEmpty(OutputPath) && !Path.IsPathRooted(OutputPath))
            OutputPath = Path.Combine(Project.ProjectBasePath, OutputPath);
        AfterDeserializeConcreteRoot();
        foreach (ArtefactGenerationTargetJson tgt in Subtargets)
            tgt.AfterDeserialize(project, this);
        AfterDeserializeConcreteTree();
        CreateGenerator();
    }

    /// <summary>
    /// Generate target artefacts
    /// </summary>
    public void GenerateArtefacts()
    {
        foreach (var subtarget in Subtargets)
            subtarget.GenerateArtefacts();
        Generator.Generate();
    }

    /// <summary>
    /// Target generator
    /// </summary>
    [JsonIgnore]
    public ag.ArtefactGeneratorJson Generator { get; protected set; }
};
