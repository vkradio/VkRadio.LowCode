using System.Xml.Linq;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;
using PD = VkRadio.LowCode.AppGen.Domain.PropertyDefinition;

namespace VkRadio.LowCode.AppGen.Domain.Relationship;

/// <summary>
/// Relationship of type &quot;reference of a dictionary value&quot;
/// </summary>
public class RelationshipReference: Relationship
{
    PD.IPropertyDefinition _ownerPropertyDefinition;
    PD.PropertyDefinition _backRefTablePropertyDefinition;
    EntityDefinition _referenceDefinition;

    /// <summary>
    /// Relationship consctructor
    /// </summary>
    /// <param name="id">Relationship Id</param>
    /// <param name="metaModel">MetaModel</param>
    public RelationshipReference(Guid id, DomainModel metaModel)
        : base(id, metaModel)
    {
    }

    /// <summary>
    /// Definition of a data object type property or a register, that contains a reference to a dictionary value
    /// </summary>
    public PD.IPropertyDefinition OwnerPropertyDefinition => _ownerPropertyDefinition;

    /// <summary>
    /// Definition of a data object type property that is an implicitly inferred from references table of objects.
    /// May be absent for ordinary relationships with dictionaries (most of relationships are of that type)
    /// </summary>
    public PD.PropertyDefinition BackRefTablePropertyDefinition => _backRefTablePropertyDefinition;

    /// <summary>
    /// Data object type definition of a dictionary
    /// </summary>
    public EntityDefinition ReferenceDefinition => _referenceDefinition;

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

        _ownerPropertyDefinition = //_domainModel.AllPropertyDefinitions.ContainsKey(ownerPropertyDefinitionId)
                                   //?
            _domainModel.AllPropertyDefinitions[ownerPropertyDefinitionId];
            //: _domainModel.AllRegisterValueDefinitions[ownerPropertyDefinitionId];

        var xel = containingXel.Element("ReferenceDefinitionId");

        if (xel is null)
        {
            var backRefTablePropertyDefinitionId = new Guid(containingXel.Element("BackRefTablePropertyDefinitionId")!.Value);
            _backRefTablePropertyDefinition = _domainModel.AllPropertyDefinitions[backRefTablePropertyDefinitionId];

            // Back referencing of a property with this relationship
            var ftBackRefTable = (PFTBackReferencedTable)_backRefTablePropertyDefinition.FunctionalType;
            ftBackRefTable.RelationshipReference = this;

            _referenceDefinition = _backRefTablePropertyDefinition.OwnerDefinition;
        }
        else
        {
            var referenceDefinitionId = new Guid(xel.Value);
            _referenceDefinition = _domainModel.AllEntityDefinitions[referenceDefinitionId];
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
