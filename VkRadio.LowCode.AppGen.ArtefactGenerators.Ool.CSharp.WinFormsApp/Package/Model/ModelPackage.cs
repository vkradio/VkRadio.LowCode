using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;
using PackNS = VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Package;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Component;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Model;

public class ModelPackage : PackNS.Package
{
    DOTPackage _dotPackage;
    StoragePackage _storagePackage;

    public ModelPackage(CSharpProjectBase parentPackage)
        : base(parentPackage, "Model")
    {
        //_storagePackage = new StoragePackage(this);
        //_subpackages.Add(_storagePackage.Name, _storagePackage);

        //_dotPackage = new DOTPackage(this);
        //_subpackages.Add(_dotPackage.Name, _dotPackage);

        DOTSingleFile = new EntitySingleFile(this);
        _components.Add(DOTSingleFile.Name, DOTSingleFile);

        StorageSingleFile = new StorageSingleFile(this);
        _components.Add(StorageSingleFile.Name, StorageSingleFile);

        StoragePackage.CreateStorageRegistryComponent(parentPackage.ParentPackage.DomainModel, this, StorageSingleFile.Namespace);
    }

    public new CSharpProjectBase ParentPackage { get { return (CSharpProjectBase)_parentPackage; } }

    //public DOTPackage DOTPackage { get { return _dotPackage; } }
    public EntitySingleFile DOTSingleFile { get; private set; }
    //public StoragePackage StoragePackage { get { return _storagePackage; } }
    public StorageSingleFile StorageSingleFile { get; private set; }
}
