using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;

namespace VkRadio.LowCode.Orm
{
    public interface IDbProviderFactory
    {
        DbConnection CreateOpenConnection();
        DbCommand CreateCommand(DbConnection connection);
        DbDataAdapter CreateDataAdapter();

        string GetQuoteSymbol();
        DbParameter CreateParameter(string paramName, object? value, Type type, bool isNullable);
        DbParameter CreateParameter(DbParameter proto);
        SelectTopStyle SelectTop { get; }
        GuidStyle GuidStyle { get;  }

        [SuppressMessage("Naming", "CA1720:Identifier contains type name", Justification = "Better call it a guid.")]
        string GuidToSqlString(Guid guid);
        string? CheckForUniqueConstraintException(Exception exception, string tableName, string[] tableFieldDbNames, string[] tableFieldHumanNames);

        #region Type conversions when reading an object value from a database.
        bool ReadBoolFromReader(DbDataReader reader, int index);
        bool? ReadBoolNullableFromReader(DbDataReader reader, int index);
        DateTime ReadDateTimeFromReader(DbDataReader reader, int index);
        DateTime? ReadDateTimeNullableFromReader(DbDataReader reader, int index);
        decimal ReadDecimalFromReader(DbDataReader reader, int index);
        decimal? ReadDecimalNullableFromReader(DbDataReader reader, int index);
        int ReadIntFromReader(DbDataReader reader, int index);
        int? ReadIntNullableFromReader(DbDataReader reader, int index);
        string? ReadStringFromReader(DbDataReader reader, int index, bool nullable);
        Guid ReadGuidFromReader(DbDataReader reader, int index);
        Guid? ReadGuidNullableFromReader(DbDataReader reader, int index);
        #endregion
    }
}
