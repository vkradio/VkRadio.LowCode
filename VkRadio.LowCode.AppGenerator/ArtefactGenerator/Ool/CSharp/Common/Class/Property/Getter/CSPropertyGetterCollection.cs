using System;
using System.Collections.Generic;

using MetaModel.PropertyDefinition;

// TODO: Здесь жестко зашито извлечение коллеций из БД MS SQL, т.е. под MySQL
// и другими серверами это работать не будет. Нужно обобщить.

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Getter
{
    /// <summary>
    /// Геттер для свойства-коллекции (табличная часть и т.д.)
    /// </summary>
    public class CSPropertyGetterCollection: CSPropertyGetter
    {
        PropertyDefinition _propertyDefinition;

        public CSPropertyGetterCollection(CSProperty in_property, PropertyDefinition in_propDef)
            : base(in_property, true)
            => _propertyDefinition = in_propDef;

        public override string[] GenerateText()
        {
            var text = new List<string>();

            if (SingleLineHint)
            {
                var fieldName = NameHelper.NamesToHungarianName(_propertyDefinition.Names, true) + "Filter";
                text.Add($"get => StorageRegistry.Instance.{Property.DOTType}Storage.ReadAsTable({fieldName});");
            }
            else
            {
                throw new NotImplementedException("Generating not-single-line getter for CSPropertyGetterCachedObject not implemented.");
            }

            return text.ToArray();
        }
    };
}
