using System;

namespace MetaModel.PropertyDefinition.SystemFunctionalTypes
{
    /// <summary>
    /// Системный тип для хранения уникального кода (GUID).
    /// Отличается от стандартного Guid тем, что позволяет указывать,
    /// хранит ли тип фиксированное значение Guid или его следует
    /// генерировать в момент обращения (в момент исполнения
    /// бизнес-модели).
    /// </summary>
    public class SGuid
    {
        Guid? _fixedValue;
        bool _generateValueAtRunTime;

        /// <summary>
        /// Вариант конструктора для фиксированного значения даты и времени
        /// </summary>
        /// <param name="in_fixedValue">Фиксированное значение даты и времени</param>
        public SGuid(Guid in_fixedValue)
        {
            _fixedValue = in_fixedValue;
        }
        /// <summary>
        /// Вариант конструктора для указания, что GUID генерируется в момент исполнения модели
        /// </summary>
        public SGuid()
        {
            _fixedValue = null;
            _generateValueAtRunTime = true;
        }

        /// <summary>
        /// Фиксированное значение GUID.
        /// Примечание: В случае, если фиксированное значение не задано (т.е.
        /// хранится указание на генерацию во временя исполнения), при попытке
        /// извлечь фиксированное значение будет выдано исключение.
        /// </summary>
        public Guid FixedValue
        {
            get { return _fixedValue.Value; }
            set
            {
                _fixedValue = value;
                _generateValueAtRunTime = false;
            }
        }
        /// <summary>
        /// Признак необходимости генерации значения во временя исполнения модели
        /// </summary>
        public bool GenerateValueAtRunTime
        {
            get { return _generateValueAtRunTime; }
            set
            {
                _generateValueAtRunTime = value;
                _fixedValue = _generateValueAtRunTime ? null : (Guid?)Guid.NewGuid();
            }
        }
    };
}
