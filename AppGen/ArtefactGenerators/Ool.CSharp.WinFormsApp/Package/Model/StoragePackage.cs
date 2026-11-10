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
using VkRadio.LowCode.AppGen.Domain;
using VkRadio.LowCode.AppGen.Domain.Names;
using PackNS = VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Package;

// Implementing Singleton in a multithreaded environment:
// http://msdn.microsoft.com/en-us/library/ff650316.aspx

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Model;

public class StoragePackage: PackNS.Package
{
    // TODO: Unify this method with contructor (below) because there are lots of duplicated logic
    public static CSComponentWMainClass CreateStorageRegistryComponent(DomainModel domainModel, PackNS.Package package, string namespaceName, NaturalLanguageEnum preferNaturalLanguageForComments)
    {
        var storageRegistryComponent = new CSComponentWMainClass
        {
            Package = package,
            Name = "StorageRegistry.cs",
            Namespace = namespaceName
        };
        package.Components.Add(storageRegistryComponent.Namespace, storageRegistryComponent);

        var clsStorageRegistry = new CSClass
        {
            Component = storageRegistryComponent,
            DocComment = new XmlComment(preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Реестр хранилищ ОД" : "Data object storage registry"),
            Name = "StorageRegistry"
        };
        storageRegistryComponent.Classes.Add(clsStorageRegistry.Name, clsStorageRegistry);
        storageRegistryComponent.MainClass = clsStorageRegistry;

        storageRegistryComponent.UserUsings.Add("VkRadio.LowCode.Libs.Orm");

        var classDescriptors = new List<CSharpHelper.ClassNameEntityDefPair>();

        foreach (var component in package
            .Components
            .Values
            .Where(c => c.Name != "StorageRegistry.cs")
            .Where(c => c.Name != "DOTs.cs"))
        {
            foreach (var cls in component.Classes.Values)
            {
                var entityClassName = cls.Name[..^7];

                var thisEntDef = domainModel
                    .AllEntityDefinitions
                    .Values
                    .Where(entDef => CSharpHelper.GenerateEntityClassName(entDef) == entityClassName).Single();

                classDescriptors.Add(new CSharpHelper.ClassNameEntityDefPair
                {
                    ClassName = entityClassName,
                    EntityDefinition = thisEntDef
                });
            }
        }

        classDescriptors.Sort(CSharpHelper.ClassNameEntityDefPair.Compare);

        var ctor = new CSConstructor(
            clsStorageRegistry,
            new XmlComment(preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Конструктор реестра хранилищ" : "Constructor of storage registry"),
            ElementVisibilityClassic.Public,
            null,
            null,
            null,
            false,
            false
        );

        var ctorParam = new CSParameter("dbProviderFactory", new CSharpReferenceType(false, "IDbProviderFactory", false));
        ctor.Params.Add(ctorParam.Name, ctorParam);

        clsStorageRegistry.Constructors.Add(CSharpHelper.GenerateMethodKey(ctor), ctor);

        //var ctorStatic = new CSConstructor(clsStorageRegistry)
        //{
        //    DocComment = new XmlComment(preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Статический конструктор реестра хранилищ" : "Static constructor of storage registry"),
        //    HintSingleLineBody = true,
        //    Visibility = ElementVisibilityClassic.Private,
        //    IsStatic = true
        //};
        //ctorStatic.BodyStrings.Add("_instance = new StorageRegistry();");
        //clsStorageRegistry.Constructors.Add(CSharpHelper.GenerateMethodKey(ctorStatic), ctorStatic);

        var initMethod = new CSMethod(
            clsStorageRegistry,
            "SetSingletonInstance",
            null,
            ElementVisibilityClassic.Public,
            null,
            null,
            new List<string> { "_instance = instance;" },
            null,
            true,
            true
        );
        clsStorageRegistry.Methods.Add(initMethod.Name, initMethod);

        var initMethodParam = new CSParameter("instance", new CSharpReferenceType(false, "StorageRegistry", false));
        initMethod.Params.Add(initMethodParam.Name, initMethodParam);

        foreach (var cls in classDescriptors)
        {
            var fieldName = $"_{NameHelper.NamesToCamelCase(cls.EntityDefinition.Names)}Storage";
            var entityLocalName = NameHelper.GetLocalNameUpperCase(cls.EntityDefinition.Names, preferNaturalLanguageForComments);
            var storageClassName = $"{NameHelper.NamesToPascalCase(cls.EntityDefinition.Names)}Storage";
            var storageComment = preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? $"Хранилище {entityLocalName}" : $"{entityLocalName} Storage";

            // 1. Storage field
            var field = new CSClassField(
                new CSharpReferenceType(false, storageClassName, false),
                fieldName,
                clsStorageRegistry,
                new XmlComment(storageComment),
                ElementVisibilityClassic.Private,
                null,
                false
            );
            clsStorageRegistry.Fields.Add(field.Name, field);

            // 2. Create storage in a constructor
            ctor.BodyStrings.Add($"{fieldName} = new {storageClassName}(dbProviderFactory); {fieldName}.Init();");

            // 3. Public property
            var prop = new CSProperty(
                storageClassName,
                field,
                null,
                ElementVisibilityClassic.Private,
                false,
                null,
                null,
                new XmlComment(storageComment),
                null
            );
            prop.SetGetter(new CSPropertyGetter(prop, true));
            prop.SetSetter(new CSPropertySetter(prop, true, false));
            clsStorageRegistry.Properties.Add(prop.Name, prop);
        }

        string singletonComment = preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Единственный экземпляр (Singleton)" : "Singleton";

        var fieldInstance = new CSClassField(
            new CSharpReferenceType(false, "StorageRegistry", false),
            "_instance",
            clsStorageRegistry,
            new XmlComment(singletonComment),
            ElementVisibilityClassic.Private,
            null,
            true
        );
        clsStorageRegistry.Fields.Add(fieldInstance.Name, fieldInstance);

        var instanceProp = new CSPropertyPredefined("Instance", "public static StorageRegistry Instance { get => _instance; }", new XmlComment(singletonComment));
        clsStorageRegistry.Properties.Add(instanceProp.Name, instanceProp);

        return storageRegistryComponent;
    }

    public StoragePackage(ModelPackage parentPackage, NaturalLanguageEnum preferNaturalLanguageForComments, bool supportNullableRefTypes)
        : base(parentPackage, "Storage")
    {
        var domainModel = ParentPackage.ParentPackage.ParentPackage.DomainModel;
        var dbModel = ParentPackage.ParentPackage.ParentPackage.DBbSchemaModel;

        var storageNamespace = string.Format("{0}.Model.Storage", ParentPackage.ParentPackage.RootNamespace);

        // For each definition of data object type create a component with a corresponding class
        var entDefs = domainModel.AllEntityDefinitions.Values;

        foreach (var entDef in entDefs)
        {
            var storageComponent = new Storage(this, entDef, ParentPackage.ParentPackage.RootNamespace, dbModel, preferNaturalLanguageForComments, supportNullableRefTypes);
            _components.Add(storageComponent.Name, storageComponent);
        }

        #region Create a component StorageRegistry.cs.
        var storageRegistryComponent = new CSComponentWMainClass
        {
            Package = this,
            Name = "StorageRegistry.cs",
            Namespace = storageNamespace
        };
        _components.Add(storageRegistryComponent.Namespace, storageRegistryComponent);

        var clsStorageRegistry = new CSClass
        {
            Component = storageRegistryComponent,
            DocComment = new XmlComment(preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Реестр хранилищ ОД" : "Object data storage registry"),
            Name = "StorageRegistry"
        };
        storageRegistryComponent.Classes.Add(clsStorageRegistry.Name, clsStorageRegistry);

        storageRegistryComponent.UserUsings.Add("VkRadio.LowCode.Libs.Orm");

        var classes = new List<CSharpHelper.ClassNameEntityDefPair>();

        foreach (var component in _components.Values.Cast<CSComponentWMainClass>())
        {
            if (component.Name != "StorageRegistry.cs")
            {
                var className = component.MainClass.Name.Substring(0, component.MainClass.Name.Length - 7);

                EntityDefinition? thisEntityDef = null;

                foreach (var entityDef in domainModel.AllEntityDefinitions.Values)
                {
                    if (CSharpHelper.GenerateEntityClassName(entityDef) == className)
                    {
                        thisEntityDef = entityDef;
                        break;
                    }
                }

                classes.Add(new CSharpHelper.ClassNameEntityDefPair()
                {
                    ClassName = className,
                    EntityDefinition = thisEntityDef!
                });
            }
        }

        classes.Sort(CSharpHelper.ClassNameEntityDefPair.Compare);

        var ctor = new CSConstructor(
            clsStorageRegistry,
            new XmlComment(preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Конструктор реестра хранилищ" : "Constructor of storage registry"),
            ElementVisibilityClassic.Public,
            null,
            null,
            null,
            false,
            false
        );
        clsStorageRegistry.Constructors.Add(CSharpHelper.GenerateMethodKey(ctor), ctor);

        var ctorParam = new CSParameter("dbProviderFactory", new CSharpReferenceType(false, "IDbProviderFactory", false));
        ctor.Params.Add(ctorParam.Name, ctorParam);

        //var ctorStatic = new CSConstructor(clsStorageRegistry)
        //{
        //    DocComment = new XmlComment(preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Статический конструктор реестра хранилищ" : "Static constructor of storage registry"),
        //    HintSingleLineBody = true,
        //    Visibility = ElementVisibilityClassic.Private,
        //    //AdditionalKeywords = "static"
        //    IsStatic = true
        //};
        //ctorStatic.BodyStrings.Add("_instance = new StorageRegistry();");
        //clsStorageRegistry.Constructors.Add(CSharpHelper.GenerateMethodKey(ctorStatic), ctorStatic);

        var initMethod = new CSMethod(
            clsStorageRegistry,
            "SetSingletonInstance",
            null,
            ElementVisibilityClassic.Public,
            null,
            null,
            new List<string> { "_instance = instance;" },
            null,
            true,
            true
        );
        clsStorageRegistry.Methods.Add(initMethod.Name, initMethod);

        var initMethodParam = new CSParameter("instance", new CSharpReferenceType(false, "StorageRegistry", false));
        initMethod.Params.Add(initMethodParam.Name, initMethodParam);

        foreach (var cls in classes)
        {
            string
                fieldName = string.Format("_{0}Storage", NameHelper.NamesToCamelCase(cls.EntityDefinition.Names)),
                entityLocalizedName = NameHelper.GetLocalNameUpperCase(cls.EntityDefinition.Names, preferNaturalLanguageForComments),
                storageClassName = string.Format("{0}Storage", NameHelper.NamesToPascalCase(cls.EntityDefinition.Names)),
                storageComment = string.Format((preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Хранилище {0}" : "{0} Storage"), entityLocalizedName);

            // 1. Storage field
            var field = new CSClassField(
                new CSharpReferenceType(false, storageClassName, false),
                fieldName,
                clsStorageRegistry,
                new XmlComment(storageComment),
                ElementVisibilityClassic.Private,
                null,
                false
            );
            clsStorageRegistry.Fields.Add(field.Name, field);

            // 2. Create storage in a constructor
            ctor.BodyStrings.Add(string.Format("{0} = new {1}(dbProviderFactory); {2}.Init();", fieldName, storageClassName, fieldName));

            // 3. Public property
            var prop = new CSProperty(
                storageClassName,
                field,
                clsStorageRegistry.Name,
                ElementVisibilityClassic.Private,
                false,
                null,
                null,
                new XmlComment(storageComment),
                null
            );
            prop.SetGetter(new CSPropertyGetter(prop, true));
            prop.SetSetter(new CSPropertySetter(prop, true, false));
            clsStorageRegistry.Properties.Add(prop.Name, prop);
        }

        string singletonComment = preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Единственный экземпляр (Singleton)" : "Singleton";
        
        var fieldInstance = new CSClassField(
            new CSharpReferenceType(false, "StorageRegistry", false),
            "_instance",
            clsStorageRegistry,
            new XmlComment(singletonComment),
            ElementVisibilityClassic.Private,
            null,
            true
        );
        clsStorageRegistry.Fields.Add(fieldInstance.Name, fieldInstance);

        var instanceProp = new CSPropertyPredefined(
            "Instance",
            "public static StorageRegistry Instance { get { return _instance; } }",
            new XmlComment(singletonComment)
        );
        clsStorageRegistry.Properties.Add(instanceProp.Name, instanceProp);
        #endregion
    }

    new public ModelPackage ParentPackage => (ModelPackage)_parentPackage;
}
