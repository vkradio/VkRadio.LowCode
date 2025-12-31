using System;
using System.Collections.Generic;

using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Field;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Getter;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Setter;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Type;
using MetaModel.PropertyDefinition;
using MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property
{
    public class CSProperty
    {
        protected const string c_tab = "    ";

        protected virtual string GenerateNamePart()
        {
            var additionalKeywords = string.Empty;
            if (IsStatic)
                additionalKeywords += " static";
            if (!string.IsNullOrWhiteSpace(AdditionalKeywords))
                additionalKeywords += " " + AdditionalKeywords;

            return $"public{additionalKeywords} {Type} {Name}";
        }

        /// <summary>
        /// Построение свойства на основе определения свойства метаданных
        /// </summary>
        /// <param name="class">Класс-владелец свойства</param>
        /// <param name="propDef">Определение свойства метаданных</param>
        /// <param name="correspondingField">Соответствующее поле</param>
        /// <param name="docComment">Документирующий комментарий</param>
        public static CSProperty GenerateProperty(CSClass @class, PropertyDefinition propDef, CSClassField correspondingField, XmlComment docComment)
        {
            CSProperty result = null;

            if (!(propDef.FunctionalType is PFTLink))
            {
                result = new CSProperty()
                {
                    Class = @class,
                    DocComment = docComment,
                    Name = NameHelper.NamesToHungarianName(propDef.Names, true),
                    NameFieldCorresponding = correspondingField.Name,
                    Type = correspondingField.TypeKeyword
                };
                result.Getter = new CSPropertyGetter(result);
                result.Setter = new CSPropertySetter(result);
            }
            else
            {
                if (propDef.FunctionalType is PFTConnector ||
                    propDef.FunctionalType is PFTReferenceValue ||
                    propDef.FunctionalType is PFTTableOwner)
                {
                    result = new CSProperty
                    {
                        Class = @class,
                        DocComment = docComment,
                        Name = NameHelper.NamesToHungarianName(propDef.Names, true),
                        NameFieldCorresponding = correspondingField.Name,
                        Type = correspondingField.TypeKeyword
                    };
                    result.Getter = new CSPropertyGetterCachedObject(result);
                    result.Setter = new CSPropertySetterCachedObject(result);
                }
            }

            return result;
        }
        /// <summary>
        /// Построение id-свойства, соответствующего свойству-ссылке, на основе определения
        /// свойства метаданных
        /// </summary>
        /// <param name="class">Класс-владелец свойства</param>
        /// <param name="propDef">Определение свойства метаданных</param>
        /// <param name="correspondingField">Соответствующее поле</param>
        /// <param name="docComment">Документирующий комментарий</param>
        public static CSProperty GeneratePropertyId(CSClass @class, PropertyDefinition propDef, CSClassField correspondingField, XmlComment docComment)
        {
            if (!(propDef.FunctionalType is PFTConnector || propDef.FunctionalType is PFTReferenceValue || propDef.FunctionalType is PFTTableOwner))
                throw new ArgumentException($"in_propDef.FunctionalType is not reference value ({propDef.FunctionalType.GetType().Name} for PropertyDefinition Id {propDef.Id}).");

            var result = new CSProperty
            {
                Class = @class,
                DocComment = docComment,
                Name = NameHelper.NamesToHungarianName(propDef.Names, true) + "Id",
                NameFieldCorresponding = correspondingField.Name,
                Type = correspondingField.TypeKeyword
            };
            result.Getter = new CSPropertyGetter(result);
            result.Setter = new CSPropertySetterCachedObjectId(result);

            return result;
        }
        /// <summary>
        /// Построение свойства-коллекции на основе определения свойства метаданных
        /// </summary>
        /// <param name="class">Класс-владелец свойства</param>
        /// <param name="propDef">Определение свойства метаданных</param>
        public static CSProperty GeneratePropertyCollection(CSClass @class, PropertyDefinition propDef)
        {
            if (!(propDef.FunctionalType is PFTBackReferencedTable || propDef.FunctionalType is PFTTablePart))
                throw new ArgumentException($"in_propDef.FunctionalType is not table/collection ({propDef.FunctionalType.GetType().Name} for PropertyDefinition Id {propDef.Id}).");

            var filterPropName = NameHelper.NamesToHungarianName(propDef.Names, true) + "Filter";
            #region Извлечение наименования поля.
            string fieldName;
            var refTable = propDef.FunctionalType as PFTBackReferencedTable;
            if (refTable != null)
            {
                fieldName = NameHelper.NameToUnderscoreSeparatedName(refTable.RelationshipReference.OwnerPropertyDefinition.Names) + "_id";
            }
            else
            {
                if (propDef.FunctionalType is PFTTablePart tblPart)
                    fieldName = NameHelper.NameToUnderscoreSeparatedName(tblPart.RelationshipTable.PropertyDefinitionInTable.Names) + "_id";
                else
                    throw new GeneratorException($"Unsupported PropertyFunctionalType ({propDef.FunctionalType.GetType().Name}) for collection property Id {propDef.Id}.");
            }
            #endregion
            var filterProp = new CSPropertyPredefined
            {
                Class = @class,
                DocComment = new XmlComment("Фильтр для свойства " + NameHelper.GetLocalNameLowerCase(propDef.Names)),
                Name = filterPropName,
                PredefinedValue = $"public virtual FilterSimple {filterPropName} {{ get {{ return FilterSimple.CreateTableFilter(id, \"{fieldName}\"); }} }}"
            };
            @class.Properties.Add(filterProp.Name, filterProp);

            var result = new CSProperty
            {
                Class = @class,
                DocComment = TypeHelper.GetXmlCommentForTablePropDef(propDef),
                Name = NameHelper.NamesToHungarianName(propDef.Names, true) + "DataTable",
                Type = TypeHelper.PropertyDefinitionToCSType(propDef),
                DOTType = TypeHelper.PropertyDefinitionToCSDOTType(propDef),
                AdditionalKeywords = "virtual"
            };
            result.DocComment.Text += " (чтение таблицы)";
            result.Getter = new CSPropertyGetterCollection(result, propDef);

            return result;
        }

        public CSClass Class { get; set; }
        public CSPropertyGetter Getter { get; set; }
        public CSPropertySetter Setter { get; set; }
        public AbstractDocComment DocComment { get; set; }
        public string Name { get; set; }
        public string NameFieldCorresponding { get; set; }
        public string Type { get; set; }
        public string DOTType { get; set; }
        public ElementVisibilityAbstract Visibility { get; set; }
        public bool IsStatic { get; set; }
        public string AdditionalKeywords { get; set; }

        public bool SingleLine { get => (Getter == null || Getter.SingleLineHint) && (Setter == null || Setter.SingleLineHint); }

        public virtual string[] GenerateText()
        {
            var text = new List<string>();

            if (DocComment != null)
                text.AddRange(DocComment.GenerateText());

            var namePart = GenerateNamePart();

            var getterLines = Getter != null ? Getter.GenerateText() : new string[] { string.Empty };
            var setterLines = Setter != null ? Setter.GenerateText() : new string[] { string.Empty };

            if (SingleLine)
            {
                namePart += " {";
                if (Getter != null)
                    namePart += " " + getterLines[0];
                if (Setter != null)
                    namePart += " " + setterLines[0];
                namePart += " }";
                text.Add(namePart);
            }
            else
            {
                text.Add(namePart);
                text.Add("{");
                if (Getter != null)
                    text.AddRange(getterLines);
                if (Setter != null)
                    text.AddRange(setterLines);
                text.Add("}");
            }

            for (var i = 0; i < text.Count; i++)
                text[i] = c_tab + text[i];

            return text.ToArray();
        }
    };
}
