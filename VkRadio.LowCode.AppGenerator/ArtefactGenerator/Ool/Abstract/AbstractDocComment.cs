namespace ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract
{
    /// <summary>
    /// Абстрактный комментарий в стиле PHPDoc, XMLDoc и т.п.
    /// </summary>
    public abstract class AbstractDocComment
    {
        protected string _text;

        /// <summary>
        /// Конструктор с инициализацией текстовым содержимым
        /// </summary>
        /// <param name="in_text">Текст комментария</param>
        public AbstractDocComment(string in_text) { _text = in_text; }

        /// <summary>
        /// Текст комментария
        /// </summary>
        public string Text { get { return _text; } set { _text = value; } }

        public abstract string[] GenerateText();
    };
}
