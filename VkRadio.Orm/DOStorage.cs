using Ardalis.GuardClauses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace VkRadio.Orm
{
    [SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "SQL queries are generated internally and so they are secure by design")]
    public abstract class DOStorage<TStorage, TDOT>: IDOStorage
        where TStorage: DOStorage<TStorage, TDOT>
        where TDOT: DbMappedDOT, new()
    {
        protected const string c_fieldId = "id";
        protected const string c_fieldIdQ = "\"id\"";
        protected const string paramPrefix = "@in_";
        protected const string paramId = paramPrefix + c_fieldId;
        protected const string commaSeparator = ", ";

        public const string ERR_CANNOT_DELETE_PREDEFINED_OBJECT = "Unable to delete the predefined object. It is an integral part of the program.";

        protected string tableName = default!;
        protected string tableNameQ = default!;
        protected string quoteSymbol = default!;
        protected string fieldsAll = default!;
        protected string fieldsAllDecimalAware = default!;
        protected string paramsAll = default!;
        protected string updateFieldValuePairs = default!;
        protected string[] fields = default!;
        protected string[] fieldsHuman = default!;
        protected readonly IList<int> decimalFields = new List<int>();
        protected string[] parameters = default!;
        protected string sqlSelectOne = default!;
        protected string sqlSelectTable = default!;
        protected string sqlSelectCollection = default!;
        protected string sqlSelectCount = default!;
        protected string sqlInsert = default!;
        protected string sqlUpdate = default!;
        protected string sqlDelete = default!;
        protected string? defaultOrderBy;

        public DOTOverrider? dotOverrider;

        protected readonly Dictionary<Guid, DbMappedDOT> predefinedObjects = new();
        protected readonly IDbProviderFactory dbProviderFactory;

        protected DOStorage(IDbProviderFactory dbProviderFactory)
        {
            this.dbProviderFactory = dbProviderFactory;
            quoteSymbol = this.dbProviderFactory.GetQuoteSymbol();
        }

        #region CREATE
        protected virtual string GenerateSqlInsert() =>
            $"insert into {tableNameQ} ({fieldsAll})" + Environment.NewLine +
            $"values ({paramsAll});";

        /// <summary>
        /// Create and initialize new object
        /// </summary>
        /// <returns></returns>
        public virtual DbMappedDOT CreateNew()
        {
            DbMappedDOT result = new TDOT();
            result.SetDbProviderFactory(dbProviderFactory);
            result.InitNew();
            if (dotOverrider != null)
            {
                result.Overrider = (DOTOverrider)dotOverrider.Clone();
                result.Overrider.DOT = result;
            }
            return result;
        }
        #endregion

        #region READ
        protected virtual string GenerateSqlSelectOne() =>
            $"select {fieldsAll}" + Environment.NewLine +
            $"from {tableNameQ}" + Environment.NewLine +
            $"where {c_fieldIdQ} = {paramId};";

        protected virtual string GenerateSqlSelectTable() =>
            string.Format(
                CultureInfo.InvariantCulture,
                "select {0}{1}from {2}{3};",
                fieldsAllDecimalAware,
                Environment.NewLine,
                tableNameQ,
                "{0}");

        protected virtual string GenerateSqlSelectCollection() =>
            string.Format(
                CultureInfo.InvariantCulture,
                "select {0}{1}" + Environment.NewLine + // top, all fields
               " from {2}" + Environment.NewLine + // table
               "{3}" + // opt where
               "{4}" + // opt order by
               "{5}",  // opt limit

                // Values:
                "{0}", // placeholder: opt top
                fieldsAll,
                tableNameQ,
                "{1}", // placeholder: opt where
                "{2}", // placeholder: opt order by
                "{3}"  // placeholder: opt limit
            );

        protected virtual string GenerateSqlSelectCount() =>
            string.Format(
                CultureInfo.InvariantCulture,
                "select COUNT(\"id\")" + Environment.NewLine +
               " from {0}" + Environment.NewLine +
               "{1}", // opt where

               // Values:
               tableNameQ,
               "{0}" // placeholder: opt where
            );

        public abstract void FillDOFromReader(DbDataReader reader, TDOT dataObject);

        protected DbMappedDOT? Restore(DbCommand command, Guid id)
        {
            Guard.Against.Null(command, nameof(command));

            command.CommandType = CommandType.Text;
            command.CommandText = sqlSelectOne;
            command.Parameters.Add(dbProviderFactory.CreateParameter(paramId, id, typeof(Guid), false));

            using DbDataReader reader = command.ExecuteReader();
            if (reader.HasRows && reader.Read())
            {
                var dataObject = new TDOT
                {
                    Id = dbProviderFactory.GuidStyle == GuidStyle.AsMs ? (Guid)reader[0] : new Guid((byte[])reader[0])
                };
                dataObject.SetDbProviderFactory(dbProviderFactory);
                FillDOFromReader(reader, dataObject);
                dataObject.SetStorageConsistency();
                if (dotOverrider != null)
                {
                    dataObject.Overrider = (DOTOverrider)dotOverrider.Clone();
                    dataObject.Overrider.DOT = dataObject;
                }
                Cache.TryAdd(id, dataObject);
                return dataObject;
            }
            else
            {
                return null;
            }
        }

        public virtual DbMappedDOT? Restore(Guid id, DbTransaction? transaction = null)
        {
            if (transaction == null && Cache.ContainsKey(id))
                return Cache[id];

            DbConnection? conn = null;
            if (transaction == null)
            {
                conn = dbProviderFactory.CreateOpenConnection();
            }
            try
            {
                using var cmd = dbProviderFactory.CreateCommand(conn ?? transaction?.Connection!);
                if (transaction != null)
                    cmd.Transaction = transaction;
                return Restore(cmd, id);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        protected virtual void FillDataTable(FilterAbstract? filter, DataTable table, DbParameter[]? parameters = null)
        {
            var additionalSql = string.Empty;
            if (filter != null)
            {
                var where = filter.ToWhere();
                if (!string.IsNullOrEmpty(where))
                    additionalSql += Environment.NewLine + "where " + where;
                var orderBy = filter.ToOrderBy();
                if (!string.IsNullOrEmpty(orderBy))
                    additionalSql += Environment.NewLine + "order by " + orderBy;
            }

            if ((filter == null || string.IsNullOrEmpty(filter.ToOrderBy())) && defaultOrderBy != null)
                additionalSql += Environment.NewLine + "order by " + defaultOrderBy;

            using var conn = dbProviderFactory.CreateOpenConnection();
            using var cmd = dbProviderFactory.CreateCommand(conn);
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = string.Format(CultureInfo.InvariantCulture, sqlSelectTable, additionalSql);

            if ((parameters?.Length ?? 0) > 0)
            {
                foreach (var dbPar in parameters!)
                    cmd.Parameters.Add(dbProviderFactory.CreateParameter(dbPar));
            }

            using var adapter = dbProviderFactory.CreateDataAdapter();
            adapter.SelectCommand = cmd;
            adapter.Fill(table);
        }

        public virtual DataTable ReadAsTable(FilterAbstract? filter = null, DbParameter[]? parameters = null)
        {
            var table = new DataTable();
            var keyColumn = new DataColumn
            {
                ColumnName = "id",
                Unique = true,
                AllowDBNull = false,
                DataType = dbProviderFactory.GuidStyle == GuidStyle.AsMs ? typeof(Guid) : typeof(byte[])
            };
            table.Columns.Add(keyColumn);
            table.PrimaryKey = new DataColumn[] { keyColumn };

            FillDataTable(filter, table, parameters);

            return table;
        }

        public virtual void RefreshTable(FilterAbstract? filter, DataTable table) => FillDataTable(filter, table);

        [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "Changing return type to IList<T> will be a breaking change to lots of legacy code")]
        public virtual List<TDOT> ReadAsCollection(string? where = null, DbParameter[]? parameters = null, string? orderBy = null, DbTransaction? transaction = null, bool doNotUseDefaultOrder = false, int selectTop = 0)
        {
            var result = new List<TDOT>();

            DbConnection? conn = null;
            try
            {
                if (transaction == null)
                    conn = dbProviderFactory.CreateOpenConnection();
                using var cmd = dbProviderFactory.CreateCommand(transaction == null ? conn! : transaction.Connection!);
                if (transaction != null)
                    cmd.Transaction = transaction;

                var internalOrderBy = string.Empty;
                if (!string.IsNullOrEmpty(orderBy))
                    internalOrderBy = " order by " + orderBy;
                else if (!doNotUseDefaultOrder && !string.IsNullOrEmpty(defaultOrderBy))
                    internalOrderBy = " order by " + defaultOrderBy;

                var internalWhere = string.Empty;
                if (!string.IsNullOrEmpty(where))
                    internalWhere = where;
                if (selectTop > 0 && dbProviderFactory.SelectTop == SelectTopStyle.AsOracle)
                {
#pragma warning disable CA1508 // Avoid dead conditional code
                    // TODO: This is a false-positive according to https://github.com/dotnet/roslyn-analyzers/issues/3685#issuecomment-702153431
                    // We need to remove this disabler after .NET SDK 5.0.2*** will be released accroding to https://github.com/dotnet/roslyn-analyzers/branches/active
                    if (!string.IsNullOrEmpty(internalWhere))
#pragma warning restore CA1508 // Avoid dead conditional code
                        internalWhere += " and ";
                    internalWhere += $"ROWNUM <= {selectTop}";
                }
                if (!string.IsNullOrEmpty(internalWhere))
                    internalWhere = " where " + internalWhere;

                var limit = string.Empty;
                if (selectTop > 0 && dbProviderFactory.SelectTop == SelectTopStyle.AsLimit)
                    limit = $" limit {selectTop}";

                cmd.CommandText = string.Format(
                    CultureInfo.InvariantCulture,
                    sqlSelectCollection,
                    selectTop > 0 && dbProviderFactory.SelectTop == SelectTopStyle.AsMs ? $"top {selectTop} " : string.Empty,
                    internalWhere,
                    internalOrderBy,
                    limit
                );
                if ((parameters?.Length ?? 0) > 0)
                {
                    foreach (var dbPar in parameters!)
                        cmd.Parameters.Add(dbProviderFactory.CreateParameter(dbPar));
                }

                using var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TDOT o = new() { Id = reader.GetGuid(0) };
                        o.SetDbProviderFactory(dbProviderFactory);
                        FillDOFromReader(reader, o);
                        o.SetStorageConsistency();
                        if (dotOverrider != null)
                        {
                            o.Overrider = (DOTOverrider)dotOverrider.Clone();
                            o.Overrider.DOT = o;
                        }
                        Cache.AddOrUpdate(o.Id.Value, o, (key, oldValue) => o);

                        result.Add(o);
                    }
                }
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

            return result;
        }

        [SuppressMessage("Design", "CA1002:Do not expose generic lists", Justification = "Changing return type to IList<T> will be a breaking change to lots of legacy code")]
        public virtual List<TDOT> ReasAsCollectionForParent(FilterSimple parentFilter, string? additionalWhere = null, DbParameter[]? parameters = null, string? orderBy = null, DbTransaction? transaction = null, bool doNotUseDefaultOrder = false, int selectTop = 0)
        {
            Guard.Against.Null(parentFilter, nameof(parentFilter));

            var internalWhere = parentFilter.Where;
            if (!string.IsNullOrEmpty(additionalWhere))
                internalWhere += " and " + additionalWhere;

            var internalOrderBy = parentFilter.OrderBy;
            if (!string.IsNullOrEmpty(orderBy))
                internalOrderBy = orderBy;
            else if (doNotUseDefaultOrder)
                internalOrderBy = null;

            return ReadAsCollection(internalWhere, parameters, internalOrderBy, transaction, doNotUseDefaultOrder, selectTop);
        }

        public virtual int GetCount(string? where = null, DbParameter[]? parameters = null, DbTransaction? transaction = null)
        {
            DbConnection? conn = null;
            try
            {
                if (transaction == null)
                    conn = dbProviderFactory.CreateOpenConnection();
                using var cmd = dbProviderFactory.CreateCommand(transaction == null ? conn! : transaction.Connection!);
                if (transaction != null)
                    cmd.Transaction = transaction;

                cmd.CommandText = string.Format(
                    CultureInfo.InvariantCulture,
                    sqlSelectCount,
                    !string.IsNullOrEmpty(where) ? $" where {where}" : string.Empty
                );
                if ((parameters?.Length ?? 0) > 0)
                {
                    foreach (var dbPar in parameters!)
                        cmd.Parameters.Add(dbProviderFactory.CreateParameter(dbPar));
                }

                using var reader = cmd.ExecuteReader();
                reader.Read();
                return reader.GetInt32(0);
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }
        }

        public virtual int GetCountForParent(FilterSimple parentFilter, string? additionalWhere = null, DbParameter[]? parameters = null, DbTransaction? transaction = null)
        {
            Guard.Against.Null(parentFilter, nameof(parentFilter));

            var where = parentFilter.Where;
            if (!string.IsNullOrEmpty(additionalWhere))
                where += " and " + additionalWhere;
            return GetCount(where, parameters, transaction);
        }

        public virtual void ResetCache()
        {
            foreach (var o in Cache.Values)
                o.ResetCachedRefProperties();
            Cache.Clear();
        }

        public IDictionary<Guid, DbMappedDOT> PredefinedObjects => predefinedObjects;
        #endregion

        #region UPDATE
        protected virtual string GenerateSqlUpdate() =>
            $"update {tableNameQ}" + Environment.NewLine +
            $"set {updateFieldValuePairs}" + Environment.NewLine +
            $"where {c_fieldIdQ} = {paramId};";

        // Also used in CREATE
        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Our design is to convert any errors (including exceptions) to the returned error message of string type")]
        string? PrivateFlush(DbCommand command, DbMappedDOT dataObject)
        {
            var idNotSet = !dataObject.Id.HasValue;

            command.CommandType = CommandType.Text;
            if (idNotSet)
            {
                dataObject.Id = Guid.NewGuid();
                command.CommandText = sqlInsert;
            }
            else
            {
                // If Id is set, we conclude that it is an update of an existing object,
                // but that can be actually not true, pls read the comment below, near
                // "rowsAffected".
                command.CommandText = sqlUpdate;
            }

            command.Parameters.Add(dbProviderFactory.CreateParameter(paramId, dataObject.Id!.Value, typeof(Guid), false));
            FillParameters(command.Parameters, (TDOT)dataObject);

            try
            {
                var rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected < 1)
                {
                    var failed = true;

                    // If Id is set, but the update was failed, then possibly actually that was
                    // a new object with a previously set Id. In that case, we change the query
                    // type from UPDATE to INSERT and repeat the action.
                    if (!idNotSet)
                    {
                        command.CommandText = sqlInsert;
                        rowsAffected = command.ExecuteNonQuery();
                        failed = rowsAffected < 1;
                    }

                    if (failed)
                    {
                        if (idNotSet)
                            dataObject.Id = null;
                        return $"Failure <?>: expected that not less than 1 row will be affected, but actually {rowsAffected} rows were affected.";
                    }
                }
            }
            catch (Exception ex)
            {
                const string msgDbException = "Exception happened while working with the database:{0}{1}";

                if (idNotSet)
                    dataObject.Id = null;
                var uniqueConstraintField = dbProviderFactory.CheckForUniqueConstraintException(ex, tableName, fields, fieldsHuman);
                return uniqueConstraintField != null ?
                    $"The field \"{uniqueConstraintField}\" should contain a unique value. The value you entered already exists in the database." :
                    string.Format(CultureInfo.CurrentCulture, msgDbException, Environment.NewLine, ex.ToString());
            }

            if (Cache.ContainsKey(dataObject.Id.Value))
                Cache.TryRemove(dataObject.Id.Value, out _);
            Cache.TryAdd(dataObject.Id.Value, dataObject);

            dataObject.SetStorageConsistency();

            return null;
        }

        // Also used in CREATE
        public virtual string? Flush(DbMappedDOT dataObject, DbTransaction? transaction = null)
        {
            // TODO: Integrity not checked yet.

            Guard.Against.Null(dataObject, nameof(dataObject));

            if (dataObject.InSync)
                return null;

            var validationError = dataObject.Validate();
            if (validationError != null)
                return validationError;

            var conn = transaction != null ?
                transaction.Connection! :
                dbProviderFactory.CreateOpenConnection();
            try
            {
                using var cmd = conn.CreateCommand();
                if (transaction != null)
                    cmd.Transaction = transaction;
                return PrivateFlush(cmd, dataObject);
            }
            finally
            {
                if (transaction == null)
                    conn.Dispose();
            }
        }
        #endregion

        #region DELETE
        protected virtual string GenerateSqlDelete() =>
            $"delete from {tableNameQ}" + Environment.NewLine +
            $"where {c_fieldIdQ} = {paramId};";

        protected string? ProtectedDelete(DbCommand command, Guid id, bool throwZeroRowsAffectedException)
        {
            Guard.Against.Null(command, nameof(command));

            command.CommandType = CommandType.Text;
            command.CommandText = sqlDelete;

            command.Parameters.Add(dbProviderFactory.CreateParameter(paramId, id, typeof(Guid), false));

            try
            {
                var rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected < 1)
                {
                    if (rowsAffected == 0 && throwZeroRowsAffectedException)
                        throw new ZeroRowsAffectedException();
                    else
                        return $"Deletion failed: it was expected to be \"affected\" not less than 1 row, but actually \"affected\" {rowsAffected} rows.";
                }
            }
            catch (Exception ex)
            {
                if (throwZeroRowsAffectedException && ex is ZeroRowsAffectedException)
                    throw;
                else
                    return $"There was an exception when working with the Database:{Environment.NewLine}{ex}";
            }

            if (Cache.ContainsKey(id))
                Cache.TryRemove(id, out _);

            return null;
        }

        protected virtual string? ProtectedDelete(Guid id, DbTransaction? transaction, bool throwZeroRowsAffectedException)
        {
            if (predefinedObjects.ContainsKey(id))
                return ERR_CANNOT_DELETE_PREDEFINED_OBJECT;

            DbConnection conn = transaction != null ?
                transaction.Connection! :
                dbProviderFactory.CreateOpenConnection();
            try
            {
                using DbCommand cmd = conn.CreateCommand();
                if (transaction != null)
                    cmd.Transaction = transaction;
                return ProtectedDelete(cmd, id, throwZeroRowsAffectedException);
            }
            finally
            {
                if (transaction == null)
                    conn.Dispose();
            }
        }

        public virtual string? Delete(DbMappedDOT dataObject, DbTransaction? transaction = null)
        {
            Guard.Against.Null(dataObject, nameof(dataObject));

            // Don't delete new objects from the DB because they are not saved there yet.
            if (!dataObject.Id.HasValue)
                return null;
            return Delete(dataObject.Id.Value, transaction);
        }

        public virtual string? Delete(Guid id, DbTransaction? transaction = null) => ProtectedDelete(id, transaction, false);

        public virtual string? Delete(Guid id, bool throwZeroRowsAffectedException)
        {
            if (predefinedObjects.ContainsKey(id))
                return ERR_CANNOT_DELETE_PREDEFINED_OBJECT;

            using var conn = dbProviderFactory.CreateOpenConnection();
            using var cmd = dbProviderFactory.CreateCommand(conn);
            return ProtectedDelete(cmd, id, throwZeroRowsAffectedException);
        }

        public string GetMessageCannotDeletePredefinedObject() => ERR_CANNOT_DELETE_PREDEFINED_OBJECT;
        #endregion

        protected void InitParams() => parameters = Enumerable
            .Range(0, fields.Length)
            .Select(i => paramPrefix + fields[i])
            .ToArray();

        public virtual void Init()
        {
            fieldsAll = string.Format(CultureInfo.InvariantCulture, "{0}{1}{0}", quoteSymbol, fields[0]);
            fieldsAllDecimalAware = fieldsAll;
            paramsAll = paramPrefix + "id";
            updateFieldValuePairs = string.Empty;
            for (var i = 1; i < fields.Length; i++)
            {
                var quotedField = string.Format(CultureInfo.InvariantCulture, "{0}{1}{0}", quoteSymbol, fields[i]);
                var param = paramPrefix + fields[i];
                fieldsAll += commaSeparator + quotedField;
                paramsAll += commaSeparator + param;

                fieldsAllDecimalAware += commaSeparator + quotedField;

                if (!string.IsNullOrEmpty(updateFieldValuePairs))
                    updateFieldValuePairs += commaSeparator;
                updateFieldValuePairs += $"{quotedField} = {param}";
            }

            sqlSelectOne = GenerateSqlSelectOne();
            sqlSelectTable = GenerateSqlSelectTable();
            sqlSelectCollection = GenerateSqlSelectCollection();
            sqlSelectCount = GenerateSqlSelectCount();
            sqlInsert = GenerateSqlInsert();
            sqlUpdate = GenerateSqlUpdate();
            sqlDelete = GenerateSqlDelete();
        }

        public ConcurrentDictionary<Guid, DbMappedDOT> Cache = new();

        protected abstract void FillParameters(DbParameterCollection parameters, TDOT dataObject);

        /// <summary>
        /// Table name (without quotes)
        /// </summary>
        public string TableName => tableName;
        /// <summary>
        /// Table name (with quotes)
        /// </summary>
        public string TableNameQ => tableNameQ;
        /// <summary>
        /// Table field names
        /// </summary>
        public IEnumerable<string> Fields => fields;

        public DOTOverrider? DOTOverrider { get => dotOverrider; set => dotOverrider = value; }
    }
}
