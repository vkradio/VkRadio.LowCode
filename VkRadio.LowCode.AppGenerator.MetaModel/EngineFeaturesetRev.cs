using System.Collections.Generic;

// TODO: [03] Было бы полезно доделать механизм оповещений об изменениях. Пока воткнулся в то, что нужно отделить анализ изменений от загрузки ММ и генерации кода (разделить на 2 шага), так чтобы пользователю выдавался вопрос.

namespace MetaModel
{
    /// <summary>
    /// Описание набора изменений, привнесенных ревизией движка метамодели
    /// </summary>
    public class EngineFeaturesetRevDesc
    {
        /// <summary>
        /// Влияние неизвестно
        /// </summary>
        const string c_legend_unknown = "?";
        /// <summary>
        /// Не влияет
        /// </summary>
        const string c_legend_notAffected = "-";
        /// <summary>
        /// Рекомендуется пересмотреть метамодель
        /// </summary>
        const string c_legend_revisionRecommended = "*";
        /// <summary>
        /// Разрушительное изменение.
        /// </summary>
        const string c_legend_destructive = "!";
        /// <summary>
        /// Легенда
        /// </summary>
        static readonly string c_legend = string.Format("Легенда: ({0}) - влияние неизвестно; ({1}) - не влияет; ({2}) рекомендуется пересмотреть метамодель.\n", c_legend_unknown, c_legend_notAffected, c_legend_revisionRecommended);

        int _rev;
        List<string> _featuresetIncrementDesc;

        static Dictionary<int, EngineFeaturesetRevDesc> _all;
        //static EngineFeaturesetRevDesc _instance;

        static void Init()
        {
            _all = new Dictionary<int, EngineFeaturesetRevDesc>();
            int rev;
            List<string> desc = new List<string>();
            
            rev = 298;
            desc = new List<string>();
            desc.Add(string.Format("({0}) Добавлена возможность деления ПОБ на пакеты (Package).", c_legend_unknown));
            desc.Add(string.Format("({0}) Намечена смысловая роль ТОД, но пока без привязок (SenseRole).", c_legend_notAffected));
            desc.Add(string.Format("({0}) ПОД, имеющие именующее поле, могут брать его значение из свойства Name (пока только локализованное)." +
                        " [Рекомендуется] Пересмотреть все ПОД - вынести имена в тег Name и сделать его извлечение в именующих полях.", c_legend_revisionRecommended));
            desc.Add(string.Format("({0}) В отношениях типа table свойства OwnerPropertyDefinitionId и TablePropertyDefinitionId переименованы соответственно в PropertyDefinitionIdInOwner и PropertyDefinitionIdInTable, чтобы исключить путаницу с похожими наименованиями типов свойств ТОД.", c_legend_destructive));
            EngineFeaturesetRevDesc one = new EngineFeaturesetRevDesc() { _rev = rev, _featuresetIncrementDesc = desc };
            _all.Add(one._rev, one);
        }

        /// <summary>
        /// Номер ревизии SVN
        /// </summary>
        public int Rev { get { return _rev; } set { _rev = value; } }
        /// <summary>
        /// Набор добавленных изменений
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
        public static IDictionary<int, EngineFeaturesetRevDesc> GetAllAbove(int in_rev)
        {
            Dictionary<int, EngineFeaturesetRevDesc> result = new Dictionary<int, EngineFeaturesetRevDesc>();

            foreach (var desc in All)
                if (desc.Key > in_rev)
                    result.Add(desc.Key, desc.Value);

            return result;
        }
    };
}
