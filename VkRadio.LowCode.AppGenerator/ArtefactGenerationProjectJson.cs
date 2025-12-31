using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace VkRadio.LowCode.AppGenerator;

public class ArtefactGenerationProjectJson
{
    [JsonExtensionData]
    #pragma warning disable 0649
    Dictionary<string, JToken> _additionalData;
    #pragma warning restore 0649

    [JsonProperty(Required = Required.Always)]
    public int FileFormat { get; private set; }

    [JsonProperty(Required = Required.Always)]
    public string Name { get; private set; }

    /// <summary>
    /// Project root directory full path
    /// </summary>
    [JsonIgnore]
    public string ProjectBasePath { get; private set; }

    /// <summary>
    /// Domain model
    /// </summary>
    [JsonIgnore]
    public MetaModel.MetaModel DomainModel { get; private set; }

    /// <summary>
    /// Artefact generation targets (list of trees)
    /// </summary>
    public List<ArtefactGenerationTargetJson> Targets { get; private set; } = new List<ArtefactGenerationTargetJson>();

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        ProjectBasePath = (string)context.Context;
        var domainModelFile = (string)_additionalData["domainModelFile"];
        if (!Path.IsPathRooted(domainModelFile))
            domainModelFile = Path.Combine(ProjectBasePath, domainModelFile);
        DomainModel = MetaModel.MetaModel.Load(domainModelFile);
        foreach (var tgt in Targets)
            tgt.AfterDeserialize(this, null);
    }
};
