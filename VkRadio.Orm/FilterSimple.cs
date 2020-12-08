using Ardalis.GuardClauses;
using System;

namespace VkRadio.Orm
{
    public class FilterSimple : FilterAbstract
    {
        protected string? where;
        protected string? orderBy;

        public FilterSimple(string? where, string? orderBy)
        {
            this.where = where;
            this.orderBy = orderBy;
        }

        public string? Where { get => where; set => where = value; }

        public string? OrderBy { get => orderBy; set => orderBy = value; }

        public override string? ToWhere() => where;

        public override string? ToOrderBy() => orderBy;

        public static FilterSimple CreateTableFilter(Guid? id, string fieldName, DbProviderFactory dbProviderFactory, bool fieldIsNullable = true)
        {
            Guard.Against.Null(fieldName, nameof(fieldName));
            Guard.Against.Null(dbProviderFactory, nameof(dbProviderFactory));

            string where = fieldIsNullable ?
                $"COALESCE(\"{fieldName}\", {dbProviderFactory.GuidToSqlString(Guid.NewGuid())})" :
                $"\"{fieldName}\"";
            where += " = " + dbProviderFactory.GuidToSqlString(id ?? Guid.Empty);
            return new FilterSimple(where, null);
        }

        public static FilterSimple CreateTableFilter(Guid? id, string fieldName, string guidSqlStringOpening, string guidSqlStringClosing, bool fieldIsNullable = true)
        {
            Guard.Against.Null(fieldName, nameof(fieldName));
            Guard.Against.Null(guidSqlStringOpening, nameof(guidSqlStringOpening));
            Guard.Against.Null(guidSqlStringClosing, nameof(guidSqlStringClosing));

            string where = fieldIsNullable ?
                $"COALESCE(\"{fieldName}\", {guidSqlStringOpening}{Guid.NewGuid()}{guidSqlStringClosing})" :
                $"\"{fieldName}\"";
            where += $" = {guidSqlStringOpening}{(id ?? Guid.Empty)}{guidSqlStringClosing}";
            return new FilterSimple(where, null);
        }
    }
}
