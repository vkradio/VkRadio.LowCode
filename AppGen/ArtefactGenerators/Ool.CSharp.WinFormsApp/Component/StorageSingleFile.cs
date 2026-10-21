using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Model;
using VkRadio.LowCode.AppGen.Domain.Names;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Component;

public class StorageSingleFile : CSComponent
{
    public StorageSingleFile(ModelPackage package, NaturalLanguageEnum preferNaturalLanguageForComments)
    {
        Package = package;
        Name = "Storages.cs";
        Namespace = $"{package.ParentPackage.RootNamespace}.Model.Storage";

        SystemUsings.Add("System");
        SystemUsings.Add("System.Collections.Generic");
        SystemUsings.Add("System.Data.Common");
        UserUsings.Add($"{package.ParentPackage.RootNamespace}.Model.DOT");
        UserUsings.Add("VkRadio.LowCode.Libs.Orm");

        var dbModel = package.ParentPackage.ParentPackage.DBbSchemaModel;

        foreach (var entityDef in package.ParentPackage.ParentPackage.DomainModel.AllEntityDefinitions.Values)
        {
            Storage.CreateStorageClass(this, entityDef, dbModel, preferNaturalLanguageForComments);
        }
    }
}
