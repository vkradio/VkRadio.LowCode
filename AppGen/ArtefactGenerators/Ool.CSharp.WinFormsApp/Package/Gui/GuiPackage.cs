using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Field;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Method;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property.Getter;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property.Setter;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Component;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Root;
using VkRadio.LowCode.AppGen.Domain;
using VkRadio.LowCode.AppGen.Domain.Names;
using PackNS = VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Package;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Gui;

public class GuiPackage : PackNS.Package
{
    ElementsPackage _elementsPackage;
    LaunchersPackage _launchersPackage;
    ListsPackage _listsPackage;

    public GuiPackage(CSharpProjectBase parentPackage, NaturalLanguageEnum preferNaturalLanguageForComments, NaturalLanguageEnum uiLanguage, bool supportHiDpi)
        : base(parentPackage, "Gui")
    {
        _elementsPackage = new ElementsPackage(this, preferNaturalLanguageForComments, uiLanguage, parentPackage.ParentPackage.Generator.SupportNullableReferenceTypes);
        _subpackages.Add(_elementsPackage.Name, _elementsPackage);

        _launchersPackage = new LaunchersPackage(this, preferNaturalLanguageForComments, uiLanguage);
        _subpackages.Add(_launchersPackage.Name, _launchersPackage);

        _listsPackage = new ListsPackage(this, preferNaturalLanguageForComments, uiLanguage, supportHiDpi);
        _subpackages.Add(_listsPackage.Name, _listsPackage);

        #region UiRegistry.cs component
        var uiRegistryComp = new CSComponentWMainClass
        {
            Package = this,
            Name = "UiRegistry.cs",
            Namespace = string.Format("{0}.Gui", ParentPackage.RootNamespace),
        };
        var userUsings = new string[] { "VkRadio.LowCode.Libs.Gui.WinForms", string.Format("{0}.Gui.Launchers", ParentPackage.RootNamespace) }.OrderBy(x => x);

        foreach (var uu in userUsings)
        {
            uiRegistryComp.UserUsings.Add(uu);
        }

        _components.Add(uiRegistryComp.Name, uiRegistryComp);

        var clsUiRegistry = new CSClass
        {
            Component = uiRegistryComp,
            DocComment = new XmlComment(preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Реестр запускальщиков средств UI" : "Registry of UI launchers"),
            Name = "UiRegistry"
        };
        uiRegistryComp.Classes.Add(clsUiRegistry.Name, clsUiRegistry);
        uiRegistryComp.MainClass = clsUiRegistry;

        var mm = ParentPackage.ParentPackage.DomainModel;
        var dbMM = ParentPackage.ParentPackage.DBbSchemaModel;

        var classes = new List<CSharpHelper.ClassNameEntityDefPair>();
        
        foreach (var component in _launchersPackage.Components.Values.Cast<CSComponentWMainClass>())
        {
            EntityDefinition? thisEntityDef = null;

            foreach (var entDef in mm.AllEntityDefinitions.Values)
            {
                if (CSharpHelper.GenerateEntityClassName(entDef) == component.MainClass.Name[3..])
                {
                    thisEntityDef = entDef;
                    break;
                }
            }

            classes.Add(
                new CSharpHelper.ClassNameEntityDefPair
                {
                    ClassName = component.MainClass.Name,
                    EntityDefinition = thisEntityDef
                }
            );
        }

        classes.Sort(CSharpHelper.ClassNameEntityDefPair.Compare);

        var ctor = new CSConstructor(
            clsUiRegistry,
            new XmlComment(preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Закрытый конструктор реестра запускальщиков" : "Private constructor of UI lauchers registry"),
            ElementVisibilityClassic.Private,
            null,
            null,
            null,
            false,
            false
        );
        clsUiRegistry.Constructors.Add(CSharpHelper.GenerateMethodKey(ctor), ctor);

        var ctorStatic = new CSConstructor(
            clsUiRegistry,
            new XmlComment(preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Статический конструктор реестра запускальщиков" : "Static constructor of UI lauchers registry"),
            ElementVisibilityClassic.Private,
            null,
            null,
            null,
            true,
            true
        );
        ctorStatic.BodyStrings.Add("_instance = new UiRegistry();");
        clsUiRegistry.Constructors.Add(CSharpHelper.GenerateMethodKey(ctorStatic), ctorStatic);

        var docCommentSingleton = preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Единственный экземпляр (Singleton)" : "Singleton";
        var field = new CSClassField(
            new CSharpReferenceType(false, "UiRegistry", false),
            "_instance",
            clsUiRegistry,
            new XmlComment(docCommentSingleton),
            ElementVisibilityClassic.Private,
            null,
            true
        );
        clsUiRegistry.Fields.Add(field.Name, field);

        var prop = new CSProperty(
            "Instance",
            field,
            null,
            ElementVisibilityClassic.Public,
            true,
            null,
            null,
            new XmlComment(docCommentSingleton),
            null
        );
        prop.SetGetter(new CSPropertyGetter(prop, true));
        clsUiRegistry.Properties.Add(prop.Name, prop);

        var qSelectorField = new CSClassField(
            new CSharpReferenceType(false, "QuickSelectStorage", false),
            "_quickSelectStorage",
            clsUiRegistry,
            new XmlComment(preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Хранилище быстрого выбора объектов из списков" : "Quick select storage of objects from lists"),
            ElementVisibilityClassic.Private,
            "new QuickSelectStorage()",
            false
        );
        clsUiRegistry.Fields.Add(qSelectorField.Name, qSelectorField);

        var qSelectorProp = new CSProperty(
            "QuickSelectStorage",
            qSelectorField,
            null,
            ElementVisibilityClassic.Public,
            false,
            null,
            null,
            new XmlComment(preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Хранилище быстрого выбора объектов из списков" : "Quick select storage of objects from lists"),
            null
        );
        qSelectorProp.SetGetter(new CSPropertyGetter(qSelectorProp, true));
        clsUiRegistry.Properties.Add(qSelectorProp.Name, qSelectorProp);

        foreach (var cls in classes)
        {
            var dotClassName = CSharpHelper.GenerateEntityClassName(cls.EntityDefinition);
            var fieldName = "_uil" + dotClassName;
            var propName = "Uil" + dotClassName;
            var localName = NameHelper.GetLocalNameUpperCase(cls.EntityDefinition.Names, preferNaturalLanguageForComments);
            var docComment = (preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Запускальщик графического интерфейса для управления объектами " : "Launcher of GUI for working with objects ") + localName;

            // Private fields for launchers
            var uilField = new CSClassField(
                new CSharpReferenceType(false, "UILauncher", false),
                fieldName,
                clsUiRegistry,
                new XmlComment(docComment),
                ElementVisibilityClassic.Private,
                null,
                false
            );
            clsUiRegistry.Fields.Add(uilField.Name, uilField);

            // Constructor that creates launcher instances
            ctor.BodyStrings.Add(string.Format("{0} = new {1}();", fieldName, propName));

            // Public properties for launchers
            var uilProp = new CSProperty(
                propName,
                uilField,
                null,
                ElementVisibilityClassic.Public,
                false,
                null,
                null,
                new XmlComment(docComment),
                null
            );
            uilProp.SetGetter(new CSPropertyGetter(uilProp, true));
            uilProp.SetSetter(new CSPropertySetter(uilProp, true, false));
            clsUiRegistry.Properties.Add(uilProp.Name, uilProp);
        }
        #endregion
    }

    public new CSharpProjectBase ParentPackage => (CSharpProjectBase)_parentPackage;

    public ElementsPackage ElementsPackage => _elementsPackage;

    public LaunchersPackage LaunchersPackage => _launchersPackage;

    public ListsPackage ListsPackage => _listsPackage;
}
