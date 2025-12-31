using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Component;
using PackNS = ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Package;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Model
{
    public class ModelPackage : PackNS.Package
    {
        DOTPackage _dotPackage;
        StoragePackage _storagePackage;

        public ModelPackage(CSharpProjectBase in_parentPackage)
            : base(in_parentPackage, "Model")
        {
            //_storagePackage = new StoragePackage(this);
            //_subpackages.Add(_storagePackage.Name, _storagePackage);

            //_dotPackage = new DOTPackage(this);
            //_subpackages.Add(_dotPackage.Name, _dotPackage);

            DOTSingleFile = new DOTSingleFile(this);
            _components.Add(DOTSingleFile.Name, DOTSingleFile);

            StorageSingleFile = new StorageSingleFile(this);
            _components.Add(StorageSingleFile.Name, StorageSingleFile);

            StoragePackage.CreateStorageRegistryComponent(in_parentPackage.ParentPackage.DomainModel, this, StorageSingleFile.Namespace);
        }

        public new CSharpProjectBase ParentPackage { get { return (CSharpProjectBase)_parentPackage; } }

        //public DOTPackage DOTPackage { get { return _dotPackage; } }
        public DOTSingleFile DOTSingleFile { get; private set; }
        //public StoragePackage StoragePackage { get { return _storagePackage; } }
        public StorageSingleFile StorageSingleFile { get; private set; }
    };
}
