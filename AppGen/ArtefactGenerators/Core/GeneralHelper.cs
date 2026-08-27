using VkRadio.LowCode.AppGen.Domain;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Core;

/// <summary>
/// Miscellaneous helper functionality
/// </summary>
public static class GeneralHelper
{
    static bool IsDefaultReverseOrderForFunctionalType(PropertyFunctionalType pft) => pft is PFTDateTime;

    /// <summary>
    /// Heuristic search of an Entity property to use for order list of entities by default
    /// </summary>
    /// <param name="entityDef">Entity definition</param>
    /// <param name="out_reverseOrder">Reverse order</param>
    /// <returns>Property definition, or null - if it is impossible to find a default property</returns>
    public static PropertyDefinition? GetListSortProperty(EntityDefinition entityDef, out bool out_reverseOrder)
    {
        out_reverseOrder = false;
        PropertyDefinition? sortProperty = null;

        sortProperty = entityDef
            .PropertyDefinitions
            .Values
            .Select(x => new
            {
                PropDef = x,
                Priority = x.ListOrder.HasValue
                    ? 1
                    : (x.FunctionalType is PFTOrderNumber
                        ? 2
                        : (x.FunctionalType is PFTName
                            ? 3
                            : (x.FunctionalType is PFTDateTime
                                ? 4
                                : (x.FunctionalType is PFTString
                                    ? 5
                                    : 6
                    ))))
            })
            .OrderBy(x => x.Priority)
            .Select(x => x.PropDef)
            .FirstOrDefault();

        if (sortProperty is not null)
        {
            out_reverseOrder =
                (sortProperty.ListOrder.HasValue && sortProperty.ListOrder.Value == ListOrderEnum.Desc) ||
                IsDefaultReverseOrderForFunctionalType(sortProperty.FunctionalType);
        }

        return sortProperty;
    }
}
