using System.Collections.Generic;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql.MsSql
{
    public class MsSqlPKSingleJson: PKSingleJson, IMsSqlConstraint
    {
        public MsSqlPKSingleJson()
        {
            _sqlType = "uniqueidentifier";
            _quoteSymbol = "\"";
        }

        public override string[] GenerateText()
        {
            string[] result = base.GenerateText();
            result[0] = result[0].Replace(" primary key", string.Empty);
            return result;
        }

        public IList<string> GenerateConstraints()
        {
            List<string> result = new List<string>();
            result.Add(string.Format("alter table {0}{1}{0} add constraint {0}pk_{1}{0} primary key clustered ({0}{2}{0});", _quoteSymbol, _table.Name, _name));
            return result;
        }
    };
}
