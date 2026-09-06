using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Method;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Component;
using VkRadio.LowCode.AppGen.Domain.Names;
using PackNS = VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Package;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Gui;

public class LaunchersPackage : PackNS.Package
{
    public LaunchersPackage(GuiPackage in_parentPackage)
        : base(in_parentPackage, "Launchers")
    {
        var mm = ParentPackage.ParentPackage.ParentPackage.DomainModel;
        var dbMM = ParentPackage.ParentPackage.ParentPackage.DBbSchemaModel;

        // For each data object type definition create a component with a corresponding class
        var entityDefs = mm.AllEntityDefinitions.Values;

        foreach (var entDef in entityDefs)
        {
            var typeName = CSharpHelper.GenerateEntityClassName(entDef);
            var uilName = "Uil" + typeName;

            var component = new CSComponentWMainClass
            {
                Package = this,
                Name = uilName + ".cs",
                EntityDefinition = entDef,
                Namespace = string.Format("{0}.Gui.Launchers", ParentPackage.ParentPackage.RootNamespace)
            };
            _components.Add(component.Name, component);
            var rootNamespace = ParentPackage.ParentPackage.RootNamespace;
            component.UserUsings.Add("orm.Db");
            component.UserUsings.Add("orm.Gui");
            component.UserUsings.Add(string.Format("{0}.Gui.Elements", rootNamespace));
            component.UserUsings.Add(string.Format("{0}.Gui.Lists", rootNamespace));
            component.UserUsings.Add(string.Format("{0}.Model.Storage", rootNamespace));

            var cls = new CSClass
            {
                Component = component,
                DocComment = new XmlComment("UI launcher for objects " + NameHelper.GetLocalNameUpperCase(entDef.Names)),
                Name = uilName,
                InheritsFrom = "UILauncher"
            };
            component.Classes.Add(cls.Name, cls);
            component.MainClass = cls;

            var ctor = new CSConstructor(cls)
            {
                DocComment = new XmlComment("Launcher constructor"),
                Visibility = ElementVisibilityClassic.Public
            };
            cls.Constructors.Add(CSharpHelper.GenerateMethodKey(ctor), ctor);
            ctor.BodyStrings.Add(string.Format("_storage = StorageRegistry.Instance.{0}Storage;", typeName));
            ctor.BodyStrings.Add(string.Format("_dotName = \"{0}\";", NameHelper.GetLocalNameUpperCase(entDef.Names)));

            var methodCard = new CSMethod
            {
                AdditionalKeywords = "override",
                Class = cls,
                DocComment = new XmlComment("Create a card of an element (data object)"),
                Name = "CreateDOCard",
                ReturnType = "DOCard",
                Visibility = ElementVisibilityClassic.Protected
            };
            var param = new CSParameter
            {
                Name = "in_o",
                Type = "DbMappedDOT"
            };
            methodCard.Params.Add(param.Name, param);
            cls.Methods.Add(CSharpHelper.GenerateMethodKey(methodCard), methodCard);
            methodCard.BodyStrings.Add(string.Format("DOP{0} panel = new DOP{0}();", typeName));
            methodCard.BodyStrings.Add("DOCard card = new DOCard(_storage, in_o, _dotName, panel);");
            methodCard.BodyStrings.Add("return card;");

            var methodList = new CSMethod
            {
                AdditionalKeywords = "override",
                Class = cls,
                DocComment = new XmlComment("Create a list of elements (data objects)"),
                Name = "CreateDOList",
                ReturnType = "DOList",
                Visibility = ElementVisibilityClassic.Protected
            };
            cls.Methods.Add(CSharpHelper.GenerateMethodKey(methodList), methodList);
            methodList.BodyStrings.Add(string.Format("return new DOL{0}();", typeName));
        }
    }

    public new GuiPackage ParentPackage => (GuiPackage)_parentPackage;
}
