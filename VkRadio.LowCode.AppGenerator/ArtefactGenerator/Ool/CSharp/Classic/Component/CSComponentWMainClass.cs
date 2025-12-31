using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Component
{
    public class CSComponentWMainClass: CSComponent
    {
        CSClass _mainClass;

        public CSClass MainClass
        {
            get { return _mainClass; }
            set
            {
                _mainClass = value;
                if (!Classes.ContainsKey(_mainClass.Name))
                    Classes.Add(_mainClass.Name, _mainClass);
            }
        }
    };
}
