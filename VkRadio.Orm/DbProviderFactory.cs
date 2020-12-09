using Ardalis.GuardClauses;
using System;
using System.Data.Common;
using System.Linq;

namespace VkRadio.Orm
{
    public abstract class DbProviderFactory : IDbProviderFactory
    {
        protected string connectionString = default!;
        protected string quoteSymbol = string.Empty;

        protected virtual void InitConnection(DbConnection connection)
        {
            Guard.Against.Null(connection, nameof(connection));

            connection.ConnectionString = connectionString;
            connection.Open();
        }

        public abstract DbConnection CreateOpenConnection();
        public abstract DbCommand CreateCommand(DbConnection connection);
        public abstract DbDataAdapter CreateDataAdapter();
        public virtual string GetQuoteSymbol() => quoteSymbol;
        public abstract DbParameter CreateParameter(string paramName, object? value, Type type, bool isNullable);
        public abstract DbParameter CreateParameter(DbParameter proto);
        public SelectTopStyle SelectTop { get; protected set; }
        public GuidStyle GuidStyle { get; protected set; }

        public abstract string GuidToSqlString(Guid guid);
        public abstract string? CheckForUniqueConstraintException(Exception exception, string tableName, string[] tableFieldDbNames, string[] tableFieldHumanNames);

        #region Type conversions when reading an object value from a database.
        public abstract bool ReadBoolFromReader(DbDataReader reader, int index);
        public abstract bool? ReadBoolNullableFromReader(DbDataReader reader, int index);
        public abstract DateTime ReadDateTimeFromReader(DbDataReader reader, int index);
        public abstract DateTime? ReadDateTimeNullableFromReader(DbDataReader reader, int index);
        public abstract decimal ReadDecimalFromReader(DbDataReader reader, int index);
        public abstract decimal? ReadDecimalNullableFromReader(DbDataReader reader, int index);
        public abstract int ReadIntFromReader(DbDataReader reader, int index);
        public abstract int? ReadIntNullableFromReader(DbDataReader reader, int index);
        public abstract string? ReadStringFromReader(DbDataReader reader, int index, bool nullable);
        public abstract Guid ReadGuidFromReader(DbDataReader reader, int index);
        public abstract Guid? ReadGuidNullableFromReader(DbDataReader reader, int index);
        #endregion

        public static DbParameter[] CloneParams(DbParameter[] proto, DbProviderFactory dbProviderFactory) =>
            proto.Select(par => dbProviderFactory.CreateParameter(par)).ToArray();


        #region Workaround for legacy cases when we cannot substitute IDbProviderFactory via DI/IoC
        static IDbProviderFactory? defaultValue;
        
        public static IDbProviderFactory Default { get => defaultValue ?? throw new InvalidOperationException("Default static property of DbProviderFactory is not set."); set { defaultValue = value; } }
        #endregion
    }
}
