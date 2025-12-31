using System.Xml.Linq;
using MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator.MetaModel.DOTDefinition;

/// <summary>
/// Data object type definition (DOT)
/// </summary>
public class DOTDefinition: IUniqueNamed
{
    Guid _id;
    Dictionary<HumanLanguageEnum, string> _names;
    MetaModel _metaModel;
    Dictionary<Guid, PropertyDefinition.PropertyDefinition> _propertyDefinitions;
    List<PredefinedDO.PredefinedDO> _predefinedDOs;

    /// <summary>
    /// Unique identifier of DOT
    /// </summary>
    public Guid Id { get { return _id; } set { _id = value; } }
    /// <summary>
    /// Dictionary of names of DOT
    /// </summary>
    public IDictionary<HumanLanguageEnum, string> Names { get { return _names; } }
    /// <summary>
    /// MetaModel
    /// </summary>
    public MetaModel MetaModel { get { return _metaModel; } }
    /// <summary>
    /// Predefined objects of DOT
    /// </summary>
    public IList<PredefinedDO.PredefinedDO> PredefinedDOs
    {
        get
        {
            if (_predefinedDOs == null)
            {
                _predefinedDOs = new List<PredefinedDO.PredefinedDO>();

                foreach (var pdo in _metaModel.AllPredefinedDOs.Values)
                {
                    if (pdo.DOTDefinition.Id == _id)
                    {
                        _predefinedDOs.Add(pdo);
                    }
                }
            }

            return _predefinedDOs;
        }
    }
    /// <summary>
    /// Definitions of properties
    /// </summary>
    public IDictionary<Guid, PropertyDefinition.PropertyDefinition> PropertyDefinitions { get { return _propertyDefinitions; } }

    /// <summary>
    /// Loading DOT from XML node
    /// </summary>
    /// <param name="metaModel">MetaModel</param>
    /// <param name="xel">XML node containing DOT definition</param>
    /// <returns>DOT definition</returns>
    public static DOTDefinition LoadFromXElement(MetaModel metaModel, XElement xel)
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
            var pd = PropertyDefinition.PropertyDefinition.LoadFromXElement(xelPropDef, metaModel);
            propDefs.Add(pd.Id, pd);
        }

        // 3. Create DOT from loaded properties
        var dotDef = new DOTDefinition()
        {
            _id = id,
            _names = names,
            _metaModel = metaModel,
            _propertyDefinitions = propDefs
        };

        // 4. Wire property definitions to their owner
        foreach (var pd in propDefs.Values)
        {
            pd.OwnerDefinition = dotDef;
        }

        return dotDef;
    }
}
