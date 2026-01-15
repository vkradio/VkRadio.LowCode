using Ardalis.GuardClauses;
using System.Xml.Linq;
using VkRadio.LowCode.AppGenerator.Targets;

namespace VkRadio.LowCode.AppGenerator.Repositories;

public class ArtefactGenerationProjectRepository
{
    #region Internals
    private readonly Dictionary<ArtefactTypeCodeEnum, ITargetRepository> _targetRepositories = new()
    {
        [ArtefactTypeCodeEnum.CSharp] = null!
    };

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

    private string LoadName(XElement containingXel, string parentElementName)
    {
        Guard.Against.Null(containingXel, nameof(containingXel));
        Guard.Against.NullOrEmpty(parentElementName);

        var xel = containingXel.Element("Name") ?? throw new ApplicationException($"Element {parentElementName}.Name not exists");

        if (string.IsNullOrWhiteSpace(xel.Value))
        {
            throw new ApplicationException("Name value required");
        }

        return xel.Value.Trim();
    }

    private async Task<ArtefactGenerationTarget> LoadTarget(XElement containingXel, string parentElementName)
    {
        Guard.Against.Null(containingXel, nameof(containingXel));
        Guard.Against.NullOrEmpty(parentElementName);

        var id = LoadId(containingXel, parentElementName);
        var name = LoadName(containingXel, parentElementName);

        var xel = containingXel.Element("ArtefactType") ?? throw new ApplicationException($"Element {parentElementName}.ArtefactType not exists for Target {id}");

        if (!Enum.TryParse<ArtefactTypeCodeEnum>(xel.Value.Trim(), true, out var targetType))
        {
            throw new ApplicationException($"Unsupported ArtefactType value: {xel.Value!.Trim()} for Target {id}");
        }

        if (!_targetRepositories.TryGetValue(targetType, out var repo))
        {
            throw new ApplicationException($"Unsupported repository for ArtefactType {xel.Value!.Trim()} for Target {id}");
        }

        var target = await repo.LoadTarget(xel);

        return target;
    }
    #endregion

    public async Task<ArtefactGenerationProject> LoadFromFile(string filePath)
    {
        Guard.Against.NullOrEmpty(filePath, nameof(filePath));

        const string rootElementName = "ArtefactGenerationProject";

        var xelRoot = XElement.Load(filePath);

        if (xelRoot.Name != rootElementName)
        {
            throw new ApplicationException("XML root element is not ArtefactGenerationProject");
        }

        var id = LoadId(xelRoot, rootElementName);
        var name = LoadName(xelRoot, rootElementName);

        var xel = xelRoot.Element("MetaModelFilePath") ?? throw new ApplicationException("Element MetaModelFilePath not exists");

        if (string.IsNullOrWhiteSpace(xel.Value))
        {
            throw new ApplicationException("MetaModelFilePath value required");
        }

        var metaModelFilePath = xel.Value.Trim();

        if (!File.Exists(metaModelFilePath))
        {
            throw new ApplicationException($"File in MetaModelFilePath not exists: {metaModelFilePath}");
        }

        var xelTargets = xelRoot.Element("ArtefactGenerationTargets");
        var xelsTargets = xelTargets?.Elements("Target");
        var targetLoadingTasks = (xelsTargets ?? [])
            .Select(x => LoadTarget(x, $"{rootElementName}.ArtefactGenerationTargets.Target"))
            .ToList();
        var targets = await Task.WhenAll(targetLoadingTasks);

        var project = new ArtefactGenerationProject(id, name, targets);
        await project.InitializeAfterLoad();

        return project;
    }
}
