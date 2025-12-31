using System;
using System.Collections.Generic;

using MetaModel.PropertyDefinition;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    /// <summary>Связь между таблицами по внешнему ключу
    /// <remarks>Этот класс добавлен 27 мая 2015 г. для возможности добивки констрейнтов
    /// специально в конец файла MS SQL</remarks>
    /// </summary>
    public class ForeignKeyConstraint : ITextDefinition
    {
        protected string _quoteSymbol = "\"";

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="in_tableName">Имя таблицы, на которую накладывается ограничение</param>
        /// <param name="in_refTableName">Имя таблицы, на которую ссылается ВК</param>
        /// <param name="in_refFieldName">Имя поля ВК</param>
        /// <param name="in_onDeleteAction">Действие при удалении объекта, на который ссылается ВК</param>
        public ForeignKeyConstraint(string in_tableName, string in_refTableName, string in_refFieldName, OnDeleteActionEnum in_onDeleteAction)
        {
            TableName = in_tableName;
            RefTableName = in_refTableName;
            RefFieldName = in_refFieldName;
            OnDeleteAction = in_onDeleteAction;
        }

        /// <summary>
        /// Имя таблицы, на которую накладывается ограничение
        /// </summary>
        public string TableName { get; private set; }
        /// <summary>
        /// Имя таблицы, на которую ссылается ВК
        /// </summary>
        public string RefTableName { get; private set; }
        /// <summary>
        /// Имя поля ВК
        /// </summary>
        public string RefFieldName { get; private set; }
        /// <summary>
        /// Действие при удалении объекта, на который ссылается ВК
        /// </summary>
        public OnDeleteActionEnum OnDeleteAction { get; private set; }

        public string[] GenerateText()
        {
            List<string> result = new List<string>();
            result.Add(string.Format("alter table {0}{1}{0}", _quoteSymbol, TableName));
            result.Add(string.Format("\tadd constraint {0}fk_{1}_{2}{0}", _quoteSymbol, TableName, RefFieldName));
            result.Add(string.Format("\tforeign key ({0}{1}{0}) references {0}{2}{0} ({0}id{0})", _quoteSymbol, RefFieldName, RefTableName));
            if (OnDeleteAction == OnDeleteActionEnum.CannotDelete)
            {
                result[result.Count - 1] += ";";
            }
            else
            {
                string deleteCommand;
                switch (OnDeleteAction)
                {
                    case OnDeleteActionEnum.Delete:
                        deleteCommand = "cascade";
                        break;
                    case OnDeleteActionEnum.ResetToNull:
                        deleteCommand = "set null";
                        break;
                    case OnDeleteActionEnum.ResetToDefault:
                        deleteCommand = "set default";
                        break;
                    default:
                        throw new ApplicationException(string.Format("ForeignKeyConstraint for table {0}.{1} has unsupported FK delete action: {2}.", TableName, RefFieldName, (int)OnDeleteAction));
                }
                result.Add(string.Format("\ton delete {0};", deleteCommand));
            }
            result.Add("go");

            return result.ToArray();
        }
    };
}
