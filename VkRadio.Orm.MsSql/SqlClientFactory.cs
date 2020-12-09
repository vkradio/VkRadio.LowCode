using Ardalis.GuardClauses;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;

namespace VkRadio.Orm
{
    public class SqlClientFactory : DbProviderFactory
    {
        static SqlDbType TypeToSqlDbType(Type type)
        {
            if (type == typeof(Guid?) || type == typeof(Guid))
                return SqlDbType.UniqueIdentifier;
            else if (type == typeof(int?) || type == typeof(int))
                return SqlDbType.Int;
            else if (type == typeof(byte?) || type == typeof(byte))
                return SqlDbType.TinyInt;
            else if (type == typeof(long?) || type == typeof(long))
                return SqlDbType.BigInt;
            else if (type == typeof(string))
                return SqlDbType.NVarChar;
            else if (type == typeof(bool?) || type == typeof(bool))
                return SqlDbType.Bit;
            else if (type == typeof(DateTime?) || type == typeof(DateTime))
                return SqlDbType.DateTime;
            else if (type == typeof(decimal?) || type == typeof(decimal))
                return SqlDbType.Decimal;
            else if (type == typeof(float?) || type == typeof(float?))
                return SqlDbType.Float;
            else
                throw new ArgumentException($"Unsupported type for Sql Parameter: {type.FullName}.");
        }

        public SqlClientFactory(string connectionString)
        {
            this.connectionString = connectionString;
            quoteSymbol = "\"";
            SelectTop = SelectTopStyle.AsMs;
            GuidStyle = GuidStyle.AsMs;
        }

        public override DbConnection CreateOpenConnection()
        {
            var connection = new SqlConnection();
            InitConnection(connection);
            return connection;
        }

        public override DbCommand CreateCommand(DbConnection connection)
        {
            Guard.Against.Null(connection, nameof(connection));
            return ((SqlConnection)connection).CreateCommand();
        }

        public override DbDataAdapter CreateDataAdapter() => new SqlDataAdapter();

        public override DbParameter CreateParameter(string paramName, object? value, Type type, bool isNullable)
        {
            Guard.Against.Null(type, nameof(type));

            return new SqlParameter(paramName, TypeToSqlDbType(type))
            {
                Value = isNullable && value == null ? DBNull.Value : value
            };
        }

        public override DbParameter CreateParameter(DbParameter proto)
        {
            Guard.Against.Null(proto, nameof(proto));

            return new SqlParameter(proto.ParameterName, proto.DbType) { Value = proto.Value };
        }

        public override string GuidToSqlString(Guid guid) => $"'{guid}'";

        public override string? CheckForUniqueConstraintException(Exception exception, string tableName, string[] tableFieldDbNames, string[] tableFieldHumanNames)
        {
            Guard.Against.Null(tableName, nameof(tableName));
            Guard.Against.Null(tableFieldDbNames, nameof(tableFieldDbNames));
            Guard.Against.Null(tableFieldHumanNames, nameof(tableFieldHumanNames));

            if (exception is SqlException exSql)
            {
                if (exSql.Message.Contains("UNIQUE KEY constraint"))
                {
                    var indexOfBegin = exSql.Message.IndexOf('\'', 0) + 1;
                    var indexOfEnd = exSql.Message.IndexOf('\'', indexOfBegin);
                    var uniqueIndexName = exSql.Message.Substring(indexOfBegin, indexOfEnd - indexOfBegin);
                    // Example (for CRUD generated naming only): Let we have a table called sp_vendor and it has a field containing unique
                    // values called web_site_or_name, then constructed index will be called ux_sp_vendor_web_site_or_name.
                    var fieldName = uniqueIndexName.Substring(($"ux_{tableName}_").Length);
                    var fieldIndex = -1;
                    for (var i = 0; i < tableFieldDbNames.Length; i++)
                    {
                        if (fieldName == tableFieldDbNames[i])
                        {
                            fieldIndex = i;
                            break;
                        }
                    }
                    var fieldNameHuman = tableFieldHumanNames[fieldIndex].Substring(0, 1).ToUpper(CultureInfo.CurrentCulture);
                    if (tableFieldHumanNames[fieldIndex].Length > 1)
                        fieldNameHuman += tableFieldHumanNames[fieldIndex].Substring(1);
                    return fieldNameHuman;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        #region Type conversions when reading an object value from a Database.
#pragma warning disable CA1062 // Here we are suppressing the Roslyn analyzer warnings on the DbDataReader param, because the speed of execution
                               // is critical here, so we just assume no one of ascendants or API users will put null reader here.
        public override bool ReadBoolFromReader(DbDataReader reader, int index) => (bool)reader[index];
        public override bool? ReadBoolNullableFromReader(DbDataReader reader, int index) => reader[index] as bool?;
        public override DateTime ReadDateTimeFromReader(DbDataReader reader, int index) => (DateTime)reader[index];
        public override DateTime? ReadDateTimeNullableFromReader(DbDataReader reader, int index) => reader[index] as DateTime?;
        public override decimal ReadDecimalFromReader(DbDataReader reader, int index) => (decimal)reader[index];
        public override decimal? ReadDecimalNullableFromReader(DbDataReader reader, int index) => reader[index] as decimal?;
        public override int ReadIntFromReader(DbDataReader reader, int index) => (int)reader[index];
        public override int? ReadIntNullableFromReader(DbDataReader reader, int index) => reader[index] as int?;
        public override string? ReadStringFromReader(DbDataReader reader, int index, bool nullable) => nullable && reader[index] == DBNull.Value ? null : (string)reader[index];
        public override Guid ReadGuidFromReader(DbDataReader reader, int index) => (Guid)reader[index];
        public override Guid? ReadGuidNullableFromReader(DbDataReader reader, int index) => reader[index] as Guid?;
#pragma warning restore CA1062 // Validate arguments of public methods
        #endregion
    };
}
