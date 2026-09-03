using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;

namespace VkRadio.LowCode.AppGen.Domain;

/// <summary>
/// Data object type definition (DOT)
/// </summary>
public class EntityDefinition : IUniqueNamed
{
    Guid _id;
    Dictionary<NaturalLanguageEnum, string> _names;
    DomainModel _domainModel;
    Dictionary<Guid, PropertyDefinition.PropertyDefinition> _propertyDefinitions;
    List<PredefinedEntityInstance> _predefinedEntities;

    /// <summary>
    /// Unique identifier of DOT
    /// </summary>
    public Guid Id { get => _id; set => _id = value; }

    /// <summary>
    /// Dictionary of names of DOT
    /// </summary>
    public IDictionary<NaturalLanguageEnum, string> Names => _names;

    /// <summary>
    /// DomainModel
    /// </summary>
    public DomainModel MetaModel => _domainModel;

    /// <summary>
    /// Predefined objects of Entity type
    /// </summary>
    public IList<PredefinedEntityInstance> PredefinedEntityInstances
    {
        get
        {
            if (_predefinedEntities is null)
            {
                _predefinedEntities = [];

                foreach (var pdo in _domainModel.AllPredefinedEntities.Values)
                {
                    if (pdo.EntityDefinition.Id == _id)
                    {
                        _predefinedEntities.Add(pdo);
                    }
                }
            }

            return _predefinedEntities;
        }
    }

    /// <summary>
    /// Definitions of properties
    /// </summary>
    public IDictionary<Guid, PropertyDefinition.PropertyDefinition> PropertyDefinitions => _propertyDefinitions;

    /// <summary>
    /// Loading Entity type from XML node
    /// </summary>
    /// <param name="domainModel">MetaModel</param>
    /// <param name="xel">XML node containing DOT definition</param>
    /// <returns>Entity definition</returns>
    public static EntityDefinition LoadFromXElement(DomainModel domainModel, XElement xel)
    {
        // 1. Load IUniqueNamed properties
        var id = new Guid(xel.Element("Id")!.Value);
        var names = NameDictionary.LoadNamesFromContainingXElement(xel);

        // 2. Load definitions of DOT properties
        var xelPropDefs = xel.Element("PropertyDefinitions");

        if (xelPropDefs is null)
        {
            throw new ApplicationException(string.Format("PropertyDefinitions element not found for DOTDefinition {0}.", id));
        }

        var propDefs = new Dictionary<Guid, PropertyDefinition.PropertyDefinition>();

        foreach (var xelPropDef in xelPropDefs.Elements("PropertyDefinition"))
        {
            var pd = PropertyDefinition.PropertyDefinition.LoadFromXElement(xelPropDef, domainModel);
            propDefs.Add(pd.Id, pd);
        }

        // 3. Create DOT from loaded properties
        var entDef = new EntityDefinition()
        {
            _id = id,
            _names = names,
            _domainModel = domainModel,
            _propertyDefinitions = propDefs
        };

        // 4. Wire property definitions to their owner
        foreach (var pd in propDefs.Values)
        {
            pd.OwnerDefinition = entDef;
        }

        return entDef;
    }
}
