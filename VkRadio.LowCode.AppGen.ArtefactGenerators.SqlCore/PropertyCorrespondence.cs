using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.SqlCore;

/// <summary>
/// Mutual correspondence of table field and entity type property
/// </summary>
public class PropertyCorrespondence
{
    ITableField _tableField;
    TableAndEntityCorrespondence _tableAndEntityCorrespondence;
    PropertyDefinition _propertyDefinition;

    /// <summary>
    /// Table field
    /// </summary>
    public ITableField TableField { get => _tableField; set => _tableField = value; }

    /// <summary>
    /// Mutual correspondence of Entity definition and a table
    /// </summary>
    public TableAndEntityCorrespondence TableAndEntityCorrespondence { get => _tableAndEntityCorrespondence; set => _tableAndEntityCorrespondence = value; }

    /// <summary>
    /// Property definition in Entity
    /// </summary>
    public PropertyDefinition PropertyDefinition { get => _propertyDefinition; set => _propertyDefinition = value; }
}
