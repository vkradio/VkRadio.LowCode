namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql;

/// <summary>
/// Helpers for work with database schema
/// </summary>
public static class DBSchemaHelper
{
    /// <summary>
    /// Name quote symbol
    /// </summary>
    public const string C_QUOTE_SYMBOL = "`";
    /// <summary>
    /// Keyword &quot;default&quot;
    /// </summary>
    public const string C_KEYWORD_DEFAULT = "default";
    /// <summary>
    /// Keyword &quot;unique&quot;
    /// </summary>
    public const string C_KEYWORD_UNIQUE = "unique";
    /// <summary>
    /// Number of tabulation spaces
    /// </summary>
    public const int C_TAB_LEN = 4;
    /// <summary>
    /// Tabulation string
    /// </summary>
    public const string C_TAB = "    ";

    /// <summary>
    /// Translate GUID value to a binary(16) for use in MySQL
    /// </summary>
    /// <param name="guid"></param>
    /// <returns>String representation of GUID</returns>
    public static string GuidToMySqlValueString(Guid guid) => "x'" + GuidToHexString(guid) + "'";

    /// <summary>
    /// Translate GUID value to a uniqueidentifier value for use in MS SQL
    /// </summary>
    /// <param name="guid"></param>
    /// <returns>String representation of GUID</returns>
    public static string GuidToMsSqlValueString(Guid guid) => "'" + guid.ToString("D") + "'";

    public static string GuidToHexString(Guid guid)
    {
        var guidBytes = guid.ToByteArray();
        var guidString = string.Empty;

        foreach (byte b in guidBytes)
        {
            guidString += b.ToString("X2");
        }

        return guidString;
    }
}
