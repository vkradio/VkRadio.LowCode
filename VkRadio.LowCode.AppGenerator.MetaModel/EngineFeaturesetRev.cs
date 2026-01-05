// TODO: [03] It would be useful to complete a mechanism of change notifications. Now faced in a situation when need to separate an analysis of requirements from load of MetaModel and generation of code (separate to 2 steps), so user will get a question

namespace VkRadio.LowCode.AppGenerator.MetaModel;

/// <summary>
/// Описание набора изменений, привнесенных ревизией движка метамодели
/// </summary>
public class EngineFeaturesetRevDesc
{
    /// <summary>
    /// Unknown affect
    /// </summary>
    const string c_legend_unknown = "?";
    /// <summary>
    /// Not affected
    /// </summary>
    const string c_legend_notAffected = "-";
    /// <summary>
    /// It is recommended to review a MetaModel
    /// </summary>
    const string c_legend_revisionRecommended = "*";
    /// <summary>
    /// Breaking change
    /// </summary>
    const string c_legend_destructive = "!";
    /// <summary>
    /// Legend
    /// </summary>
    static readonly string c_legend = string.Format("Legend: ({0}) - unknown affect; ({1}) - not affected; ({2}) it is recommended to review a MetaModel.\n", c_legend_unknown, c_legend_notAffected, c_legend_revisionRecommended);

    int _rev;
    List<string> _featuresetIncrementDesc;

    static Dictionary<int, EngineFeaturesetRevDesc> _all = default!;
    //static EngineFeaturesetRevDesc _instance;

    static void Init()
    {
        _all = [];
        int rev;
        var desc = new List<string>();
        
        rev = 298;
        desc =
        [
            string.Format("({0}) Added ability to split a MetaModel to packages.", c_legend_unknown),
            string.Format("({0}) Begin to create a SenseRole of a MetaModel (but not concrete implementations).", c_legend_notAffected),
            string.Format("({0}) MetaModels that has a name field can get it from a property Name (only localized for now)." +
                        " [Recommended] Review all MetaModels - extract names to an element Name, and make its extraction in naming fields.", c_legend_revisionRecommended),
            string.Format("({0}) Table relationships OwnerPropertyDefinitionId and TablePropertyDefinitionId renamed to PropertyDefinitionIdInOwner and PropertyDefinitionIdInTable, to exclude a mixing with a similarly named property types of data type definitions.", c_legend_destructive),
        ];

        var one = new EngineFeaturesetRevDesc()
        {
            _rev = rev,
            _featuresetIncrementDesc = desc
        };

        _all.Add(one._rev, one);
    }

    /// <summary>
    /// SVN revision number
    /// </summary>
    public int Rev { get { return _rev; } set { _rev = value; } }

    /// <summary>
    /// Set of added changes
    /// </summary>
    public IList<string> FeaturesetIncrementDesc { get { return _featuresetIncrementDesc; } }

    public static IDictionary<int, EngineFeaturesetRevDesc> All { get { if (_all == null) Init(); return _all; } }
    //public static EngineFeaturesetRevDesc GetEqualOrMaximumLesser(int in_rev)
    //{
    //    IDictionary<int, EngineFeaturesetRevDesc> all = All;
    //    if (all.ContainsKey(in_rev))
    //    {
    //        return all[in_rev];
    //    }
    //    else
    //    {
    //        foreach (int rev in all.Keys)
    //        {
    //            if (rev < in_rev)
    //                return all[rev];
    //        }
    //    }
    //    throw new ApplicationException(string.Format("EqualOrMaximumLesser rev not found for requested rev {0}.", in_rev));
    //}

    public static IDictionary<int, EngineFeaturesetRevDesc> GetAllAbove(int rev)
    {
        var result = new Dictionary<int, EngineFeaturesetRevDesc>();

        foreach (var desc in All)
        {
            if (desc.Key > rev)
            {
                result.Add(desc.Key, desc.Value);
            }
        }

        return result;
    }
}
