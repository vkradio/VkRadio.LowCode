using System.Text;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Component;

/// <summary>
/// Component with a predefined code content
/// </summary>
public class ComponentWPredefinedCode: Component
{
    protected List<string> _predefinedCode = new List<string>();
    protected bool _emitUtf8Bom;
    protected bool _lastLineWNewLine = true;

    public IList<string> PredefinedCode { get { return _predefinedCode; } }
    public bool EmitUtf8Bom { get { return _emitUtf8Bom; } set { _emitUtf8Bom = value; } }
    public bool LastLineWNewLine { get { return _lastLineWNewLine; } set { _lastLineWNewLine = value; } }

    public override void GenerateComponent()
    {
        if (!(DoNotOverwriteIfAlreadyExists && File.Exists(FullPath)))
        {
            using var sw = new StreamWriter(FullPath, false, new UTF8Encoding(_emitUtf8Bom));

            for (var i = 0; i <= _predefinedCode.Count - 1; i++)
            {
                sw.Write(_predefinedCode[i]);

                if (_lastLineWNewLine || i != _predefinedCode.Count - 1)
                {
                    sw.WriteLine();
                }
            }
        }
    }
}
