using MetaModel.Names;

namespace MetaModel.PropertyDefinition
{
    /// <summary>
    /// Определение свойства
    /// </summary>
    public interface IPropertyDefinition: IUniqueNamed
    {
        /// <summary>
        /// Функциональный тип свойства
        /// </summary>
        PropertyFunctionalType FunctionalType { get; }
        /// <summary>
        /// Признак упорядочения объектов в списке
        /// </summary>
        ListOrderEnum? ListOrder { get; set; }
    };
}
