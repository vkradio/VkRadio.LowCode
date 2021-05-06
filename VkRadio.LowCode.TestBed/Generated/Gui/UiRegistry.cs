using VkRadio.LowCode.Gui.WinForms;
using VkRadio.LowCode.TestBed.Generated.Gui.Launchers;

namespace VkRadio.LowCode.TestBed.Generated.Gui
{
    /// <summary>
    /// UI Launchers Registry
    /// </summary>
    public class UiRegistry
    {
        /// <summary>
        /// Sole instance (Singleton)
        /// </summary>
        static UiRegistry _instance = default!;
        /// <summary>
        /// Quick Select Storage
        /// </summary>
        readonly QuickSelectStorage _quickSelectStorage = new();
        /// <summary>
        /// UI Launcher for Drive Account objects
        /// </summary>
        UILauncher _uilDriveAccount;

        /// <summary>
        /// Private constructor of UI launchers
        /// </summary>
        UiRegistry()
        {
            _uilDriveAccount = new UilDriveAccount();
        }
        /// <summary>
        /// Static constructor of UI launchers
        /// </summary>
        static UiRegistry() => ResetInstance();

        public static void ResetInstance() => _instance = new UiRegistry();

        /// <summary>
        /// Sole instance (Singleton)
        /// </summary>
        public static UiRegistry Instance => _instance;
        /// <summary>
        /// Quick Select Storage
        /// </summary>
        public QuickSelectStorage QuickSelectStorage => _quickSelectStorage;
        /// <summary>
        /// UI Launcher for Drive Account objects
        /// </summary>
        public UILauncher UilDriveAccount { get => _uilDriveAccount; set => _uilDriveAccount = value; }
    }
}
