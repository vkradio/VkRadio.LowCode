using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Core;

/// <summary>
/// Abstract artefact generator
/// </summary>
public abstract class ArtefactGenerator
{
    protected Target _target;
    protected ArtefactTypeEnum _type;
    protected DomainModel _domainModel;

    protected ArtefactGenerator(ArtefactTypeEnum type, DomainModel domainModel, Target target)
    {
        _type = type;
        _domainModel = domainModel;
        _target = target;
    }

    /// <summary>
    /// Generation target
    /// </summary>
    public Target Target => _target;

    /// <summary>
    /// Artefact type
    /// </summary>
    public ArtefactTypeEnum Type => _type;

    /// <summary>
    /// Domain Model
    /// </summary>
    public DomainModel DomainModel => _domainModel;

    abstract protected void InitFromTargetXElement(XElement xelTarget);

    /// <summary>
    /// Generate an artefact package
    /// </summary>
    /// <returns>null - when success, otherwise an error message</returns>
    public abstract string? Generate();

    /// <summary>
    /// Create a concrete generator instance
    /// </summary>
    /// <param name="target">Generation target</param>
    /// <param name="type">Artefact type</param>
    /// <param name="domainModel">Domain Model</param>
    /// <param name="xelTarget">XML node that stores additional parameter for the artefact target or type</param>
    /// <returns>Artefact generator instance</returns>
    public static ArtefactGenerator CreateConcrete(Target target, ArtefactTypeEnum type, DomainModel domainModel, XElement xelTarget, Func<ArtefactTypeEnum, DomainModel, Target, ArtefactGenerator> concreteInstanceConstructor)
    {
        var generator = concreteInstanceConstructor(type, domainModel, target);

        generator.InitFromTargetXElement(xelTarget);

        return generator;
    }
}
