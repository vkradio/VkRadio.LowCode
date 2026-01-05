namespace VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;

/// <summary>
/// What to do when the object, having a Foreign Key to this object, is being deleted
/// </summary>
public enum OnDeleteActionEnum
{
    /// <summary>
    /// Do nothing, leave a &quot;gap&quot;
    /// </summary>
    Ingnore,
    /// <summary>
    /// Deletion is impossible
    /// </summary>
    CannotDelete,
    /// <summary>
    /// Cascade delete
    /// </summary>
    Delete,
    /// <summary>
    /// Set the Foreign Key values to NULL
    /// </summary>
    ResetToNull,
    /// <summary>
    /// Set the Foreign Key values to a default value
    /// </summary>
    ResetToDefault
}
