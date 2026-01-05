using System.Xml.Linq;
using VkRadio.LowCode.AppGenerator.MetaModel.Names;
using VkRadio.LowCode.AppGenerator.MetaModel.PredefinedDO;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.SystemFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator.MetaModel;

/// <summary>
/// MetaModel (synonim: problem space)
/// </summary>
public class MetaModel : INamed
{
    Dictionary<HumanLanguageEnum, string> _names;
    //int _engineFeaturesetRev = 0;
    //IDictionary<int, EngineFeaturesetRevDesc> _aboveEngineFeaturesetRevs;
    Dictionary<Guid, PredefinedDO.PredefinedDO> _allPredefinedDOs;
    Dictionary<Guid, DOTDefinition.DOTDefinition> _allDOTDefinitions;
    Dictionary<Guid, RegisterDefinition.RegisterDefinition> _allRegisterDefinitions;
    Dictionary<Guid, Relationship.Relationship> _allRelationships;
    Dictionary<Guid, PropertyDefinition.PropertyDefinition> _allPropertyDefinitions;
    //Dictionary<Guid, RegisterDefinition.RegisterValueDefinition> _allRegisterValueDefinitions;

    /// <summary>
    /// Private MetaModel constructor
    /// </summary>
    MetaModel()
    {
        _allPredefinedDOs = [];
        _allDOTDefinitions = [];
        _allRegisterDefinitions = [];
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

            // Load data object type definitions and definitions of their properties
            foreach (var xel in xelPackageRoots.Values)
            {
                LoadDOTDefinitions(xel);
            }

            //// Load definitions of registers, their keys, values, sources
            //foreach (XElement xel in xelPackageRoots.Values)
            //{
            //    LoadRegisterDefinitions(xel);
            //}

            // Load references. References for data object types are being set immediately, and for corresponding
            // properties the links are being set
            foreach (XElement xel in xelPackageRoots.Values)
            {
                LoadRelationships(xel);
            }

            // Load predefined objects
            foreach (XElement xel in xelPackageRoots.Values)
            {
                LoadPredefinedDOs(xel);
            }
        }
        else
        {
            // Load data object type definitions and definitions of their properties
            LoadDOTDefinitions(xelRoot);

            //// Load definitions of registers, their keys, values, sources
            //LoadRegisterDefinitions(xelRoot);

            // Load references. References for data object types are being set immediately, and for corresponding
            // properties the links are being set
            LoadRelationships(xelRoot);

            // Load predefined objects
            LoadPredefinedDOs(xelRoot);
        }

        _names = NameDictionary.LoadNamesFromContainingXElement(xelRoot);

        // Execute delayed linking of default values of functional types in form of links to data object types
        ChangePredefinedDOGuidsToRefsInDefaults();

        // Execute delayed linking of reference value of predefined objects
        ChangePredefinedDOGuidsToRefsInPredefinedDOPropValues();

        // Checking definitions of properties of data object types and register values of reference types for reference integrity
        CheckRefPropertyIntegrity();
    }

    /// <summary>
    /// Load data object type definitions
    /// </summary>
    /// <param name="xelRoot">Root XML element of MetaModel file</param>
    private void LoadDOTDefinitions(XElement xelRoot)
    {
        var xelDOTDefinitions = xelRoot.Element("DOTDefinitions")
            ?? throw new ApplicationException("Element DOTDefinitions not found in metamodel.");

        foreach (var xelDOTDefinition in xelDOTDefinitions.Elements("DOTDefinition"))
        {
            var dotDef = DOTDefinition.DOTDefinition.LoadFromXElement(this, xelDOTDefinition);
            _allDOTDefinitions.Add(dotDef.Id, dotDef);
        }
    }

    ///// <summary>
    ///// Load definitions of registers
    ///// </summary>
    ///// <param name="xelRoot">Root XML element of MetaModel file</param>
    //private void LoadRegisterDefinitions(XElement xelRoot)
    //{
    //    var xelRegisterDefinitions = xelRoot.Element("RegisterDefinitions");

    //    if (xelRegisterDefinitions is not null)
    //    {
    //        foreach (var xelRegisterDefinition in xelRegisterDefinitions.Elements("RegisterDefinition"))
    //        {
    //            var registerDef = RegisterDefinition.RegisterDefinition.LoadFromXElement(this, xelRegisterDefinition);
    //            _allRegisterDefinitions.Add(registerDef.Id, registerDef);
    //        }
    //    }
    //}

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
            var dotRel = Relationship.Relationship.LoadFromXElement(this, xelRel);
            _allRelationships.Add(dotRel.Id, dotRel);
        }
    }

    /// <summary>
    /// Load predefined data objects
    /// </summary>
    /// <param name="xelRoot"></param>
    private void LoadPredefinedDOs(XElement xelRoot)
    {
        var xelDOs = xelRoot.Element("PredefinedDOs")
            ?? throw new ApplicationException("Element PredefinedDOs not found in metamodel.");

        foreach (var xelDO in xelDOs.Elements("PredefinedDO"))
        {
            var pdo = PredefinedDO.PredefinedDO.LoadFromXElement(this, xelDO);

            try
            {
                _allPredefinedDOs.Add(pdo.Id, pdo);
            }
            catch (Exception ex)
            {
                throw new UniquinessException(pdo.Id, "Non-unique predefined data object", ex);
            }
        }
    }

    /// <summary>
    /// Execute delayed linking of default values of functional types in form of links to data object types
    /// </summary>
    private void ChangePredefinedDOGuidsToRefsInDefaults()
    {
        foreach (var def in AllPropertyDefinitions.Values)
        {
            var pftLink = def.FunctionalType as PFTLink;

            if (pftLink?.DefaultValue is not null)
            {
                var refObj = (SRefObject)pftLink.DefaultValue;
                refObj.Value = AllPredefinedDOs[refObj.Key];
            }
        }
    }

    /// <summary>
    /// Execute delayed linking of reference value of predefined objects
    /// </summary>
    private void ChangePredefinedDOGuidsToRefsInPredefinedDOPropValues()
    {
        foreach (var pdo in AllPredefinedDOs.Values)
        {
            foreach (var pValue in pdo.PropertyValues.Values)
            {
                var pftLink = pValue.Definition.FunctionalType as PFTLink;

                if (pValue?.ValueObject is not null)
                {
                    var refObj = (SRefObject)pValue.ValueObject;

                    try
                    {
                        refObj.Value = AllPredefinedDOs[refObj.Key];
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
    /// MetaModel name on different natural languages
    /// </summary>
    public IDictionary<HumanLanguageEnum, string> Names { get { return _names; } }
    public bool DefaultLinksStrict { get; set; }
    /// <summary>
    /// SVN rev of an engine, that supports the MetaModel
    /// </summary>
    //public int EngineFeaturesetRev { get { return _engineFeaturesetRev; } }
    /// <summary>
    /// Descriptions of higher (newest) versions of an engine
    /// </summary>
    //public IDictionary<int, EngineFeaturesetRevDesc> AboveEngineFeaturesetRevs { get { return _aboveEngineFeaturesetRevs; } }
    /// <summary>
    /// Full dictionary of predefined data objects
    /// </summary>
    public IDictionary<Guid, PredefinedDO.PredefinedDO> AllPredefinedDOs { get { return _allPredefinedDOs; } }
    /// <summary>
    /// Full dictionary of data object type definitions (DOTs)
    /// </summary>
    public IDictionary<Guid, DOTDefinition.DOTDefinition> AllDOTDefinitions { get { return _allDOTDefinitions; } }
    /// <summary>
    /// Full dictionary of definitions of registers
    /// </summary>
    public IDictionary<Guid, RegisterDefinition.RegisterDefinition> AllRegisterDefinitions { get { return _allRegisterDefinitions; } }
    /// <summary>
    /// Full dictionary of relationships between data objects
    /// </summary>
    public IDictionary<Guid, Relationship.Relationship> AllRelationships { get { return _allRelationships; } }
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

                foreach (var dotDef in _allDOTDefinitions.Values)
                {
                    foreach (var propDef in dotDef.PropertyDefinitions.Values)
                    {
                        _allPropertyDefinitions.Add(propDef.Id, propDef);
                    }
                }
            }

            return _allPropertyDefinitions;
        }
    }

    ///// <summary>
    ///// Full dictionary of definitionas of register values
    ///// </summary>
    //public IDictionary<Guid, RegisterDefinition.RegisterValueDefinition> AllRegisterValueDefinitions
    //{
    //    get
    //    {
    //        if (_allRegisterValueDefinitions is null)
    //        {
    //            _allRegisterValueDefinitions = [];

    //            foreach (var registerDef in _allRegisterDefinitions.Values)
    //            {
    //                foreach (var valueDef in registerDef.ValueDefinitions.Values)
    //                {
    //                    _allRegisterValueDefinitions.Add(valueDef.Id, valueDef);
    //                }
    //            }
    //        }

    //        return _allRegisterValueDefinitions;
    //    }
    //}

    /// <summary>
    /// Load a new MetaModel from file
    /// </summary>
    /// <param name="in_filePath">Path to a file of MetaModel</param>
    /// <returns>Loaded MetaModel</returns>
    public static MetaModel Load(string in_filePath)
    {
        var metaModel = new MetaModel();

        metaModel.PrivateLoad(in_filePath);

        return metaModel;
    }
}
