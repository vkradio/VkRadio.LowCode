using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using CompNS = ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Component;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular.Component
{
    public class CSComponent: CompNS.Component
    {
        protected List<string> _systemUsings = new List<string>();
        protected List<string> _userUsings = new List<string>();

        public override void GenerateComponent()
        {
            var text = new List<string>();

            foreach (var sysUsing in _systemUsings)
                text.Add($"using {sysUsing};");
            if (_systemUsings.Count != 0 && _userUsings.Count != 0)
                text.Add(string.Empty);
            foreach (var usrUsing in _userUsings)
                text.Add($"using {usrUsing};");

            if (_systemUsings.Count != 0 || _userUsings.Count != 0)
                text.Add(string.Empty);

            text.Add("namespace " + Namespace);
            text.Add("{");

            foreach (var cls in Classes.Values.OrderBy(c => c.Name))
            {
                text.AddRange(cls.GenerateText());
                text.Add(string.Empty);
            }
            if (Classes.Count != 0)
                text.RemoveAt(text.Count - 1);

            text.Add("}");

            using (var sw = new StreamWriter(FullPath, false, new UTF8Encoding(true)))
            {
                foreach (var str in text)
                    sw.WriteLine(str);
            }
        }

        public IList<string> SystemUsings { get { return _systemUsings; } }
        public IList<string> UserUsings { get { return _userUsings; } }
        public string Namespace { get; set; }
    };
}
