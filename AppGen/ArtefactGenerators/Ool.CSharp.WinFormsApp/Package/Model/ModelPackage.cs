using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Component;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Root;
using PackNS = VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Package;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Model;

public class ModelPackage : PackNS.Package
{
    EntityPackage _entityPackage;
    StoragePackage _storagePackage;

    public ModelPackage(CSharpProjectBase parentPackage)
        : base(parentPackage, "Model")
    {
        //_storagePackage = new StoragePackage(this);
        //_subpackages.Add(_storagePackage.Name, _storagePackage);

        //_dotPackage = new DOTPackage(this);
        //_subpackages.Add(_dotPackage.Name, _dotPackage);

        EntitySingleFile = new EntitySingleFile(this);
        _components.Add(EntitySingleFile.Name, EntitySingleFile);

        StorageSingleFile = new StorageSingleFile(this);
        _components.Add(StorageSingleFile.Name, StorageSingleFile);

        StoragePackage.CreateStorageRegistryComponent(parentPackage.ParentPackage.DomainModel, this, StorageSingleFile.Namespace);
    }

    public new CSharpProjectBase ParentPackage => (CSharpProjectBase)_parentPackage;

    //public EntityPackage EntityPackage => _entityPackage;

    public EntitySingleFile EntitySingleFile { get; private set; }

    //public StoragePackage StoragePackage => _storagePackage;

    public StorageSingleFile StorageSingleFile { get; private set; }
}
