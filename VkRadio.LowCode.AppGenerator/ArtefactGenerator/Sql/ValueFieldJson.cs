using System;
using System.Globalization;

using MetaModel.Names;
using MetaModel.PropertyDefinition;
using MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    /// <summary>
    /// Поле таблицы, хранящее непосредственное значение
    /// (т.е. не внешний ключ или что-то еще подобное)
    /// </summary>
    public abstract class ValueFieldJson: ITableFieldJson
    {
        protected string _name;
        protected bool _nullable;
        protected string _sqlType;
        protected TableJson _table;
        protected PropertyCorrespondenceJson _dotPropertyCorrespondence;
        protected string _defaultValue;
        protected bool _unique;
        protected string[] _sqlTypesWDeniedDefaults;
        protected string _boolSqlType;
        protected string _quoteSymbol;
        protected bool _generateConstraintsInline = true;

        ValueFieldJson() { throw new Exception(); }

        protected abstract void SetupStringField(PropertyDefinition in_propertyDefinition);
        protected virtual void SetupStringFieldDefault(bool in_defaultDeniedBySql, PropertyDefinition in_propertyDefinition)
        {
            if (!in_defaultDeniedBySql && in_propertyDefinition.DefaultValue != null)
            {
                // TODO: Используем простейшие SQL Escape. В идеале нужно реализовать полноценный escape в соответствии
                // со стандартами SQL и имеющимися вендорскими реализациями.
                _defaultValue = DOTPropertyCorrespondence.TableAndDOTCorrespondence.DBSchemaMetaModel.GetValueStringForString((string)in_propertyDefinition.DefaultValue);
                //_defaultValue = "N'" + ((string)in_propertyDefinition.DefaultValue).Replace("'", "''") + "'";
            }
        }
        protected abstract void SetupUniqueCodeField(PropertyDefinition in_propertyDefinition);
        protected virtual void SetupDateTimeField(PropertyDefinition in_propertyDefinition) { _sqlType = "datetime"; }

        /// <summary>
        /// Наименование поля (без символов квотирования)
        /// </summary>
        public string Name { get { return _name; } }
        /// <summary>
        /// Может ли поле содержать значения NULL
        /// </summary>
        public bool Nullable { get { return _nullable; } }
        /// <summary>
        /// Тип SQL (строка)
        /// </summary>
        public string SqlType { get { return _sqlType; } }
        /// <summary>
        /// Таблица, к которой принадлежит поле
        /// </summary>
        public TableJson Table { get { return _table; } }
        /// <summary>
        /// Соответствие между полем таблицы и свойством ТОД
        /// </summary>
        public PropertyCorrespondenceJson DOTPropertyCorrespondence { get { return _dotPropertyCorrespondence; } }
        /// <summary>
        /// Значение по умолчанию (строка, либо null, если значение по умолчанию отсутствует)
        /// </summary>
        public string DefaultValue { get { return _defaultValue; } }
        /// <summary>
        /// Является ли значение уникальным в пределах таблицы
        /// </summary>
        public bool Unique { get { return _unique; } }

        /// <summary>
        /// Конструктор поля таблицы с непосредственным значением, берущий за основу
        /// определение свойства ТОД
        /// </summary>
        /// <param name="in_table">Таблица, к которой принадлежит свойство</param>
        /// <param name="in_tableAndDOTCorrespondence">Соответствие между таблицей и ТОД</param>
        /// <param name="in_propertyDefinition">Свойство ТОД</param>
        public ValueFieldJson(TableAndDOTCorrespondenceJson in_tableAndDOTCorrespondence, PropertyDefinition in_propertyDefinition)
        {
            _table = in_tableAndDOTCorrespondence.Table;
            _name = NameHelper.NameToUnderscoreSeparatedName(in_propertyDefinition.Names[HumanLanguageEnum.En]);
            _nullable = in_propertyDefinition.FunctionalType.Nullable;
            _unique = in_propertyDefinition.FunctionalType.Unique;
            _dotPropertyCorrespondence = new PropertyCorrespondenceJson() { PropertyDefinition = in_propertyDefinition, TableField = this, TableAndDOTCorrespondence = in_tableAndDOTCorrespondence };
        }
        public void Init()
        {
            PropertyDefinition propDef = _dotPropertyCorrespondence.PropertyDefinition;

            // Установление типа SQL и значения поля по умолчанию.
            // TODO: Привести типы SQL по возможности ближе к стандарту SQL.
            if (propDef.FunctionalType is PFTBoolean)
            {
                _sqlType = _boolSqlType;
                if (_dotPropertyCorrespondence.PropertyDefinition.DefaultValue != null)
                    _defaultValue = (bool)_dotPropertyCorrespondence.PropertyDefinition.DefaultValue ? "1" : "0";
            }
            else if (propDef.FunctionalType is PFTDateTime)
            {
                SetupDateTimeField(propDef);
            }
            else if (propDef.FunctionalType is PFTDecimal)
            {
                _sqlType = "decimal(10, 2)";
                if (_dotPropertyCorrespondence.PropertyDefinition.DefaultValue != null)
                    _defaultValue = ((decimal)_dotPropertyCorrespondence.PropertyDefinition.DefaultValue).ToString(CultureInfo.InvariantCulture);
            }
            else if (propDef.FunctionalType is PFTInteger)
            {
                _sqlType = "integer";
                if (_dotPropertyCorrespondence.PropertyDefinition.DefaultValue != null)
                    _defaultValue = ((int)_dotPropertyCorrespondence.PropertyDefinition.DefaultValue).ToString();
            }
            else if (propDef.FunctionalType is PFTUniqueCode)
            {
                SetupUniqueCodeField(propDef);
            }
            else if (propDef.FunctionalType is PFTString)
            {
                SetupStringField(propDef);
            }
            else
            {
                throw new ApplicationException(string.Format("Unsupported PropertyFunctionalType for ValueField: {0}.", propDef.FunctionalType.GetType().Name));
            }
        }

        /// <summary>
        /// Генерирование текстового представления объявление поля таблицы на SQL
        /// </summary>
        /// <returns>Объявление поля таблицы на SQL</returns>
        public virtual string[] GenerateText()
        {
            string result = string.Format("{0}{1}{2} {3} {4}", _quoteSymbol, _name, _quoteSymbol, _sqlType, _nullable ? "null" : "not null");
            if (_generateConstraintsInline)
            {
                if (_unique)
                    result += " " + DBSchemaHelper.C_KEYWORD_UNIQUE;
                if (_defaultValue != null)
                    result += string.Format(" {0} {1}", DBSchemaHelper.C_KEYWORD_DEFAULT, _defaultValue);
            }
            return new string[1] { result };
        }
    };
}
