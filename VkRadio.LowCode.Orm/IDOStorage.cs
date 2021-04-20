using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace VkRadio.LowCode.Orm
{
    /// <summary>
    /// Data Object Storage interface
    /// </summary>
    public interface IDOStorage
    {
        #region CREATE
        DbMappedDOT CreateNew();

        // Also see Flush() which is used in both CREATE and UPDATE scenarios
        #endregion

        #region READ
        DbMappedDOT? Restore(Guid id, DbTransaction? transaction = null);

        DataTable ReadAsTable(FilterAbstract? filter = null, DbParameter[]? parameters = null);

        void RefreshTable(FilterAbstract? filter, DataTable table);

        void ResetCache();

        IDictionary<Guid, DbMappedDOT> PredefinedObjects { get; }
        #endregion

        #region UPDATE
        // Flush also used in CREATE scenarios
        string? Flush(DbMappedDOT dataObject, DbTransaction? transaction = null);
        #endregion

        #region DELETE
        string? Delete(DbMappedDOT dataObject, DbTransaction? transaction = null);

        string? Delete(Guid id, DbTransaction? transaction = null);

        string? Delete(Guid id, bool throwZeroRowsAffectedException);

        string? GetMessageCannotDeletePredefinedObject();
        #endregion

        void Init();
    }
}
