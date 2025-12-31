using System.Xml.Linq;

using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common
{
    /// <summary>
    /// Комментарий в формате XML для C#
    /// </summary>
    public class XmlComment: AbstractDocComment
    {
        /// <summary>
        /// Конструктор с инициализацией текстовым содержимым
        /// </summary>
        /// <param name="text">Текст комментария</param>
        public XmlComment(string text) : base(text) {}

        // TODO: Расширить XmlDoc до возможности комментировать параметры и возвращаемое значение.
        public override string[] GenerateText()
        {
            var xel = new XElement("root", _text);
            var encodedText = xel.ToString();
            encodedText = encodedText.Substring(6, encodedText.Length - 13);

            return new string[]
            {
                "/// <summary>",
                "/// " + encodedText,
                "/// </summary>"
            };
        }
    };
}
