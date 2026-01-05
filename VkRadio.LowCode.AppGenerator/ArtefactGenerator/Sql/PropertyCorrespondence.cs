using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql;

/// <summary>
/// Mutual correspondence of table field and data object type property
/// </summary>
public class PropertyCorrespondence
{
    ITableField _tableField;
    TableAndDOTCorrespondence _tableAndDOTCorrespondence;
    PropertyDefinition _propertyDefinition;

    /// <summary>
    /// Table field
    /// </summary>
    public ITableField TableField { get { return _tableField; } set { _tableField = value; } }
    /// <summary>
    /// Mutual correspondence of data object type definition and a table
    /// </summary>
    public TableAndDOTCorrespondence TableAndDOTCorrespondence { get { return _tableAndDOTCorrespondence; } set { _tableAndDOTCorrespondence = value; } }
    /// <summary>
    /// Property definition in DOT
    /// </summary>
    public PropertyDefinition PropertyDefinition { get { return _propertyDefinition; } set { _propertyDefinition = value; } }
}
