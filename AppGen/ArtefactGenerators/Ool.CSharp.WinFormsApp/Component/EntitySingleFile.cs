using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Model;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Component;

public class EntitySingleFile : CSComponent
{
    public EntitySingleFile(ModelPackage package)
    {
        Package = package;
        Name = "DOTs.cs";
        Namespace = $"{package.ParentPackage.RootNamespace}.Model.DOT";

        SystemUsings.Add("System");
        SystemUsings.Add("System.Collections.Generic");
        SystemUsings.Add("System.Data");
        SystemUsings.Add("System.Data.Common");
        UserUsings.Add("orm.Db");
        UserUsings.Add($"{package.ParentPackage.RootNamespace}.Model.Storage");

        foreach (var dotDef in package.ParentPackage.ParentPackage.DomainModel.AllEntityDefinitions.Values)
        {
            EntityPackage.CreateEntityClass(this, dotDef);
        }
    }
}
