using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;
using VkRadio.Patterns.Guards;

namespace VkRadio.LowCode.AppGen.Domain;

/// <summary>
/// Data object type (DOT, or Entity) definition
/// </summary>
public class EntityDefinition : IUniqueNamed
{
    private readonly IDictionary<NaturalLanguageEnum, string> _names;

    private List<PredefinedEntityInstance>? _predefinedEntities;

    /// <summary>
    /// Unique identifier of DOT
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Dictionary of names of DOT
    /// </summary>
    public IReadOnlyDictionary<NaturalLanguageEnum, string> Names => _names.AsReadOnly();

    /// <summary>
    /// DomainModel
    /// </summary>
    public DomainModel DomainModel { get; private set; }

    /// <summary>
    /// Predefined objects of Entity type
    /// </summary>
    public IList<PredefinedEntityInstance> PredefinedEntityInstances
    {
        get
        {
            _predefinedEntities ??= DomainModel
                .AllPredefinedEntities
                .Values
                .Where(x => x.EntityDefinition.Id == Id)
                .ToList();

            return _predefinedEntities;
        }
    }

    /// <summary>
    /// Definitions of properties
    /// </summary>
    public IDictionary<Guid, PropertyDefinition.PropertyDefinition> PropertyDefinitions { get; private set; }

    /// <summary>
    /// Loading Entity type from XML node
    /// </summary>
    /// <param name="domainModel">MetaModel</param>
    /// <param name="xel">XML node containing DOT definition</param>
    public EntityDefinition(DomainModel domainModel, XElement xel)
    {
        DomainModel = GuardRule.Forbid.Null(domainModel, nameof(domainModel));

        // 1. Load IUniqueNamed properties
        Id = new Guid(xel.Element("Id")!.Value);
        _names = new Dictionary<NaturalLanguageEnum, string>(NameDictionary.LoadNamesFromContainingXElement(xel));

        // 2. Load definitions of DOT properties
        var xelPropDefs = xel.Element("PropertyDefinitions")
            ?? throw new ApplicationException(string.Format("PropertyDefinitions element not found for DOTDefinition {0}.", Id));

        PropertyDefinitions = xelPropDefs
            .Elements("PropertyDefinition")
            .Select(x => new PropertyDefinition.PropertyDefinition(x, domainModel, this))
            .ToDictionary(x => x.Id);
    }
}
