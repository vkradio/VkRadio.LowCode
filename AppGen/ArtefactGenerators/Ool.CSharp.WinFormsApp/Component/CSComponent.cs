using System.Text;
using CompNS = VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Component;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Component;

public class CSComponent : CompNS.Component
{
    protected List<string> _systemUsings = [];
    protected List<string> _userUsings = [];
    protected string _namespace;

    public override void GenerateComponent()
    {
        var text = new List<string>();

        foreach (var sysUsing in _systemUsings)
        {
            text.Add(string.Format("using {0};", sysUsing));
        }

        if (_systemUsings.Count != 0 && _userUsings.Count != 0)
        {
            text.Add(string.Empty);
        }

        foreach (var usrUsing in _userUsings)
        {
            text.Add(string.Format("using {0};", usrUsing));
        }

        if (_systemUsings.Count != 0 || _userUsings.Count != 0)
        {
            text.Add(string.Empty);
        }

        text.Add("namespace " + _namespace);
        text.Add("{");

        foreach (var cls in Classes.Values)
        {
            text.AddRange(cls.GenerateText());
            text.Add(string.Empty);
        }

        if (Classes.Count != 0)
        {
            text.RemoveAt(text.Count - 1);
        }

        text.Add("}");

        using var sw = new StreamWriter(FullPath, false, new UTF8Encoding(true));

        foreach (var str in text)
        {
            sw.WriteLine(str);
        }
    }

    public IList<string> SystemUsings => _systemUsings;

    public IList<string> UserUsings => _userUsings;

    public string Namespace { get => _namespace; set => _namespace = value; }
}
