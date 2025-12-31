using System.Collections.Generic;
using System.Linq;

using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular.Component;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Field;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Method;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Getter;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Setter;
using PackNS = ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Package;

// О реализации Singleton в многопоточной среде см. пример здесь:
// http://msdn.microsoft.com/en-us/library/ff650316.aspx

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular.Package.Model
{
    public class StoragePackage: PackNS.Package
    {
        public static CSComponentWMainClass CreateStorageRegistryComponent(MetaModel.MetaModel domainModel, PackNS.Package package, string @namespace)
        {
            var storageRegistryComponent = new CSComponentWMainClass
            {
                Package = package,
                Name = "StorageRegistry.cs",
                Namespace = @namespace
            };
            package.Components.Add(storageRegistryComponent.Namespace, storageRegistryComponent);

            var clsStorageRegistry = new CSClass
            {
                Component = storageRegistryComponent,
                DocComment = new XmlComment("Реестр хранилищ ОД"),
                Name = "StorageRegistry"
            };
            storageRegistryComponent.Classes.Add(clsStorageRegistry.Name, clsStorageRegistry);
            storageRegistryComponent.MainClass = clsStorageRegistry;

            var classDescriptors = new List<CSharpHelper.ClassNameDOTDefPair>();
            foreach (CSComponent component in package.Components.Values
                .Where(c => c.Name != "StorageRegistry.cs")
                .Where(c => c.Name != "DOTs.cs"))
            {
                foreach (CSClass cls in component.Classes.Values)
                {
                    var dotClassName = cls.Name.Substring(0, cls.Name.Length - 7);

                    var thisDotDef = domainModel.AllDOTDefinitions.Values
                        .Where(dotDef => CSharpHelper.GenerateDOTClassName(dotDef) == dotClassName).Single();

                    classDescriptors.Add(new CSharpHelper.ClassNameDOTDefPair
                    {
                        ClassName = dotClassName,
                        DOTDefinition = thisDotDef
                    });
                }
            }

            classDescriptors.Sort(CSharpHelper.ClassNameDOTDefPair.Compare);

            var ctor = new CSConstructor(clsStorageRegistry)
            {
                DocComment = new XmlComment("Закрытый конструктор реестра хранилищ"),
                HintSingleLineBody = false,
                Visibility = ElementVisibilityClassic.Private
            };
            clsStorageRegistry.Constructors.Add(CSharpHelper.GenerateMethodKey(ctor), ctor);

            var ctorStatic = new CSConstructor(clsStorageRegistry)
            {
                DocComment = new XmlComment("Статический конструктор реестра хранилищ"),
                HintSingleLineBody = true,
                Visibility = ElementVisibilityClassic.Private,
                IsStatic = true
            };
            ctorStatic.BodyStrings.Add("_instance = new StorageRegistry();");
            clsStorageRegistry.Constructors.Add(CSharpHelper.GenerateMethodKey(ctorStatic), ctorStatic);

            foreach (var cls in classDescriptors)
            {
                var fieldName = $"_{NameHelper.NamesToCamelCase(cls.DOTDefinition.Names)}Storage";
                var dotLocalName = NameHelper.GetLocalNameUpperCase(cls.DOTDefinition.Names);
                var storageClassName = $"{NameHelper.NamesToHungarianName(cls.DOTDefinition.Names)}Storage";
                var storageComment = $"Хранилище {dotLocalName}";

                // 1. Поле хранилища.
                var field = new CSClassField
                {
                    Class = clsStorageRegistry,
                    DocComment = new XmlComment(storageComment),
                    Name = fieldName,
                    TypeKeyword = storageClassName,
                    Visibility = ElementVisibilityClassic.Private
                };
                clsStorageRegistry.Fields.Add(field.Name, field);

                // 2. Создание хранилища в конструкторе.
                ctor.BodyStrings.Add($"{fieldName} = new {storageClassName}(); {fieldName}.Init();");

                // 3. Публичное свойство.
                var prop = new CSProperty
                {
                    Class = clsStorageRegistry,
                    DocComment = new XmlComment(storageComment),
                    Name = storageClassName,
                    Type = storageClassName,
                    NameFieldCorresponding = fieldName
                };
                prop.Getter = new CSPropertyGetter(prop, true);
                prop.Setter = new CSPropertySetter(prop, true, false);
                clsStorageRegistry.Properties.Add(prop.Name, prop);
            }

            const string c_singletonComment = "Единственный экземпляр (Singleton)";

            var fieldInstance = new CSClassField
            {
                Class = clsStorageRegistry,
                DocComment = new XmlComment(c_singletonComment),
                Name = "_instance",
                TypeKeyword = "StorageRegistry",
                Visibility = ElementVisibilityClassic.Private,
                IsStatic = true
            };
            clsStorageRegistry.Fields.Add(fieldInstance.Name, fieldInstance);

            var instanceProp = new CSPropertyPredefined
            {
                PredefinedValue = "public static StorageRegistry Instance { get => _instance; }",
                Name = "Instance",
                DocComment = new XmlComment(c_singletonComment)
            };
            clsStorageRegistry.Properties.Add(instanceProp.Name, instanceProp);

            return storageRegistryComponent;
        }

        public StoragePackage(ModelPackage in_parentPackage)
            : base(in_parentPackage, "Storage")
        {
            var model = ParentPackage.ParentPackage.ParentPackage.DomainModel;
            var dbModel = ParentPackage.ParentPackage.ParentPackage.DBbSchemaModel;

            var storageNamespace = $"{ParentPackage.ParentPackage.RootNamespace}.Model.Storage";

            // Для каждого определения ТОД создаем компонент с соответствеющим классом.
            foreach (var dotDef in model.AllDOTDefinitions.Values)
            {
                var storageComponent = new Storage(this, dotDef, ParentPackage.ParentPackage.RootNamespace, dbModel);
                _components.Add(storageComponent.Name, storageComponent);
            }

            // Создаем компонент StorageRegistry.cs.
            CreateStorageRegistryComponent(model, this, storageNamespace);
        }

        new public ModelPackage ParentPackage { get => (ModelPackage)_parentPackage; }
    };
}
