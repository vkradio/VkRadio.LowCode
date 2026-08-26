using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.Names.Translit.RU;

namespace VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;

/// <summary>
/// Predefined data object
/// </summary>
public class PredefinedEntityInstance : IUniqueNamed
{
    Guid _id;
    DomainModel _domainModel;
    EntityDefinition _entityDefinition;
    Dictionary<Guid, IPropertyValue> _propertyValues;
    Dictionary<NaturalLanguageEnum, string> _names;

    /// <summary>
    /// Unique identifier of a predefined object
    /// </summary>
    public Guid Id { get => _id; set => _id = value; }

    /// <summary>
    /// DomainModel
    /// </summary>
    public DomainModel MetaModel => _domainModel;
    /// <summary>
    /// Data objec type definition
    /// </summary>
    public EntityDefinition DOTDefinition => _entityDefinition;

    /// <summary>
    /// Values of properties
    /// </summary>
    public IDictionary<Guid, IPropertyValue> PropertyValues => _propertyValues;

    /// <summary>
    /// Name dictionary of a predefined data object
    /// </summary>
    public IDictionary<NaturalLanguageEnum, string> Names => _names;

    /// <summary>
    /// Loading of a predefined data object from an XML node
    /// </summary>
    /// <param name="domainModel">MetaModel</param>
    /// <param name="xelSource">XML node</param>
    /// <returns>Predefined object</returns>
    public static PredefinedEntityInstance LoadFromXElement(DomainModel domainModel, XElement xelSource)
    {
        // 1. Read an Id of a predefined object
        var id = new Guid(xelSource.Element("Id")!.Value);

        // 2. Wire with a definition of data object type
        var dotDefinitionId = new Guid(xelSource.Element("DOTDefinitionId")!.Value);
        EntityDefinition dotDefinition = domainModel.AllEntityDefinitions[dotDefinitionId];

        // 3. Load predefined names, if they are set (othewise get them from properties)
        var names = NameDictionary.LoadNamesFromContainingXElement(xelSource);
        var searchNameProp = names.Count == 0;

        // 4. Load values of properties, and set default values
        var propertyValues = new Dictionary<Guid, IPropertyValue>();
        var xelValues = xelSource.Element("PropertyValues")!;

        foreach (var propDef in dotDefinition.PropertyDefinitions.Values)
        {
            // Search for XML node that contain a value of a property
            var xelPropValue = xelValues
                .Elements("PropertyValue")
                .Select(x => new
                {
                    PropValue = x,
                    PropDefId = new Guid(x.Element("PropertyDefinitionId")!.Value)
                })
                .Where(x => x.PropDefId == propDef.Id)
                .Select(x => x.PropValue)
                .FirstOrDefault();

            // If an XML node with a property value found, load it, othewise use a default value from a property definition
            var propertyValue = propDef.FunctionalType.CreatePropertyValue();

            if (xelPropValue is not null)
            {
                var xelValue = xelPropValue.Element("Value")!;
                var xat = xelValue.Attribute("UseName");

                if (xat is not null && bool.Parse(xat.Value))
                {
                    if (names.Count == 0)
                    {
                        throw new ApplicationException(string.Format("UseName=\"True\" is set for PredefinedDO Id {0}, but no Name element exists.", id));
                    }

                    // TODO: Here we need a more general and flexible mechanism for switching between localized and English texts
                    propertyValue.ValueObject = names.ContainsKey(NaturalLanguageEnum.Ru)
                        ? names[NaturalLanguageEnum.Ru]
                        : names[NaturalLanguageEnum.En];
                }
                else
                {
                    propertyValue.ValueObject = propDef.FunctionalType.ParseValueFromXmlString(xelValue.Value);
                }
            }
            else
            {
                propertyValue.ValueObject = propDef.DefaultValue;
            }

            propertyValues.Add(propertyValue.Definition.Id, propertyValue);

            // If names of a predefined object are not explicitly set with a tag Name, search for a matching name among properties
            if (searchNameProp && propDef.Names[NaturalLanguageEnum.En] == "name")
            {
                var nameValue = xelPropValue!.Element("Value")!.Value;
                var lang = NameDictionary.DetectLanguage(nameValue);

                if (lang == NaturalLanguageEnum.Ru)
                {
                    names.Add(NaturalLanguageEnum.Ru, nameValue);
                    nameValue = Transliteration.Front(nameValue);
                }

                names.Add(NaturalLanguageEnum.En, nameValue);

                searchNameProp = false;
            }
        }

        // 5. If a name of a predefined object still not found, extract it from a first met property value
        if (searchNameProp)
        {
            foreach (var pval in propertyValues.Values)
            {
                var nameValue = pval.ValueObject?.ToString();

                if (string.IsNullOrWhiteSpace(nameValue))
                {
                    continue;
                }

                var lang = NameDictionary.DetectLanguage(nameValue);

                if (lang == NaturalLanguageEnum.Ru)
                {
                    nameValue = Transliteration.Front(nameValue);
                }

                names.Add(NaturalLanguageEnum.En, nameValue);

                break;
            }
        }

        // 6. Create a predefined object using all those params
        var pdo = new PredefinedEntityInstance()
        {
            _id = id,
            _entityDefinition = dotDefinition,
            _domainModel = domainModel,
            _propertyValues = propertyValues,
            _names = names
        };

        return pdo;
    }
}
