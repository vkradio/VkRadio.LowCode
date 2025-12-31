using MetaModel.PropertyDefinition;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    /// <summary>
    /// Взаимное соответствие поля таблицы и свойства ТОД
    /// </summary>
    public class PropertyCorrespondence
    {
        ITableField _tableField;
        TableAndDOTCorrespondence _tableAndDOTCorrespondence;
        PropertyDefinition _propertyDefinition;

        /// <summary>
        /// Поле таблицы, для которого установлено соответствие
        /// </summary>
        public ITableField TableField { get { return _tableField; } set { _tableField = value; } }
        /// <summary>
        /// Взаимное соответствие определения ТОД и таблицы
        /// </summary>
        public TableAndDOTCorrespondence TableAndDOTCorrespondence { get { return _tableAndDOTCorrespondence; } set { _tableAndDOTCorrespondence = value; } }
        /// <summary>
        /// Определение свойства ТОД, для которого установлено соответствие
        /// </summary>
        public PropertyDefinition PropertyDefinition { get { return _propertyDefinition; } set { _propertyDefinition = value; } }
    };
}
