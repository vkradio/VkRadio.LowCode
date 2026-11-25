using System;
using VkRadio.Patterns.Guards;

namespace VkRadio.LowCode.Libs.Orm
{
    public class FilterSimple : FilterAbstract
    {
        protected string? _where;
        protected string? _orderBy;

        public FilterSimple(string? where, string? orderBy)
        {
            _where = where;
            _orderBy = orderBy;
        }

        public string? Where { get => _where; set => _where = value; }

        public string? OrderBy { get => _orderBy; set => _orderBy = value; }

        public override string? ToWhere() => _where;

        public override string? ToOrderBy() => _orderBy;

        public static FilterSimple CreateTableFilter(Guid? id, string fieldName, IDbProviderFactory dbProviderFactory, bool fieldIsNullable = true)
        {
            GuardRule.Forbid.Null(fieldName, nameof(fieldName));
            GuardRule.Forbid.Null(dbProviderFactory, nameof(dbProviderFactory));

            string where = fieldIsNullable ?
                $"COALESCE(\"{fieldName}\", {dbProviderFactory.GuidToSqlString(Guid.NewGuid())})" :
                $"\"{fieldName}\"";
            where += " = " + dbProviderFactory.GuidToSqlString(id ?? Guid.Empty);
            return new FilterSimple(where, null);
        }

        public static FilterSimple CreateTableFilter(Guid? id, string fieldName, string guidSqlStringOpening, string guidSqlStringClosing, bool fieldIsNullable = true)
        {
            GuardRule.Forbid.Null(fieldName, nameof(fieldName));
            GuardRule.Forbid.Null(guidSqlStringOpening, nameof(guidSqlStringOpening));
            GuardRule.Forbid.Null(guidSqlStringClosing, nameof(guidSqlStringClosing));

            string where = fieldIsNullable ?
                $"COALESCE(\"{fieldName}\", {guidSqlStringOpening}{Guid.NewGuid()}{guidSqlStringClosing})" :
                $"\"{fieldName}\"";
            where += $" = {guidSqlStringOpening}{(id ?? Guid.Empty)}{guidSqlStringClosing}";
            return new FilterSimple(where, null);
        }
    }
}
