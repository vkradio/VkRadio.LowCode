using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular.Package.Model;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular.Component;

public class DOTSingleFile : CSComponent
{
    public DOTSingleFile(ModelPackage package)
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

        foreach (var dotDef in package.ParentPackage.ParentPackage.DomainModel.AllDOTDefinitions.Values)
        {
            DOTPackage.CreateDOTClass(this, dotDef);
        }
    }
}
