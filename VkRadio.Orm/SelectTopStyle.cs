namespace VkRadio.Orm
{
    /// <summary>
    /// How to spell query limitation
    /// </summary>
    public enum SelectTopStyle
    {
        /// <summary>
        /// MS style: select top 5 ...;
        /// </summary>
        AsMs,
        /// <summary>
        /// LIMIT style: select ... limit 5;
        /// </summary>
        AsLimit,
        /// <summary>
        /// Oracle style: select ... where ROWNUM &lt;= 5 ...;
        /// </summary>
        AsOracle
    }
}
