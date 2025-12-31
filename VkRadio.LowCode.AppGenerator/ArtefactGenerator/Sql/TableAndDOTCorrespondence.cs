using System.Collections.Generic;

using MetaModel.DOTDefinition;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    /// <summary>
    /// Соответствие между таблицей и определением типа объекта данных
    /// </summary>
    public class TableAndDOTCorrespondence : TableAndSourceCorrespondence
    {
        DOTDefinition _dotDefinition;
        List<PropertyCorrespondence> _propertyCorrespondences = new();

        /// <summary>
        /// Определение ТОД
        /// </summary>
        public DOTDefinition DOTDefinition { get { return _dotDefinition; } set { _dotDefinition = value; } }
        /// <summary>
        /// Соответствия полей таблицы свойствам ТОД
        /// </summary>
        public IList<PropertyCorrespondence> PropertyCorrespondences => _propertyCorrespondences;
    }
}
