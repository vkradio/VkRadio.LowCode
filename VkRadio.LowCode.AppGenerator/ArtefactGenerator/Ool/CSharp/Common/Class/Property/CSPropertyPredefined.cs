using System.Collections.Generic;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property
{
    /// <summary>
    /// Свойство C# с полностью заранее заданным кодом в виде одной строки
    /// (на самом деле это быстрый грязный хак).
    /// </summary>
    public class CSPropertyPredefined: CSProperty
    {
        public override string[] GenerateText()
        {
            var text = new List<string>();

            if (DocComment != null)
                text.AddRange(DocComment.GenerateText());

            text.Add(PredefinedValue);

            for (var i = 0; i < text.Count; i++)
                text[i] = c_tab + text[i];

            return text.ToArray();
        }

        public string PredefinedValue { get; set; }
    };
}
