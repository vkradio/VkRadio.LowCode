using Ardalis.GuardClauses;
using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.Names.Translit.RU;

namespace VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;

/// <summary>
/// Predefined data object
/// </summary>
public class PredefinedEntityInstance : IUniqueNamed
{
    /// <summary>
    /// Unique identifier of a predefined object
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// DomainModel
    /// </summary>
    public DomainModel DomainModel { get; private set; }

    /// <summary>
    /// Data object type (Entity) definition
    /// </summary>
    public EntityDefinition EntityDefinition { get; private set; }

    /// <summary>
    /// Values of properties
    /// </summary>
    public IDictionary<Guid, IPropertyValue> PropertyValues { get; private set; }

    /// <summary>
    /// Name dictionary of a predefined data object
    /// </summary>
    public IDictionary<NaturalLanguageEnum, string> Names { get; private set; }

    /// <summary>
    /// Loading of a predefined data object from an XML node
    /// </summary>
    /// <param name="domainModel">MetaModel</param>
    /// <param name="xelSource">XML node</param>
    public PredefinedEntityInstance(DomainModel domainModel, XElement xelSource)
    {
        Guard.Against.Null(xelSource, nameof(xelSource));

        DomainModel = Guard.Against.Null(domainModel, nameof(domainModel));

        // 1. Read an Id of a predefined object
        Id = new Guid(xelSource.Element("Id")!.Value);

        // 2. Wire with a definition of data object type
        var entityDefinitionId = new Guid(xelSource.Element("DOTDefinitionId")!.Value);
        EntityDefinition = domainModel.AllEntityDefinitions[entityDefinitionId];

        // 3. Load predefined names, if they are set (othewise get them from properties)
        Names = NameDictionary.LoadNamesFromContainingXElement(xelSource);
        var searchNameProp = Names.Count == 0;

        // 4. Load values of properties, and set default values
        PropertyValues = new Dictionary<Guid, IPropertyValue>();
        var xelValues = xelSource.Element("PropertyValues")!;

        foreach (var propDef in EntityDefinition.PropertyDefinitions.Values)
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
                    if (Names.Count == 0)
                    {
                        throw new ApplicationException(string.Format("UseName=\"True\" is set for PredefinedDO Id {0}, but no Name element exists.", Id));
                    }

                    // TODO: Here we need a more general and flexible mechanism for switching between localized and English texts
                    propertyValue.SetValueObject(Names.ContainsKey(NaturalLanguageEnum.Ru)
                        ? Names[NaturalLanguageEnum.Ru]
                        : Names[NaturalLanguageEnum.En]
                    );
                }
                else
                {
                    propertyValue.SetValueObject(propDef.FunctionalType.ParseValueFromXmlString(xelValue.Value));
                }
            }
            else
            {
                propertyValue.SetValueObject(propDef.DefaultValue);
            }

            PropertyValues.Add(propertyValue.Definition.Id, propertyValue);

            // If names of a predefined object are not explicitly set with a tag Name, search for a matching name among properties
            if (searchNameProp && propDef.Names[NaturalLanguageEnum.En] == "name")
            {
                var nameValue = xelPropValue!.Element("Value")!.Value;
                var lang = NameDictionary.DetectLanguage(nameValue);

                if (lang == NaturalLanguageEnum.Ru)
                {
                    Names.Add(NaturalLanguageEnum.Ru, nameValue);
                    nameValue = Transliteration.Front(nameValue);
                }

                Names.Add(NaturalLanguageEnum.En, nameValue);

                searchNameProp = false;
            }
        }

        // 5. If a name of a predefined object still not found, extract it from a first met property value
        if (searchNameProp)
        {
            foreach (var pval in PropertyValues.Values)
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

                Names.Add(NaturalLanguageEnum.En, nameValue);

                break;
            }
        }
    }
}
