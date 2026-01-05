using System.Collections.Generic;

using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Field;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Method;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Property;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Getter;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Setter;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Component;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql;
using VkRadio.LowCode.AppGenerator.MetaModel.DOTDefinition;
using PackNS = VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Package;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Gui;

public class GuiPackage : PackNS.Package
{
    ElementsPackage _elementsPackage;
    LaunchersPackage _launchersPackage;
    ListsPackage _listsPackage;

    public GuiPackage(CSharpProjectBase parentPackage)
        : base(parentPackage, "Gui")
    {
        _elementsPackage = new ElementsPackage(this);
        _subpackages.Add(_elementsPackage.Name, _elementsPackage);

        _launchersPackage = new LaunchersPackage(this);
        _subpackages.Add(_launchersPackage.Name, _launchersPackage);

        _listsPackage = new ListsPackage(this);
        _subpackages.Add(_listsPackage.Name, _listsPackage);

        #region UiRegistry.cs component
        var uiRegistryComp = new CSComponentWMainClass
        {
            Package = this,
            Name = "UiRegistry.cs",
            Namespace = string.Format("{0}.Gui", ParentPackage.RootNamespace),
        };
        uiRegistryComp.UserUsings.Add("orm.Gui");
        uiRegistryComp.UserUsings.Add(string.Format("{0}.Gui.Launchers", ParentPackage.RootNamespace));
        _components.Add(uiRegistryComp.Name, uiRegistryComp);

        var clsUiRegistry = new CSClass
        {
            Component = uiRegistryComp,
            DocComment = new XmlComment("Registry of UI launchers"),
            Name = "UiRegistry"
        };
        uiRegistryComp.Classes.Add(clsUiRegistry.Name, clsUiRegistry);
        uiRegistryComp.MainClass = clsUiRegistry;

        var mm = ParentPackage.ParentPackage.DomainModel;
        var dbMM = ParentPackage.ParentPackage.DBbSchemaModel;

        var classes = new List<CSharpHelper.ClassNameDOTDefPair>();
        
        foreach (var component in _launchersPackage.Components.Values)
        {
            DOTDefinition? thisDotDef = null;

            foreach (var dotDef in mm.AllDOTDefinitions.Values)
            {
                if (CSharpHelper.GenerateDOTClassName(dotDef) == component.MainClass.Name.Substring(3))
                {
                    thisDotDef = dotDef;
                    break;
                }
            }

            classes.Add(
                new CSharpHelper.ClassNameDOTDefPair
                {
                    ClassName = component.MainClass.Name,
                    DOTDefinition = thisDotDef
                }
            );
        }

        classes.Sort(CSharpHelper.ClassNameDOTDefPair.Compare);

        var ctor = new CSConstructor(clsUiRegistry)
        {
            DocComment = new XmlComment("Private constructor of UI lauchers registry"),
            Visibility = ElementVisibilityClassic.Private
        };
        clsUiRegistry.Constructors.Add(CSharpHelper.GenerateMethodKey(ctor), ctor);

        var ctorStatic = new CSConstructor(clsUiRegistry)
        {
            DocComment = new XmlComment("Static constructor of UI lauchers registry"),
            Visibility = ElementVisibilityClassic.Private,
            //AdditionalKeywords = "static",
            HintSingleLineBody = true,
            IsStatic = true,
        };
        ctorStatic.BodyStrings.Add("_instance = new UiRegistry();");
        clsUiRegistry.Constructors.Add(CSharpHelper.GenerateMethodKey(ctorStatic), ctorStatic);

        const string c_docCommentSingleton = "Singleton";
        var field = new CSClassField
        {
            Class = clsUiRegistry,
            DocComment = new XmlComment(c_docCommentSingleton),
            //InitialValue = "new UiRegistry()",
            IsStatic = true,
            Name = "_instance",
            TypeKeyword = "UiRegistry",
            Visibility = ElementVisibilityClassic.Private
        };
        clsUiRegistry.Fields.Add(field.Name, field);

        var prop = new CSProperty
        {
            Class = clsUiRegistry,
            DocComment = new XmlComment(c_docCommentSingleton),
            IsStatic = true,
            Name = "Instance",
            NameFieldCorresponding = "_instance",
            Type = "UiRegistry",
            Visibility = ElementVisibilityClassic.Public
        };
        prop.Getter = new CSPropertyGetter(prop, true);
        clsUiRegistry.Properties.Add(prop.Name, prop);

        var qSelectorField = new CSClassField
        {
            Class = clsUiRegistry,
            DocComment = new XmlComment("Quick select storage of objects from lists"),
            InitialValue = "new QuickSelectStorage()",
            Name = "_quickSelectStorage",
            TypeKeyword = "QuickSelectStorage",
            Visibility = ElementVisibilityClassic.Private
        };
        clsUiRegistry.Fields.Add(qSelectorField.Name, qSelectorField);

        var qSelectorProp = new CSProperty
        {
            Class = clsUiRegistry,
            DocComment = new XmlComment("Quick select storage of objects from lists"),
            Name = "QuickSelectStorage",
            NameFieldCorresponding = "_quickSelectStorage",
            Type = "QuickSelectStorage",
            Visibility = ElementVisibilityClassic.Public
        };
        qSelectorProp.Getter = new CSPropertyGetter(qSelectorProp, true);
        clsUiRegistry.Properties.Add(qSelectorProp.Name, qSelectorProp);

        foreach (var cls in classes)
        {
            var dotClassName = CSharpHelper.GenerateDOTClassName(cls.DOTDefinition);
            var fieldName = "_uil" + dotClassName;
            var propName = "Uil" + dotClassName;
            var localName = NameHelper.GetLocalNameUpperCase(cls.DOTDefinition.Names);
            var docComment = "Launcher of GUI for working with objects " + localName;

            // Private fields for launchers
            var uilField = new CSClassField
            {
                Class = clsUiRegistry,
                DocComment = new XmlComment(docComment),
                Name = fieldName,
                TypeKeyword = "UILauncher",
                Visibility = ElementVisibilityClassic.Private
            };
            clsUiRegistry.Fields.Add(uilField.Name, uilField);

            // Constructor that creates launcher instances
            ctor.BodyStrings.Add(string.Format("{0} = new {1}();", fieldName, propName));

            // Public properties for launchers
            var uilProp = new CSProperty
            {
                Class = clsUiRegistry,
                DocComment = new XmlComment(docComment),
                Name = propName,
                NameFieldCorresponding = fieldName,
                Type = "UILauncher",
                Visibility = ElementVisibilityClassic.Public
            };
            uilProp.Getter = new CSPropertyGetter(uilProp, true);
            uilProp.Setter = new CSPropertySetter(uilProp, true, false);
            clsUiRegistry.Properties.Add(uilProp.Name, uilProp);
        }
        #endregion
    }

    public new CSharpProjectBase ParentPackage { get { return (CSharpProjectBase)_parentPackage; } }
    public ElementsPackage ElementsPackage { get { return _elementsPackage; } }
    public LaunchersPackage LaunchersPackage { get { return _launchersPackage; } }
    public ListsPackage ListsPackage { get { return _listsPackage; } }
}
