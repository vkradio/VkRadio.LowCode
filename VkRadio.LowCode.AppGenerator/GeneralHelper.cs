using MetaModel.DOTDefinition;
using MetaModel.PropertyDefinition;
using MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator;

/// <summary>
/// Различные вспомогательные функции.
/// </summary>
public static class GeneralHelper
{
    static bool GetDefaultReverseOrderMarkerForFunctionalType(PropertyFunctionalType in_pft)
    {
        return in_pft is PFTDateTime;
    }

    /// <summary>
    /// Эвристический поиск свойства ТОД, по которому следует по умолчанию упорядочивать
    /// список ТОД.
    /// </summary>
    /// <param name="in_dotDef">Определение ТОД</param>
    /// <param name="out_reverseOrder">Признак обратного порядка сортировки</param>
    /// <returns>Определение свойства ТОД или null, если сортировку по умолчанию выявить эвристически не удается</returns>
    public static PropertyDefinition GetListSortProperty(DOTDefinition in_dotDef, out bool out_reverseOrder)
    {
        out_reverseOrder = false;
        PropertyDefinition sortProperty = null;

        // 1. Ищем поле с маркером ListOrder. Если оно есть, то именно по нему следует
        //    упорядочивать объекты.
        foreach (PropertyDefinition propDef in in_dotDef.PropertyDefinitions.Values)
        {
            if (propDef.ListOrder.HasValue)
            {
                sortProperty = propDef;
                break;
            }
        }

        if (sortProperty == null)
        {
            // 1. Ищем поле порядкового номера.
            foreach (PropertyDefinition propDef in in_dotDef.PropertyDefinitions.Values)
            {
                if (propDef.FunctionalType is PFTOrderNumber)
                {
                    sortProperty = propDef;
                    out_reverseOrder = GetDefaultReverseOrderMarkerForFunctionalType(propDef.FunctionalType);
                    break;
                }
            }
            // 2. Ищем поле имени.
            if (sortProperty == null)
            {
                foreach (PropertyDefinition propDef in in_dotDef.PropertyDefinitions.Values)
                {
                    if (propDef.FunctionalType is PFTName)
                    {
                        sortProperty = propDef;
                        out_reverseOrder = GetDefaultReverseOrderMarkerForFunctionalType(propDef.FunctionalType);
                        break;
                    }
                }
            }
            // 3. Ищем поле даты-времени.
            if (sortProperty == null)
            {
                foreach (PropertyDefinition propDef in in_dotDef.PropertyDefinitions.Values)
                {
                    if (propDef.FunctionalType is PFTDateTime)
                    {
                        sortProperty = propDef;
                        out_reverseOrder = GetDefaultReverseOrderMarkerForFunctionalType(propDef.FunctionalType);
                        break;
                    }
                }
            }
            // 4. Если ничего не найдено, ищем первое попавшееся текстовое поле.
            if (sortProperty == null)
            {
                foreach (PropertyDefinition propDef in in_dotDef.PropertyDefinitions.Values)
                {
                    if (propDef.FunctionalType is PFTString)
                    {
                        sortProperty = propDef;
                        out_reverseOrder = GetDefaultReverseOrderMarkerForFunctionalType(propDef.FunctionalType);
                        break;
                    }
                }
            }
        }
        else
        {
            if (sortProperty.ListOrder.Value == ListOrderEnum.Default)
                out_reverseOrder = GetDefaultReverseOrderMarkerForFunctionalType(sortProperty.FunctionalType);
            else
                out_reverseOrder = sortProperty.ListOrder.Value == ListOrderEnum.Desc;
        }

        return sortProperty;
    }
};
