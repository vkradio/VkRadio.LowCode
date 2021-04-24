using VkRadio.LowCode.Orm;

namespace VkRadio.LowCode.TestBed.Generated.Model.Storage
{
    /// <summary>
    /// Реестр хранилищ ОД
    /// </summary>
    public class StorageRegistry
    {
        /// <summary>
        /// Storage of Drive Account
        /// </summary>
        DriveAccountStorage _driveAccountStorage;
        /// <summary>
        /// Sole instance (Singleton)
        /// </summary>
        static StorageRegistry _instance;

        /// <summary>
        /// Storage Registry Constructor
        /// </summary>
        public StorageRegistry(IDbProviderFactory dbProviderFactory)
        {
            _driveAccountStorage = new DriveAccountStorage(dbProviderFactory); _driveAccountStorage.Init();
        }
        /// <summary>
        /// Static Constructor of the Storage Registry
        /// </summary>
        //static StorageRegistry() { _instance = new StorageRegistry(); }

        public static void CreateStaticStorageRegistry(IDbProviderFactory dbProviderFactory) => _instance = new StorageRegistry(dbProviderFactory);

        /// <summary>
        /// Storage of Drive Account
        /// </summary>
        public DriveAccountStorage DriveAccountStorage { get { return _driveAccountStorage; } set { _driveAccountStorage = value; } }
        public static StorageRegistry Instance { get => _instance; set => _instance = value; }
    };
}
