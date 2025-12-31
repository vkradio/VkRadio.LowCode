using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetaModel.PropertyDefinition
{
    /// <summary>
    /// Порядок сортировки списка объектов.
    /// </summary>
    public enum ListOrderEnum
    {
        /// <summary>
        /// По возрастанию значений
        /// </summary>
        Asc,
        /// <summary>
        /// По убыванию значений
        /// </summary>
        Desc,
        /// <summary>
        /// По умолчанию, принятому для данного функционального типа свойств
        /// </summary>
        Default
    };
}
