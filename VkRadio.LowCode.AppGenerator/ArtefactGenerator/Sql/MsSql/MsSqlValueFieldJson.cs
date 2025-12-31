using System.Collections.Generic;

using MetaModel.PropertyDefinition;
using MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql.MsSql
{
    public class MsSqlValueFieldJson: ValueFieldJson, IMsSqlConstraint
    {
        public MsSqlValueFieldJson(TableAndDOTCorrespondenceJson in_tableAndDOTCorrespondence, PropertyDefinition in_propertyDefinition)
            : base(in_tableAndDOTCorrespondence, in_propertyDefinition)
        {
            _sqlTypesWDeniedDefaults = new string[]
            {
            };
            _boolSqlType = "bit";
            _quoteSymbol = "\"";
            _generateConstraintsInline = false;
        }

        protected override void SetupStringField(PropertyDefinition in_propertyDefinition)
        {
            PFTString pft = (PFTString)in_propertyDefinition.FunctionalType;
            int len = pft.MaxLength;
            // TODO: Здесь нужно уточнить, 4000 или 8000, т.к. здесь
            // http://stackoverflow.com/questions/564755/sql-server-text-type-vs-varchar-data-type
            // сказано 8000, но на примере text, а для ntext, возможно, в 2 раза меньше.
            _sqlType = string.Format("nvarchar({0})", len <= 4000 ? len.ToString() : "MAX");

            bool defaultDeniedBySql = false;
            foreach (string sqlType in _sqlTypesWDeniedDefaults)
            {
                if (sqlType == _sqlType)
                {
                    defaultDeniedBySql = true;
                    break;
                }
            }

            SetupStringFieldDefault(defaultDeniedBySql, in_propertyDefinition);
        }
        protected override void SetupUniqueCodeField(PropertyDefinition in_propertyDefinition)
        {
            PFTUniqueCode pft = (PFTUniqueCode)in_propertyDefinition.FunctionalType;
            _sqlType = MsSqlForeignKeyFieldJson.C_SQL_TYPE;
            // TODO: Здесь не заморачиваемся с дефолтом, как будто он не нужен.
        }

        public IList<string> GenerateConstraints()
        {
            List<string> result = new List<string>();

            if (_unique)
            {
                result.Add(string.Format("alter table {0}{1}{0} add constraint {0}ux_{1}_{2}{0} unique nonclustered ({0}{2}{0});", _quoteSymbol, _table.Name, _name));
            }
            if (_defaultValue != null)
            {
                result.Add(string.Format("alter table {0}{1}{0} add constraint {0}df_{1}_{2}{0} default {3} for {0}{2}{0};", _quoteSymbol, _table.Name, _name, _defaultValue));
            }

            return result;
        }
    };
}
