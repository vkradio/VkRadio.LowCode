using System.Xml.Linq;
using PD = VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;
using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator.MetaModel.Relationship;

/// <summary>
/// Relationship of type &quot;reference of a dictionary value&quot;
/// </summary>
public class RelationshipReference: Relationship
{
    PD.IPropertyDefinition _ownerPropertyDefinition;
    PD.PropertyDefinition _backRefTablePropertyDefinition;
    DOTDefinition.DOTDefinition _referenceDefinition;

    /// <summary>
    /// Relationship consctructor
    /// </summary>
    /// <param name="id">Relationship Id</param>
    /// <param name="metaModel">MetaModel</param>
    public RelationshipReference(Guid id, MetaModel metaModel)
        : base(id, metaModel)
    {
    }

    /// <summary>
    /// Definition of a data object type property or a register, that contains a reference to a dictionary value
    /// </summary>
    public PD.IPropertyDefinition OwnerPropertyDefinition { get { return _ownerPropertyDefinition; } }
    /// <summary>
    /// Definition of a data object type property that is an implicitly inferred from references table of objects.
    /// May be absent for ordinary relationships with dictionaries (most of relationships are of that type)
    /// </summary>
    public PD.PropertyDefinition BackRefTablePropertyDefinition { get { return _backRefTablePropertyDefinition; } }
    /// <summary>
    /// Data object type definition of a dictionary
    /// </summary>
    public DOTDefinition.DOTDefinition ReferenceDefinition { get { return _referenceDefinition; } }

    /// <summary>
    /// Reference type code
    /// </summary>
    public const string C_TYPE_CODE = "reference";

    /// <summary>
    /// Load of concrete properties of a relationship from an XML node
    /// </summary>
    /// <param name="containingXel">XML node</param>
    protected override void LoadFromXElement(XElement containingXel)
    {
        var ownerPropertyDefinitionId = new Guid(containingXel.Element("OwnerPropertyDefinitionId")!.Value);

        _ownerPropertyDefinition = //_metaModel.AllPropertyDefinitions.ContainsKey(ownerPropertyDefinitionId)
                                   //?
            _metaModel.AllPropertyDefinitions[ownerPropertyDefinitionId];
            //: _metaModel.AllRegisterValueDefinitions[ownerPropertyDefinitionId];

        var xel = containingXel.Element("ReferenceDefinitionId");

        if (xel is null)
        {
            var backRefTablePropertyDefinitionId = new Guid(containingXel.Element("BackRefTablePropertyDefinitionId")!.Value);
            _backRefTablePropertyDefinition = _metaModel.AllPropertyDefinitions[backRefTablePropertyDefinitionId];

            // Back referencing of a property with this relationship
            var ftBackRefTable = (PFTBackReferencedTable)_backRefTablePropertyDefinition.FunctionalType;
            ftBackRefTable.RelationshipReference = this;

            _referenceDefinition = _backRefTablePropertyDefinition.OwnerDefinition;
        }
        else
        {
            var referenceDefinitionId = new Guid(xel.Value);
            _referenceDefinition = _metaModel.AllDOTDefinitions[referenceDefinitionId];
        }

        // Back referencing of a property with this relationship
        var ftRefValue = (PFTReferenceValue)_ownerPropertyDefinition.FunctionalType;
        ftRefValue.RelationshipReference = this;

        // Deferred enrichment of a dictionary of property names. If some property had no name in some language, that name
        // is being extracted from a data object type definition, from that it pointing to, if that exists
        NameDictionary.EnrichNames(_ownerPropertyDefinition.Names, _referenceDefinition.Names);

        if (_backRefTablePropertyDefinition is not null) // TODO: Here after implementing of register a bug is possible, because we assume that DOT definitions are only from DOTs and not from registers
        {
            NameDictionary.EnrichNames(_backRefTablePropertyDefinition.Names, ((PD.PropertyDefinition)_ownerPropertyDefinition).OwnerDefinition.Names);
        }
    }
}
