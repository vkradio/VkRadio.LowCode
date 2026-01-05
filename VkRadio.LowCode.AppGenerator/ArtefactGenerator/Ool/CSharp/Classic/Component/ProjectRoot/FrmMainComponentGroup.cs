//using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Component;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Component.ProjectRoot;

public class FrmMainComponentGroup
{
    CSComponentWMainClass _mainComponent;

    public FrmMainComponentGroup(CSharpProjectExtension in_package)
    {
        _mainComponent = new CSComponentWMainClass()
        {
            Package = in_package,
            Namespace = in_package.RootNamespace,
            Name = "FRM_Main.cs"
        };

        in_package.Components.Add(_mainComponent.Name, _mainComponent);

        var cls = new CSClass()
        {
            Component = _mainComponent,
            DocComment = new XmlComment("Главная форма приложения"),
            InheritsFrom = "Form",
            Name = "FRM_Main",
            Partial = true
        };
        _mainComponent.Classes.Add(cls.Name, cls);
        _mainComponent.MainClass = cls;
    }
    
    public CSComponentWMainClass MainComponent { get { return _mainComponent; } }
}
