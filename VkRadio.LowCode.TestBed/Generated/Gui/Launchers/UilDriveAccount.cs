using VkRadio.LowCode.Gui.WinForms;
using VkRadio.LowCode.Orm;
using VkRadio.LowCode.TestBed.Generated.Gui.Elements;
using VkRadio.LowCode.TestBed.Generated.Gui.Lists;
using VkRadio.LowCode.TestBed.Generated.Model.Storage;

namespace VkRadio.LowCode.TestBed.Generated.Gui.Launchers
{
    /// <summary>
    /// UI Means Launcher for objects of type Drive Account
    /// </summary>
    public class UilDriveAccount: UILauncher
    {
        /// <summary>
        /// Lancher&apos; constructor
        /// </summary>
        public UilDriveAccount()
        {
            _storage = StorageRegistry.Instance.DriveAccountStorage;
            _dotName = "Drive Account";
        }

        /// <summary>
        /// Creating of the object&apos;s Card
        /// </summary>
        protected override DOCard CreateDOCard(DbMappedDOT in_o)
        {
            var panel = new DOPDriveAccount();
            var card = new DOCard(_storage, in_o, _dotName, panel);
            return card;
        }
        /// <summary>
        /// Creating of the object&apos;s List
        /// </summary>
        protected override DOList CreateDOList() => new DOLDriveAccount();
    };
}
