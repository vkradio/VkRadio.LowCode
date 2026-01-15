using Ardalis.GuardClauses;
using System.Xml.Linq;
using VkRadio.LowCode.AppGenerator.Targets;

namespace VkRadio.LowCode.AppGenerator.Repositories;

public abstract class BaseTargetRepository : ITargetRepository
{
    private Guid LoadId(XElement containingXel, string parentElementName)
    {
        Guard.Against.Null(containingXel);
        Guard.Against.NullOrEmpty(parentElementName);

        var xel = containingXel.Element("Id") ?? throw new ApplicationException($"Element {parentElementName}.Id not exists");

        if (!Guid.TryParse(xel.Value, out var id))
        {
            throw new ApplicationException("Invalid Id value");
        }

        return id;
    }

    private ArtefactTypeCodeEnum LoadArtefactType(XElement containingXel, string parentElementName, Guid targetId)
    {
        Guard.Against.Null(containingXel, nameof(containingXel));
        Guard.Against.NullOrEmpty(parentElementName);

        var xel = containingXel.Element("ArtefactType") ?? throw new ApplicationException($"Element {parentElementName}.ArtefactType not exists for Target Id {targetId}");

        if (string.IsNullOrWhiteSpace(xel.Value))
        {
            throw new ApplicationException($"ArtefactType value required (Target Id {targetId})");
        }

        if (!Enum.TryParse<ArtefactTypeCodeEnum>(xel.Value, true, out var artefactType))
        {
            throw new ApplicationException($"Invalid ArtefactType: {xel.Value} (Target Id {targetId})");
        }

        return artefactType;
    }

    private string? LoadOutputPath(XElement containingXel)
    {
        var xel = containingXel.Element("OutputPath");
        return xel?.Value?.Trim();
    }

    protected (Guid Id, ArtefactTypeCodeEnum ArtefactType, string? OutputPath) LoadBaseValues(XElement containingXel)
    {
        const string containingElemetName = "Target";

        var id = LoadId(containingXel, containingElemetName);
        var artefactType = LoadArtefactType(containingXel, containingElemetName, id);
        var outputPath = LoadOutputPath(containingXel);

        return (id, artefactType, outputPath);
    }

    public abstract Task<ArtefactGenerationTarget> LoadTarget(XElement containingXel);

    public abstract ArtefactTypeCodeEnum ArtefactType { get; }
}
