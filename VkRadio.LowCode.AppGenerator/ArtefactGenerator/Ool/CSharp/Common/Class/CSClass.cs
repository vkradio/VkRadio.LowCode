using System.Collections.Generic;

using AbstClassNS = ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Class;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Method;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class
{
    /// <summary>
    /// C# class
    /// </summary>
    public class CSClass: AbstClassNS.Class
    {
        const string c_tab = "    ";

        protected Dictionary<string, CSProperty> _properties = new Dictionary<string, CSProperty>();
        protected Dictionary<string, CSConstructor> _constructors = new Dictionary<string, CSConstructor>();

        protected override string[] GenerateClassDocComment()
        {
            var text = base.GenerateClassDocComment();
            
            for (var i = 0; i < text.Length; i++)
                text[i] = c_tab + text[i];

            return text;
        }

        protected override string[] GenerateClassHeader()
        {
            var text = new List<string>();

            var classNameString = $"public{(Partial ? " partial" : string.Empty)} class {_name}";
            if (!string.IsNullOrEmpty(_inheritsFrom))
                classNameString += (": " + _inheritsFrom);
            text.Add(classNameString);
            text.Add("{");

            for (var i = 0; i < text.Count; i++)
                text[i] = c_tab + text[i];

            return text.ToArray();
        }

        protected override string[] GenerateClassBodyLines()
        {
            var text = new List<string>();

            var textEmbeddedClass = new List<string>();
            if (EmbeddedClassPredefined != null)
                textEmbeddedClass.AddRange(EmbeddedClassPredefined.GenerateText());

            var textFields = new List<string>();
            foreach (var field in _fields.Values)
                textFields.AddRange(field.GenerateText());

            var textConstructors = new List<string>();
            foreach (var ctor in _constructors.Values)
                textConstructors.AddRange(ctor.GenerateText());

            var textMethods = new List<string>();
            foreach (var method in _methods.Values)
                textMethods.AddRange(method.GenerateText());

            var textConstants = new List<string>();
            foreach (var constant in _constants.Values)
                textConstants.AddRange(constant.GenerateText());

            var textProperties = new List<string>();
            foreach (var prop in _properties.Values)
                textProperties.AddRange(prop.GenerateText());

            if (textEmbeddedClass.Count != 0)
            {
                text.AddRange(textEmbeddedClass);
                if (textFields.Count != 0 || textMethods.Count != 0 || textConstants.Count != 0 || textProperties.Count != 0)
                    text.Add(string.Empty);
            }
            if (textFields.Count != 0)
            {
                text.AddRange(textFields);
                if (textConstructors.Count != 0 || textMethods.Count != 0 || textConstants.Count != 0 || textProperties.Count != 0)
                    text.Add(string.Empty);
            }
            if (textConstructors.Count != 0)
            {
                text.AddRange(textConstructors);
                if (textMethods.Count != 0 || textConstants.Count != 0 || textProperties.Count != 0)
                    text.Add(string.Empty);
            }
            if (textMethods.Count != 0)
            {
                text.AddRange(textMethods);
                if (textConstants.Count != 0 || textProperties.Count != 0)
                    text.Add(string.Empty);
            }
            if (textConstants.Count != 0)
            {
                text.AddRange(textConstants);
                if (textProperties.Count != 0)
                    text.Add(string.Empty);
            }
            if (textProperties.Count != 0)
            {
                text.AddRange(textProperties);
            }

            for (var i = 0; i < text.Count; i++)
            {
                if (!string.IsNullOrWhiteSpace(text[i]))
                    text[i] = c_tab + text[i];
            }

            return text.ToArray();
        }

        protected override string[] GenerateClassFooter() { return new string[] { c_tab + "};" }; }

        /// <summary>
        /// Встроенный класс для быстрого извлечения ПОД
        /// </summary>
        public CSClassPredefined EmbeddedClassPredefined { get; set; }
        /// <summary>
        /// Является ли представление класса частичным.
        /// </summary>
        public bool Partial { get; set; }
        /// <summary>
        /// Свойства класса
        /// </summary>
        public IDictionary<string, CSProperty> Properties { get { return _properties; } }
        /// <summary>
        /// Конструкторы класса
        /// </summary>
        public IDictionary<string, CSConstructor> Constructors { get { return _constructors; } }
    };
}
