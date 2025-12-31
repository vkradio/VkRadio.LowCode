using System;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    /// <summary>
    /// Вспомогательный функционал для работы со схемой БД
    /// </summary>
    public static class DBSchemaHelper
    {
        /// <summary>
        /// Символ квотирования имен
        /// </summary>
        public const string C_QUOTE_SYMBOL = "`";
        /// <summary>
        /// Ключевое слово default
        /// </summary>
        public const string C_KEYWORD_DEFAULT = "default";
        /// <summary>
        /// Ключевое слово unique
        /// </summary>
        public const string C_KEYWORD_UNIQUE = "unique";
        /// <summary>
        /// Длина табуляции
        /// </summary>
        public const int C_TAB_LEN = 4;
        /// <summary>
        /// Строка табуляции
        /// </summary>
        public const string C_TAB = "    ";

        /// <summary>
        /// Преобразование значения Guid в строковое значение binary(16) для использования в MySQL
        /// </summary>
        /// <param name="in_guid">Значение типа Guid</param>
        /// <returns>Строковое представление</returns>
        public static string GuidToMySqlValueString(Guid in_guid)
        {
            return "x'" + GuidToHexString(in_guid) + "'";
        }
        /// <summary>
        /// Преобразование значения Guid в строковое значение uniqueidentifier для использования в MS SQL
        /// </summary>
        /// <param name="in_guid">Значение типа Guid</param>
        /// <returns>Строковое представление</returns>
        public static string GuidToMsSqlValueString(Guid in_guid)
        {
            return "'" + in_guid.ToString("D") + "'";
        }
        public static string GuidToHexString(Guid in_guid)
        {
            byte[] guidBytes = in_guid.ToByteArray();
            string guidString = string.Empty;
            foreach (byte b in guidBytes)
                guidString += b.ToString("X2");
            return guidString;
        }
    };
}
