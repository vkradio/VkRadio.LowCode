using System;
using System.Xml.Linq;

namespace MetaModel.RegisterDefinition
{
    /// <summary>
    /// Определение допустимого источника регистра
    /// </summary>
    public class RegisterSourceDefinition
    {
        RegisterDefinition _registerDefinition;
        DOTDefinition.DOTDefinition _dotDefinition;

        /// <summary>
        /// Определение регистра, в который входит данный источник
        /// </summary>
        public RegisterDefinition RegisterDefinition { get { return _registerDefinition; } set { _registerDefinition = value; } }
        /// <summary>
        /// Определение ТОД допустимого источника регистра
        /// </summary>
        public DOTDefinition.DOTDefinition DOTDefinition { get { return _dotDefinition; } }

        /// <summary>
        /// Загрузка допустимого источника регистра из узла XML
        /// </summary>
        /// <param name="in_metaModel">Метамодель</param>
        /// <param name="in_xel">Узел XML</param>
        /// <returns>Допустимый источник регистра</returns>
        public static RegisterSourceDefinition LoadFromXElement(MetaModel in_metaModel, XElement in_xel)
        {
            Guid dotDefId = new Guid(in_xel.Element("DOTDefinitionId").Value);
            return new RegisterSourceDefinition() { _dotDefinition = in_metaModel.AllDOTDefinitions[dotDefId] };
        }
    };
}
