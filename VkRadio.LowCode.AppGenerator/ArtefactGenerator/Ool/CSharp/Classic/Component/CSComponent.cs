using System.Collections.Generic;
using System.IO;
using System.Text;

using CompNS = ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Component;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Component
{
    public class CSComponent: CompNS.Component
    {
        protected List<string> _systemUsings = new List<string>();
        protected List<string> _userUsings = new List<string>();
        protected string _namespace;

        public override void GenerateComponent()
        {
            List<string> text = new List<string>();

            foreach (string sysUsing in _systemUsings)
                text.Add(string.Format("using {0};", sysUsing));
            if (_systemUsings.Count != 0 && _userUsings.Count != 0)
                text.Add(string.Empty);
            foreach (string usrUsing in _userUsings)
                text.Add(string.Format("using {0};", usrUsing));

            if (_systemUsings.Count != 0 || _userUsings.Count != 0)
                text.Add(string.Empty);

            text.Add("namespace " + _namespace);
            text.Add("{");

            foreach (CSClass cls in Classes.Values)
            {
                text.AddRange(cls.GenerateText());
                text.Add(string.Empty);
            }
            if (Classes.Count != 0)
                text.RemoveAt(text.Count - 1);

            text.Add("}");

            using (StreamWriter sw = new StreamWriter(FullPath, false, new UTF8Encoding(true)))
            {
                foreach (string str in text)
                    sw.WriteLine(str);
            }
        }

        public IList<string> SystemUsings { get { return _systemUsings; } }
        public IList<string> UserUsings { get { return _userUsings; } }
        public string Namespace { get { return _namespace; } set { _namespace = value; } }
    };
}
