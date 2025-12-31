using System;

namespace MetaModel.PropertyDefinition.SystemFunctionalTypes
{
    /// <summary>
    /// Ссылочный тип.
    /// Позволяет при создании хранить только ссылку (Guid) и в процессе отложенного
    /// связывания доприсваивать собственно значение объекта данных.
    /// </summary>
    public class SRefObject
    {
        Guid _key;
        object _value;

        /// <summary>
        /// Конструктор, инициализирующий только ссылку (Guid)
        /// </summary>
        /// <param name="in_key">Ссылка</param>
        public SRefObject(Guid in_key) { _key = in_key; }

        /// <summary>
        /// Ссылка, или ключ, значения
        /// </summary>
        public Guid Key { get { return _key; } }
        /// <summary>
        /// Объектное значение
        /// </summary>
        public object Value { get { return _value; } set { _value = value; } }
    };
}
