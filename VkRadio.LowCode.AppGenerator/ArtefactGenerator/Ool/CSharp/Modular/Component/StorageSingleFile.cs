using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular.Package.Model;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular.Component
{
    public class StorageSingleFile : CSComponent
    {
        public StorageSingleFile(ModelPackage package)
        {
            Package = package;
            Name = "Storages.cs";
            Namespace = $"{package.ParentPackage.RootNamespace}.Model.Storage";

            SystemUsings.Add("System");
            SystemUsings.Add("System.Collections.Generic");
            SystemUsings.Add("System.Data.Common");
            UserUsings.Add("orm.Db");
            UserUsings.Add($"{package.ParentPackage.RootNamespace}.Model.DOT");

            var dbModel = package.ParentPackage.ParentPackage.DBbSchemaModel;
            foreach (var dotDef in package.ParentPackage.ParentPackage.DomainModel.AllDOTDefinitions.Values)
                Storage.CreateStorageClass(this, dotDef, dbModel);
        }
    };
}
