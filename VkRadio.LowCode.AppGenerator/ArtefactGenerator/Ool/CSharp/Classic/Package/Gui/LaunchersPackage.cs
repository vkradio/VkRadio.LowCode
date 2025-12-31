using System.Collections.Generic;

using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Component;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Method;
using ArtefactGenerationProject.ArtefactGenerator.Sql;
using PackNS = ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Package;
using MetaModel.DOTDefinition;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Gui
{
    public class LaunchersPackage: PackNS.Package
    {
        public LaunchersPackage(GuiPackage in_parentPackage)
            : base(in_parentPackage, "Launchers")
        {
            MetaModel.MetaModel mm = ParentPackage.ParentPackage.ParentPackage.DomainModel;
            DBSchemaMetaModelJson dbMM = ParentPackage.ParentPackage.ParentPackage.DBbSchemaModel;

            // Для каждого определения ТОД создаем компонент с соответствеющим классом.
            ICollection<DOTDefinition> dotDefs = mm.AllDOTDefinitions.Values;
            foreach (DOTDefinition dotDef in dotDefs)
            {
                string typeName = CSharpHelper.GenerateDOTClassName(dotDef);
                string uilName = "Uil" + typeName;

                CSComponentWMainClass component = new CSComponentWMainClass()
                {
                    Package = this,
                    Name = uilName + ".cs",
                    DOTDefinition = dotDef,
                    Namespace = string.Format("{0}.Gui.Launchers", ParentPackage.ParentPackage.RootNamespace)
                };
                _components.Add(component.Name, component);
                string rootNamespace = ParentPackage.ParentPackage.RootNamespace;
                component.UserUsings.Add("orm.Db");
                component.UserUsings.Add("orm.Gui");
                component.UserUsings.Add(string.Format("{0}.Gui.Elements", rootNamespace));
                component.UserUsings.Add(string.Format("{0}.Gui.Lists", rootNamespace));
                component.UserUsings.Add(string.Format("{0}.Model.Storage", rootNamespace));

                CSClass cls = new CSClass()
                {
                    Component = component,
                    DocComment = new XmlComment("Запускальщик средств UI для объектов " + NameHelper.GetLocalNameUpperCase(dotDef.Names)),
                    Name = uilName,
                    InheritsFrom = "UILauncher"
                };
                component.Classes.Add(cls.Name, cls);
                component.MainClass = cls;

                CSConstructor ctor = new CSConstructor(cls)
                {
                    DocComment = new XmlComment("Конструктор запускальщика"),
                    Visibility = ElementVisibilityClassic.Public
                };
                cls.Constructors.Add(CSharpHelper.GenerateMethodKey(ctor), ctor);
                ctor.BodyStrings.Add(string.Format("_storage = StorageRegistry.Instance.{0}Storage;", typeName));
                ctor.BodyStrings.Add(string.Format("_dotName = \"{0}\";", NameHelper.GetLocalNameUpperCase(dotDef.Names)));

                CSMethod methodCard = new CSMethod()
                {
                    AdditionalKeywords = "override",
                    Class = cls,
                    DocComment = new XmlComment("Создание карточки элемента"),
                    Name = "CreateDOCard",
                    ReturnType = "DOCard",
                    Visibility = ElementVisibilityClassic.Protected
                };
                CSParameter param = new CSParameter() { Name = "in_o", Type = "DbMappedDOT" };
                methodCard.Params.Add(param.Name, param);
                cls.Methods.Add(CSharpHelper.GenerateMethodKey(methodCard), methodCard);
                methodCard.BodyStrings.Add(string.Format("DOP{0} panel = new DOP{0}();", typeName));
                methodCard.BodyStrings.Add("DOCard card = new DOCard(_storage, in_o, _dotName, panel);");
                methodCard.BodyStrings.Add("return card;");

                CSMethod methodList = new CSMethod()
                {
                    AdditionalKeywords = "override",
                    Class = cls,
                    DocComment = new XmlComment("Создание списка элементов"),
                    Name = "CreateDOList",
                    ReturnType = "DOList",
                    Visibility = ElementVisibilityClassic.Protected
                };
                cls.Methods.Add(CSharpHelper.GenerateMethodKey(methodList), methodList);
                methodList.BodyStrings.Add(string.Format("return new DOL{0}();", typeName));
            }
        }

        public new GuiPackage ParentPackage { get { return (GuiPackage)_parentPackage; } }
    };
}
