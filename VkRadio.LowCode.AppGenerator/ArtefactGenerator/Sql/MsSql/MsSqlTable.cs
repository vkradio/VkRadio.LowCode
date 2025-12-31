using System.Collections.Generic;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql.MsSql
{
    public class MsSqlTable : Table
    {
        public MsSqlTable(string in_name, SchemaDeploymentScript in_schemaDeploymentScript) : base(in_name, in_schemaDeploymentScript)
        {
            _quoteSymbol = "\"";
        }

        public IList<string> GenerateConstraints()
        {
            List<string> result = new List<string>();

            //if (_primaryKey != null)
            //    result.AddRange(((MsSqlPKSingle)_primaryKey).GenerateConstraints());
            foreach (ITableFieldJson field in AllFields)
            {
                IMsSqlConstraint constraintField = (IMsSqlConstraint)field;
                result.AddRange(constraintField.GenerateConstraints());
            }

            if (result.Count != 0)
                result.Add("go");
            return result;
        }
    }
}
