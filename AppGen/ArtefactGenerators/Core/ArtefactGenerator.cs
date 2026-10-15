using Ardalis.GuardClauses;
using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Core;

/// <summary>
/// Abstract artefact generator
/// </summary>
public abstract class ArtefactGenerator
{
    protected ArtefactGenerator(ArtefactTypeEnum type, DomainModel domainModel, Target target)
    {
        Type = type;
        DomainModel = Guard.Against.Null(domainModel, nameof(domainModel));
        Target = Guard.Against.Null(target, nameof(target));
    }

    /// <summary>
    /// Generation target
    /// </summary>
    public Target Target { get; protected set; }

    /// <summary>
    /// Artefact type
    /// </summary>
    public ArtefactTypeEnum Type { get; protected set; }

    /// <summary>
    /// Domain Model
    /// </summary>
    public DomainModel DomainModel { get; protected set; }

    public abstract void InitFromTargetXElement(XElement xelTarget);

    /// <summary>
    /// Generate an artefact package
    /// </summary>
    /// <returns>null - when success, otherwise an error message</returns>
    public abstract string? Generate();
}
