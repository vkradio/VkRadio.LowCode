using System;

namespace MetaModel.PropertyDefinition.SystemFunctionalTypes
{
    /// <summary>
    /// Системный тип для хранения даты и времени.
    /// Отличается от стандартного DateTime тем, что позволяет указывать,
    /// хранит ли тип фиксированное значение даты и времени или его следует
    /// получать из системы в момент обращения (в момент исполнения
    /// бизнес-модели).
    /// </summary>
    public class SDateTime
    {
        DateTime? _fixedValue;
        bool _useModelRuntimeValue;

        /// <summary>
        /// Вариант конструктора для фиксированного значения даты и времени
        /// </summary>
        /// <param name="in_fixedValue">Фиксированное значение даты и времени</param>
        public SDateTime(DateTime in_fixedValue)
        {
            _fixedValue = in_fixedValue;
        }
        /// <summary>
        /// Вариант конструктора для указания, что дата и время извлекаются из
        /// системы в момент исполнения модели
        /// </summary>
        public SDateTime()
        {
            _fixedValue = null;
            _useModelRuntimeValue = true;
        }

        /// <summary>
        /// Фиксированное значение даты и времени.
        /// Примечание: В случае, если фиксированное значение не задано (т.е.
        /// хранится указание на дату и время времени исполнения), при попытке
        /// извлечь фиксированное значение будет выдано исключение.
        /// </summary>
        public DateTime FixedValue
        {
            get { return _fixedValue.Value; }
            set
            {
                _fixedValue = value;
                _useModelRuntimeValue = false;
            }
        }
        /// <summary>
        /// Признак необходимости использования значения времени исполнения модели
        /// </summary>
        public bool UseModelRuntimeValue
        {
            get { return _useModelRuntimeValue; }
            set
            {
                _useModelRuntimeValue = value;
                _fixedValue = _useModelRuntimeValue ? null : (DateTime?)DateTime.Now;
            }
        }
    };
}
