using System;
using System.Collections.Generic;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    public abstract class TableJson: ITextDefinition
    {
        protected string _name;
        protected SchemaDeploymentScriptJson _schemaDeploymentScript;
        //protected List<DynamicTypeForeignKey> _dynamicTypeForeignKeys = new List<DynamicTypeForeignKey>();
        protected List<ForeignKeyFieldJson> _foreignKeyFields = new List<ForeignKeyFieldJson>();
        protected List<ValueFieldJson> _valueFields = new List<ValueFieldJson>();
        protected PrimaryKeyJson _primaryKey;
        protected string _quoteSymbol;

        /// <summary>
        /// Закрытый конструктор, чтобы невозможно было создать извне таблицу без параметров
        /// </summary>
        TableJson() {}
        /// <summary>
        /// Открытый конструктор, устанавливающий имя таблицы
        /// </summary>
        /// <param name="in_name">Имя таблицы</param>
        /// <param name="in_schemaDeploymentScript">Логическое представление скрипта развертывания БД</param>
        public TableJson(string in_name, SchemaDeploymentScriptJson in_schemaDeploymentScript)
        {
            _name = in_name;
            _schemaDeploymentScript = in_schemaDeploymentScript;
        }

        /// <summary>
        /// Генерирование "хвоста" определения таблицы - то, что находится между закрывающей
        /// скобкой и символом точки с запятой (;).
        /// </summary>
        /// <returns></returns>
        protected virtual string GenerateTableDefTail() { return string.Empty; }

        public string Name { get { return _name; } }
        public SchemaDeploymentScriptJson SchemaDeploymentScript { get { return _schemaDeploymentScript; } }
        //public IList<DynamicTypeForeignKey> DynamicTypeForeignKeys { get { return _dynamicTypeForeignKeys; } }
        public IList<ForeignKeyFieldJson> ForeignKeyFields { get { return _foreignKeyFields; } }
        public IList<ITableFieldJson> AllFields
        {
            get
            {
                List<ITableFieldJson> fields = new List<ITableFieldJson>();

                // Добавление ПК.
                if (_primaryKey is PKSingleJson)
                {
                    fields.Add((PKSingleJson)_primaryKey);
                }
                //else if (_primaryKey is PKCompound)
                //{
                //    PKCompound pkCompound = (PKCompound)_primaryKey;
                //    foreach (PKCompoundPart keyPart in pkCompound.Parts)
                //        fields.Add(keyPart);
                //}
                else
                {
                    throw new ApplicationException(string.Format("Unsupported PrimaryKey type: {0}.", _primaryKey.GetType().Name));
                }

                // Добавление полей непосредственных значений.
                foreach (ValueFieldJson field in _valueFields)
                    fields.Add(field);

                // Добавление полей обычных ВК.
                foreach (ForeignKeyFieldJson fk in _foreignKeyFields)
                    fields.Add(fk);

                //// Добавление динамически типизированных составных ВК.
                //foreach (DynamicTypeForeignKey dynFK in _dynamicTypeForeignKeys)
                //{
                //    fields.Add(dynFK.RefField);
                //    fields.Add(dynFK.TypeField);
                //}

                return fields;
            }
        }
        public IList<ValueFieldJson> ValueFields { get { return _valueFields; } }
        public PrimaryKeyJson PrimaryKey { get { return _primaryKey; } set { _primaryKey = value; } }
        public string QuoteSymbol { get { return _quoteSymbol; } }

        public string[] GenerateText()
        {
            List<string> result = new List<string>();

            result.Add(string.Format("create table {0}{1}{2}", _quoteSymbol, _name, _quoteSymbol));
            result.Add("(");
            IList<ITableFieldJson> fields = AllFields;
            for (int i = 0; i < fields.Count; i++)
            {
                string[] fieldStrings = fields[i].GenerateText();
                if (fieldStrings.Length != 0)
                {
                    for (int j = 0; j < fieldStrings.Length; j++)
                        fieldStrings[j] = DBSchemaHelper.C_TAB + fieldStrings[j];

                    if (i < fields.Count - 1)
                        fieldStrings[fieldStrings.Length - 1] += ",";

                    result.AddRange(fieldStrings);
                }
            }
            result.Add(")" + GenerateTableDefTail() + ";");

            return result.ToArray();
        }
    };
}
