using System.Collections.Generic;

using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Field;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Method;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Getter;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Setter;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Component;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;
using ArtefactGenerationProject.ArtefactGenerator.Sql;
using MetaModel.DOTDefinition;
using PackNS = ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Package;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Gui
{
    public class GuiPackage: PackNS.Package
    {
        ElementsPackage _elementsPackage;
        LaunchersPackage _launchersPackage;
        ListsPackage _listsPackage;

        public GuiPackage(CSharpProjectBase in_parentPackage)
            : base(in_parentPackage, "Gui")
        {
            _elementsPackage = new ElementsPackage(this);
            _subpackages.Add(_elementsPackage.Name, _elementsPackage);

            _launchersPackage = new LaunchersPackage(this);
            _subpackages.Add(_launchersPackage.Name, _launchersPackage);

            _listsPackage = new ListsPackage(this);
            _subpackages.Add(_listsPackage.Name, _listsPackage);

            #region Компонент UiRegistry.cs.
            CSComponentWMainClass uiRegistryComp = new CSComponentWMainClass()
            {
                Package = this,
                Name = "UiRegistry.cs",
                Namespace = string.Format("{0}.Gui", ParentPackage.RootNamespace),
            };
            uiRegistryComp.UserUsings.Add("orm.Gui");
            uiRegistryComp.UserUsings.Add(string.Format("{0}.Gui.Launchers", ParentPackage.RootNamespace));
            _components.Add(uiRegistryComp.Name, uiRegistryComp);

            CSClass clsUiRegistry = new CSClass()
            {
                Component = uiRegistryComp,
                DocComment = new XmlComment("Реестр запускальщиков средств UI"),
                Name = "UiRegistry"
            };
            uiRegistryComp.Classes.Add(clsUiRegistry.Name, clsUiRegistry);
            uiRegistryComp.MainClass = clsUiRegistry;

            MetaModel.MetaModel mm = ParentPackage.ParentPackage.DomainModel;
            DBSchemaMetaModelJson dbMM = ParentPackage.ParentPackage.DBbSchemaModel;

            List<CSharpHelper.ClassNameDOTDefPair> classes = new List<CSharpHelper.ClassNameDOTDefPair>();
            foreach (CSComponentWMainClass component in _launchersPackage.Components.Values)
            {
                DOTDefinition thisDotDef = null;
                foreach (DOTDefinition dotDef in mm.AllDOTDefinitions.Values)
                {
                    if (CSharpHelper.GenerateDOTClassName(dotDef) == component.MainClass.Name.Substring(3))
                    {
                        thisDotDef = dotDef;
                        break;
                    }
                }

                classes.Add(new CSharpHelper.ClassNameDOTDefPair()
                {
                    ClassName = component.MainClass.Name,
                    DOTDefinition = thisDotDef
                });
            }

            classes.Sort(CSharpHelper.ClassNameDOTDefPair.Compare);

            CSConstructor ctor = new CSConstructor(clsUiRegistry)
            {
                DocComment = new XmlComment("Закрытый конструктор реестра запускальщиков"),
                Visibility = ElementVisibilityClassic.Private
            };
            clsUiRegistry.Constructors.Add(CSharpHelper.GenerateMethodKey(ctor), ctor);

            CSConstructor ctorStatic = new CSConstructor(clsUiRegistry)
            {
                DocComment = new XmlComment("Статический конструктор реестра запускальщиков"),
                Visibility = ElementVisibilityClassic.Private,
                //AdditionalKeywords = "static",
                HintSingleLineBody = true,
                IsStatic = true,
            };
            ctorStatic.BodyStrings.Add("_instance = new UiRegistry();");
            clsUiRegistry.Constructors.Add(CSharpHelper.GenerateMethodKey(ctorStatic), ctorStatic);

            const string c_docCommentSingleton = "Единственный экземпляр (Singleton)";
            CSClassField field = new CSClassField()
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

            CSProperty prop = new CSProperty()
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

            CSClassField qSelectorField = new CSClassField()
            {
                Class = clsUiRegistry,
                DocComment = new XmlComment("Хранилище быстрого выбора объектов из списков"),
                InitialValue = "new QuickSelectStorage()",
                Name = "_quickSelectStorage",
                TypeKeyword = "QuickSelectStorage",
                Visibility = ElementVisibilityClassic.Private
            };
            clsUiRegistry.Fields.Add(qSelectorField.Name, qSelectorField);

            CSProperty qSelectorProp = new CSProperty()
            {
                Class = clsUiRegistry,
                DocComment = new XmlComment("Хранилище быстрого выбора объектов из списков"),
                Name = "QuickSelectStorage",
                NameFieldCorresponding = "_quickSelectStorage",
                Type = "QuickSelectStorage",
                Visibility = ElementVisibilityClassic.Public
            };
            qSelectorProp.Getter = new CSPropertyGetter(qSelectorProp, true);
            clsUiRegistry.Properties.Add(qSelectorProp.Name, qSelectorProp);

            foreach (CSharpHelper.ClassNameDOTDefPair cls in classes)
            {
                string dotClassName = CSharpHelper.GenerateDOTClassName(cls.DOTDefinition);
                string fieldName = "_uil" + dotClassName;
                string propName = "Uil" + dotClassName;
                string localName = NameHelper.GetLocalNameUpperCase(cls.DOTDefinition.Names);
                string docComment = "Запускальщик графического интерфейса для управления объектами " + localName;

                // Закрытые поля, соответствующие запускальщикам.
                CSClassField uilField = new CSClassField()
                {
                    Class = clsUiRegistry,
                    DocComment = new XmlComment(docComment),
                    Name = fieldName,
                    TypeKeyword = "UILauncher",
                    Visibility = ElementVisibilityClassic.Private
                };
                clsUiRegistry.Fields.Add(uilField.Name, uilField);

                // Конструктор, создающий экземпляры запускальщиков.
                ctor.BodyStrings.Add(string.Format("{0} = new {1}();", fieldName, propName));

                // Публичные свойства, соответствующие запускальщикам.
                CSProperty uilProp = new CSProperty()
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
    };
}
