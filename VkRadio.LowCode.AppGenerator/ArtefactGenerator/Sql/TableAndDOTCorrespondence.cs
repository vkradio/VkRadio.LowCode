using VkRadio.LowCode.AppGenerator.MetaModel.DOTDefinition;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql;

/// <summary>
/// Correspondence between a table and a data object type definition
/// </summary>
public class TableAndDOTCorrespondence : TableAndSourceCorrespondence
{
    DOTDefinition _dotDefinition;
    List<PropertyCorrespondence> _propertyCorrespondences = new();

    /// <summary>
    /// Data object type definition
    /// </summary>
    public DOTDefinition DOTDefinition { get { return _dotDefinition; } set { _dotDefinition = value; } }
    /// <summary>
    /// Correspondence of table fields to DOT properties
    /// </summary>
    public IList<PropertyCorrespondence> PropertyCorrespondences => _propertyCorrespondences;
}
