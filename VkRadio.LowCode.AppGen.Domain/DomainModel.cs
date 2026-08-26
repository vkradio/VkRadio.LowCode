using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PredefinedEntityInstances;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.SystemFunctionalTypes;

namespace VkRadio.LowCode.AppGen.Domain;

/// <summary>
/// DomainModel
/// </summary>
public class DomainModel : INamed
{
    Dictionary<NaturalLanguageEnum, string> _names;
    Dictionary<Guid, PredefinedEntityInstance> _allPredefinedEntityInstances;
    Dictionary<Guid, EntityDefinition> _allEntityDefinitions;
    Dictionary<Guid, Relationship.Relationship> _allRelationships;
    Dictionary<Guid, PropertyDefinition.PropertyDefinition> _allPropertyDefinitions;

    /// <summary>
    /// Private MetaModel constructor
    /// </summary>
    DomainModel()
    {
        _allPredefinedEntityInstances = [];
        _allEntityDefinitions = [];
        _allRelationships = [];
    }

    #region Private methods for loading MetaModel from a file
    /// <summary>
    /// Private load of MetaModel from a file
    /// </summary>
    /// <param name="filePath">path to a MetaModel file</param>
    private void PrivateLoad(string filePath)
    {
        var xelRoot = XElement.Load(filePath);

        var xelDefaultLinks = xelRoot.Element("DefaultLinks");

        if (xelDefaultLinks is not null)
        {
            if (xelDefaultLinks.Value == "strict")
            {
                DefaultLinksStrict = true;
            }
            else if (xelDefaultLinks.Value == "free")
            {
                DefaultLinksStrict = false;
            }
            else
            {
                throw new ApplicationException(string.Format("Unsupported root metamodel DefaultLinks value: \"{0}\". Supported values: \"strict\", \"free\" (default).", xelDefaultLinks.Value ?? "<NULL>"));
            }
        }

        if (xelRoot.Name != "MetaModel")
        {
            if (xelRoot.Name != "MetaModelCompound")
            {
                throw new ApplicationException("Root metamodel element is not MetaModel or MetaModelCompound.");
            }

            // Load all files of a set of MetaModels
            var xelPackages = xelRoot.Element("Packages")
                ?? throw new ApplicationException("Packages element not found in MetaModelCompound.");

            var xelPackageRoots = new Dictionary<string, XElement>();

            foreach (var xelPackagePath in xelPackages.Elements("PackagePath"))
            {
                var fi = new FileInfo(filePath);
                var path = Path.Combine(fi.DirectoryName!, xelPackagePath.Value);
                var xelPackageRoot = XElement.Load(path);

                if (xelPackageRoot.Name != "MetaModelPackage")
                {
                    throw new ApplicationException("Root metamodel element is not MetaModelPackage.");
                }

                xelPackageRoots.Add(path, xelPackageRoot);
            }

            // Load entity definitions and definitions of their properties
            foreach (var xel in xelPackageRoots.Values)
            {
                LoadEntityDefinitions(xel);
            }

            // Load references. References for data object types are being set immediately, and for corresponding
            // properties the links are being set
            foreach (XElement xel in xelPackageRoots.Values)
            {
                LoadRelationships(xel);
            }

            // Load predefined objects
            foreach (XElement xel in xelPackageRoots.Values)
            {
                LoadPredefinedEntities(xel);
            }
        }
        else
        {
            // Load data object type definitions and definitions of their properties
            LoadEntityDefinitions(xelRoot);

            // Load references. References for data object types are being set immediately, and for corresponding
            // properties the links are being set
            LoadRelationships(xelRoot);

            // Load predefined objects
            LoadPredefinedEntities(xelRoot);
        }

        _names = NameDictionary.LoadNamesFromContainingXElement(xelRoot);

        // Execute delayed linking of default values of functional types in form of links to data object types
        ChangePredefinedEntityGuidsToRefsInDefaults();

        // Execute delayed linking of reference value of predefined objects
        ChangePredefinedEntityGuidsToRefsInPredefinedEntityPropValues();

        // Checking definitions of properties of data object types and register values of reference types for reference integrity
        CheckRefPropertyIntegrity();
    }

    /// <summary>
    /// Load entity type definitions
    /// </summary>
    /// <param name="xelRoot">Root XML element of MetaModel file</param>
    private void LoadEntityDefinitions(XElement xelRoot)
    {
        var xelEntDefinitions = xelRoot.Element("DOTDefinitions")
            ?? throw new ApplicationException("Element DOTDefinitions not found in metamodel.");

        foreach (var xelEntDefinition in xelEntDefinitions.Elements("DOTDefinition"))
        {
            var entDef = EntityDefinition.LoadFromXElement(this, xelEntDefinition);
            _allEntityDefinitions.Add(entDef.Id, entDef);
        }
    }

    /// <summary>
    /// Load relationships
    /// </summary>
    /// <param name="xelRoot">Root XML element of MetaModel file</param>
    void LoadRelationships(XElement xelRoot)
    {
        var xelRels = xelRoot.Element("Relationships")
            ?? throw new ApplicationException("Element Relationships not found in metamodel.");

        foreach (var xelRel in xelRels.Elements("Relationship"))
        {
            var entRel = Relationship.Relationship.LoadFromXElement(this, xelRel);
            _allRelationships.Add(entRel.Id, entRel);
        }
    }

    /// <summary>
    /// Load predefined entity instances
    /// </summary>
    /// <param name="xelRoot"></param>
    private void LoadPredefinedEntities(XElement xelRoot)
    {
        var xelPredefEnts = xelRoot.Element("PredefinedDOs")
            ?? throw new ApplicationException("Element PredefinedDOs not found in metamodel.");

        foreach (var xelPredefEnt in xelPredefEnts.Elements("PredefinedDO"))
        {
            var pde = PredefinedEntityInstance.LoadFromXElement(this, xelPredefEnt);

            try
            {
                _allPredefinedEntityInstances.Add(pde.Id, pde);
            }
            catch (Exception ex)
            {
                throw new UniquinessException(pde.Id, "Non-unique predefined data object", ex);
            }
        }
    }

    /// <summary>
    /// Execute delayed linking of default values of functional types in form of links to data object types
    /// </summary>
    private void ChangePredefinedEntityGuidsToRefsInDefaults()
    {
        foreach (var def in AllPropertyDefinitions.Values)
        {
            var pftLink = def.FunctionalType as PFTLink;

            if (pftLink?.DefaultValue is not null)
            {
                var refObj = (SRefObject)pftLink.DefaultValue;
                refObj.Value = AllPredefinedEntities[refObj.Key];
            }
        }
    }

    /// <summary>
    /// Execute delayed linking of reference value of predefined objects
    /// </summary>
    private void ChangePredefinedEntityGuidsToRefsInPredefinedEntityPropValues()
    {
        foreach (var pdo in AllPredefinedEntities.Values)
        {
            foreach (var pValue in pdo.PropertyValues.Values)
            {
                var pftLink = pValue.Definition.FunctionalType as PFTLink;

                if (pValue?.ValueObject is not null)
                {
                    var refObj = (SRefObject)pValue.ValueObject;

                    try
                    {
                        refObj.Value = AllPredefinedEntities[refObj.Key];
                    }
                    catch (KeyNotFoundException ex)
                    {
                        throw new UniquinessException(refObj.Key, "Deferred linking of reference values of predefined data objects", ex);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Checking definitions of properties of data object types and register values of reference types for reference integrity
    /// </summary>
    private void CheckRefPropertyIntegrity()
    {
        foreach (var propDef in AllPropertyDefinitions.Values)
        {
            if (propDef.FunctionalType is PFTLink)
            {
                var pftConnector = propDef.FunctionalType as PFTConnector;

                if (pftConnector is not null)
                {
                    if (pftConnector.RelationshipConnector is null)
                    {
                        throw new ApplicationException(string.Format("Property {0} have no RelationshipConnector object specified.", propDef.Id));
                    }

                    continue;
                }
                
                var pftReferenceValue = propDef.FunctionalType as PFTReferenceValue;

                if (pftReferenceValue is not null)
                {
                    if (pftReferenceValue.RelationshipReference is null)
                    {
                        throw new ApplicationException(string.Format("Property {0} have no RelationshipReference object specified.", propDef.Id));
                    }

                    continue;
                }

                var pftTableOwner = propDef.FunctionalType as PFTTableOwner;

                if (pftTableOwner is not null)
                {
                    if (pftTableOwner.RelationshipTable is null)
                    {
                        throw new ApplicationException(string.Format("Property {0} have no RelationshipTable object specified.", propDef.Id));
                    }

                    continue;
                }

                var pftTablePart = propDef.FunctionalType as PFTTablePart;

                if (pftTablePart is not null)
                {
                    if (pftTablePart.RelationshipTable is null)
                    {
                        throw new ApplicationException(string.Format("Property {0} have no RelationshipTable object specified.", propDef.Id));
                    }

                    continue;
                }

                var pftBackRefTable = propDef.FunctionalType as PFTBackReferencedTable;

                if (pftBackRefTable is not null)
                {
                    if (pftBackRefTable.RelationshipReference is null)
                    {
                        throw new ApplicationException(string.Format("Property {0} have no RelationshipReference object specified.", propDef.Id));
                    }

                    continue;
                }

                throw new ApplicationException(string.Format("Property {0} have unknown PFTLink type FunctionalType ({1}).", propDef.Id, propDef.FunctionalType.GetType().Name));
            }
        }
    }
    #endregion

    /// <summary>
    /// DomainModel name on different natural languages
    /// </summary>
    public IDictionary<NaturalLanguageEnum, string> Names => _names;

    public bool DefaultLinksStrict { get; set; }

    /// <summary>
    /// Full dictionary of predefined data objects
    /// </summary>
    public IDictionary<Guid, PredefinedEntityInstance> AllPredefinedEntities => _allPredefinedEntityInstances;

    /// <summary>
    /// Full dictionary of Entity definitions
    /// </summary>
    public IDictionary<Guid, EntityDefinition> AllEntityDefinitions => _allEntityDefinitions;

    /// <summary>
    /// Full dictionary of relationships between data objects
    /// </summary>
    public IDictionary<Guid, Relationship.Relationship> AllRelationships => _allRelationships;

    /// <summary>
    /// Full dictionary of definitions of properties of data object types
    /// </summary>
    public IDictionary<Guid, PropertyDefinition.PropertyDefinition> AllPropertyDefinitions
    {
        get
        {
            if (_allPropertyDefinitions is null)
            {
                _allPropertyDefinitions = [];

                foreach (var entDef in _allEntityDefinitions.Values)
                {
                    foreach (var propDef in entDef.PropertyDefinitions.Values)
                    {
                        _allPropertyDefinitions.Add(propDef.Id, propDef);
                    }
                }
            }

            return _allPropertyDefinitions;
        }
    }

    /// <summary>
    /// Load a new DomainModel from file
    /// </summary>
    /// <param name="filePath">Path to a file of DomainModel</param>
    /// <returns>Loaded DomainModel</returns>
    public static DomainModel Load(string filePath)
    {
        var metaModel = new DomainModel();

        metaModel.PrivateLoad(filePath);

        return metaModel;
    }
}
