using VkRadio.LowCode.AppGenerator.MetaModel.DOTDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator;

/// <summary>
/// General helpers
/// </summary>
public static class GeneralHelper
{
    private static bool GetDefaultReverseOrderMarkerForFunctionalType(PropertyFunctionalType pft) => pft is PFTDateTime;

    /// <summary>
    /// Heuristical search for a DOT property by which to make a sort of a list of such objects
    /// </summary>
    /// <param name="dotDef">DOT definition</param>
    /// <param name="outReverseOrder">Use reverse order</param>
    /// <returns>DOT definition, or null, if default sort order was not found</returns>
    public static PropertyDefinition? GetListSortProperty(DOTDefinition dotDef, out bool outReverseOrder)
    {
        outReverseOrder = false;

        // Search for a field explicitly marked by ListOrder. If found, that's it.
        PropertyDefinition? sortProperty = dotDef
            .PropertyDefinitions
            .Values
            .FirstOrDefault(x => x.ListOrder.HasValue);

        if (sortProperty is null)
        {
            // Search for
            // - order numbers
            // - name
            // - date-time
            // - any text field
            sortProperty = dotDef
                .PropertyDefinitions
                .Values
                .FirstOrDefault(x => x.FunctionalType is PFTOrderNumber)
            ?? dotDef
                .PropertyDefinitions
                .Values
                .FirstOrDefault(x => x.FunctionalType is PFTName)
            ?? dotDef
                .PropertyDefinitions
                .Values
                .FirstOrDefault(x => x.FunctionalType is PFTDateTime)
            ?? dotDef
                .PropertyDefinitions
                .Values
                .FirstOrDefault(x => x.FunctionalType is PFTString);

            if (sortProperty is not null)
            {
                outReverseOrder = GetDefaultReverseOrderMarkerForFunctionalType(sortProperty.FunctionalType);
            }
        }
        else
        {
            outReverseOrder = sortProperty.ListOrder!.Value == ListOrderEnum.Default
                ? GetDefaultReverseOrderMarkerForFunctionalType(sortProperty.FunctionalType)
                : sortProperty.ListOrder.Value == ListOrderEnum.Desc;
        }

        return sortProperty;
    }
}
