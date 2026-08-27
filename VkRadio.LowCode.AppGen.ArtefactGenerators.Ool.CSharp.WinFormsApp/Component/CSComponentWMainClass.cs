using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Component;

public class CSComponentWMainClass : CSComponent
{
    CSClass _mainClass;

    public CSClass MainClass
    {
        get => _mainClass;

        set
        {
            _mainClass = value;

            if (!Classes.ContainsKey(_mainClass.Name))
            {
                Classes.Add(_mainClass.Name, _mainClass);
            }
        }
    }
}
