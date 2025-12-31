using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract;
using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Class;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Field
{
    /// <summary>
    /// Поле класса C#
    /// </summary>
    public class CSClassField: ClassField
    {
        public string TypeKeyword { get; set; }

        protected override string GenerateTextConcrete()
        {
            var addKeywords = string.Empty;
            if (_isStatic)
                addKeywords += "static ";

            return string.Format(
                "    {0}{1}{2} {3}{4};",
                (_visibility.Value != ElementVisibilityEnum.Private ? _visibility.ToString() + " " : string.Empty),
                addKeywords,
                TypeKeyword,
                _name,
                string.IsNullOrEmpty(_initialValue) ? string.Empty : " = " + _initialValue
            );
        }
    };
}
