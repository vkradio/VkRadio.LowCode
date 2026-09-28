using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Component;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Root;
using VkRadio.LowCode.AppGen.Domain.Names;
using PackNS = VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Package;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Model;

public class ModelPackage : PackNS.Package
{
    public ModelPackage(CSharpProjectBase parentPackage, NaturalLanguageEnum preferNaturalLanguageForComments)
        : base(parentPackage, "Model")
    {
        var generator = (ArtefactGeneratorCSharpClassic)parentPackage.ParentPackage.ArtefactGenerationTarget.ArtefactGenerator;

        if (generator.PutSimilarArtefactsToSingleFile)
        {
            EntitySingleFile = new EntitySingleFile(this, preferNaturalLanguageForComments);
            _components.Add(EntitySingleFile.Name, EntitySingleFile);

            StorageSingleFile = new StorageSingleFile(this, preferNaturalLanguageForComments);
            _components.Add(StorageSingleFile.Name, StorageSingleFile);

            StoragePackage.CreateStorageRegistryComponent(parentPackage.ParentPackage.DomainModel, this, StorageSingleFile.Namespace, preferNaturalLanguageForComments);
        }
        else
        {
            EntityPackage = new EntityPackage(this, preferNaturalLanguageForComments);
            _subpackages.Add(EntityPackage.Name, EntityPackage);

            StoragePackage = new StoragePackage(this, preferNaturalLanguageForComments);
            _subpackages.Add(StoragePackage.Name, StoragePackage);
        }
    }

    public new CSharpProjectBase ParentPackage => (CSharpProjectBase)_parentPackage;

    public EntityPackage? EntityPackage { get; private set; }

    public EntitySingleFile? EntitySingleFile { get; private set; }

    public StoragePackage? StoragePackage { get; private set; }

    public StorageSingleFile? StorageSingleFile { get; private set; }
}
