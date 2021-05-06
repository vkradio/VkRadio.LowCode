using System.Diagnostics.CodeAnalysis;
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
            storage = StorageRegistry.Instance.DriveAccountStorage;
            dotName = "Drive Account";
        }

        /// <summary>
        /// Creating of the object&apos;s Card
        /// </summary>
        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "DOPDriveAccount is attached as a child in the base control and so will be automatically disposed")]
        protected override DOCard CreateDOCard(DbMappedDOT dataObject)
        {
            var panel = new DOPDriveAccount();
            var card = new DOCard(storage, dataObject, dotName, panel);
            return card;
        }

        /// <summary>
        /// Creating of the object&apos;s List
        /// </summary>
        protected override DOList CreateDOList() => new DOLDriveAccount();
    }
}
