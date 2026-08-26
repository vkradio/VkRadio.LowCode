using VkRadio.LowCode.AppGen.Domain;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.SqlCore;

/// <summary>
/// Correspondence between a table and an Entity type definition
/// </summary>
public class TableAndEntityCorrespondence : TableAndSourceCorrespondence
{
    EntityDefinition _entityDefinition;
    List<PropertyCorrespondence> _propertyCorrespondences = [];

    /// <summary>
    /// Entity definition
    /// </summary>
    public EntityDefinition EntityDefinition { get => _entityDefinition; set => _entityDefinition = value; }

    /// <summary>
    /// Correspondence of table fields to Entity properties
    /// </summary>
    public IList<PropertyCorrespondence> PropertyCorrespondences => _propertyCorrespondences;
}
