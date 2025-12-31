using System.Collections.Generic;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    public class PredefinedInsertJson: ITextDefinition
    {
        protected const string c_insertPattern = "insert into {0}{1}{2} ({3}) values ({4});";

        protected SchemaDeploymentScriptJson _schemaDeploymentScript;
        protected List<FieldValueJson> _fieldValues = new List<FieldValueJson>();
        protected TableJson _table;
        protected string _quoteSymbol = string.Empty;

        public SchemaDeploymentScriptJson SchemaDeploymentScript { get { return _schemaDeploymentScript; } set { _schemaDeploymentScript = value; } }
        public IList<FieldValueJson> FieldValues { get { return _fieldValues; } }
        public TableJson Table { get { return _table; } set { _table = value; } }

        public string[] GenerateText()
        {
            string fieldsText = string.Empty;
            foreach (FieldValueJson fv in _fieldValues)
            {
                if (fieldsText != string.Empty)
                    fieldsText += ", ";
                fieldsText += _quoteSymbol + fv.Field.Name + _quoteSymbol;
            }

            string valuesText = string.Empty;
            foreach (FieldValueJson fv in _fieldValues)
            {
                if (valuesText != string.Empty)
                    valuesText += ", ";
                valuesText += fv.Value;
            }

            return new string[]
            {
                string.Format(
                    c_insertPattern,
                    _quoteSymbol, _table.Name, _quoteSymbol,
                    fieldsText,
                    valuesText
                )
            };
        }

        public static int PredefinedInsertComparer(PredefinedInsertJson in_1, PredefinedInsertJson in_2)
        {
            int result = string.Compare(in_1.Table.Name, in_2.Table.Name);
            if (result == 0)
                result = string.Compare(in_1.FieldValues[0].Value, in_2.FieldValues[0].Value);
            return result;
        }
    };
}
