using System.Collections.Generic;

using MetaModel.DOTDefinition;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    /// <summary>
    /// Соответствие между таблицей и определением типа объекта данных
    /// </summary>
    public class TableAndDOTCorrespondenceJson: TableAndSourceCorrespondenceJson
    {
        DOTDefinition _dotDefinition;
        List<PropertyCorrespondenceJson> _propertyCorrespondences = new List<PropertyCorrespondenceJson>();

        /// <summary>
        /// Определение ТОД
        /// </summary>
        public DOTDefinition DOTDefinition { get { return _dotDefinition; } set { _dotDefinition = value; } }
        /// <summary>
        /// Соответствия полей таблицы свойствам ТОД
        /// </summary>
        public IList<PropertyCorrespondenceJson> PropertyCorrespondences { get { return _propertyCorrespondences; } }
    };
}
