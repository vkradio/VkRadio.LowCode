namespace VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;

/// <summary>
/// Table field interface
/// </summary>
public interface ITableField : ITextDefinition
{
    /// <summary>
    /// Field name
    /// </summary>
    string Name { get; }
    /// <summary>
    /// Are NULL values allowed
    /// </summary>
    bool Nullable { get; }
    /// <summary>
    /// SQL type (string literal acceptable for a particular SQL dialect)
    /// </summary>
    string SqlType { get; }
    /// <summary>
    /// Table that owns this field
    /// </summary>
    Table Table { get; }
    /// <summary>
    /// Correspondence to a data object type property (may be missing in case of surrogate keys, for example)
    /// </summary>
    PropertyCorrespondence DOTPropertyCorrespondence { get; }
    /// <summary>
    /// Uniqueness of field values throughout the table
    /// </summary>
    bool Unique { get; }
}
