using System.Text;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Component;

/// <summary>
/// Component with a predefined code content
/// </summary>
public class ComponentWPredefinedCode: Component
{
    protected List<string> _predefinedCode = [];
    protected bool _emitUtf8Bom;
    protected bool _lastLineWNewLine = true;

    public IList<string> PredefinedCode => _predefinedCode;

    public bool EmitUtf8Bom { get => _emitUtf8Bom; set => _emitUtf8Bom = value; }

    public bool LastLineWNewLine { get => _lastLineWNewLine; set => _lastLineWNewLine = value; }

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
