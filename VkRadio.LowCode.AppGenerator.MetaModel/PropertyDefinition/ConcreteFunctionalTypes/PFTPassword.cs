using System;
using System.Xml.Linq;

using MetaModel.Names;

namespace MetaModel.PropertyDefinition.ConcreteFunctionalTypes
{
    /// <summary>
    /// Функциональный тип свойства - пароль
    /// </summary>
    public class PFTPassword: PFTString
    {
        PasswordStoreAs _storeAs;

        /// <summary>
        /// Конструктор функционального типа свойства - пароля
        /// </summary>
        public PFTPassword()
        {
            _defaultValue       = null;
            _nullable           = false;
            _quantitative       = false;
            _stringCode         = C_STRING_CODE;
            _unique             = false;
            _defaultMaxLength   = 255;
            _defaultMinLength   = 8;
            _maxLength          = 255;
            _minLength          = 8;
            _storeAs            = PasswordStoreAs.Md5;

            _defaultNames.Add(HumanLanguageEnum.En, C_STRING_CODE);
            _defaultNames.Add(HumanLanguageEnum.Ru, "пароль");
        }

        /// <summary>
        /// Строковый код фунционального типа свойства (используется в файле метамодели)
        /// </summary>
        public const string C_STRING_CODE = "password";

        /// <summary>
        /// Способ хранения пароля в БД
        /// </summary>
        public PasswordStoreAs StoreAs { get { return _storeAs; } set { _storeAs = value; } }

        protected override void InitExtendedParams(XElement in_xelPropertyDefinition)
        {
            base.InitExtendedParams(in_xelPropertyDefinition);

            XElement xel = in_xelPropertyDefinition.Element("StoreAs");
            if (xel != null)
            {
                string strStoreAs = xel.Value;
                switch (strStoreAs)
                {
                    case "open":
                        _storeAs = PasswordStoreAs.Open;
                        break;
                    case "md5":
                        _storeAs = PasswordStoreAs.Md5;
                        break;
                    default:
                        throw new ApplicationException(string.Format("Unsupported StoreAs code for password property Id {0}: \"{1}\".", in_xelPropertyDefinition.Element("Id").Value, strStoreAs));
                }
            }
        }
    };

    /// <summary>
    /// Способ хранения пароля в БД
    /// </summary>
    public enum PasswordStoreAs
    {
        /// <summary>
        /// Хранить в открытом виде
        /// </summary>
        Open,
        // Хранить в виде хэша md5
        Md5
    };
}
