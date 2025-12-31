using Microsoft.Data.SqlClient;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using VkRadio.LowCode.Gui.WinForms;
using VkRadio.LowCode.TestBed.Generated.Gui;
using VkRadio.LowCode.TestBed.Generated.Model.Storage;
using VkRadio.LowCode.TestBed.Properties;

namespace VkRadio.LowCode.TestBed
{
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "This app will not be launched on Windows version prior to 7")]
    public partial class MainForm : Form
    {
        const string appName = "VkRadio.LowCode.TestBed";
        bool loadSettings;

        public MainForm()
        {
            InitializeComponent();

            SqlServerTextBox.TextChanged += SettingsChanged;
            DatabaseTextBox.TextChanged += SettingsChanged;
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            loadSettings = true;
            SqlServerTextBox.Text = Settings.Default.SqlServer;
            DatabaseTextBox.Text = Settings.Default.Database;
            loadSettings = false;

            QuickSelectStorage.ProgramFolder = appName;
        }

        void ObjectListButton_Click(object sender, EventArgs e)
        {
            var builder = new SqlConnectionStringBuilder
            {
                ApplicationName = appName,
                DataSource = Settings.Default.SqlServer,
                InitialCatalog = Settings.Default.Database,
                IntegratedSecurity = true,
                TrustServerCertificate = true
            };

            try
            {
                // Legacy way of using lots of Singletons everywhere. Now I'll almost never do that, I'll prefer
                // DI instead (such as partially implemented in SqlClientFactory constructor here).
                Orm.DbProviderFactory.Default = new Orm.SqlClientFactory(builder.ConnectionString);
                StorageRegistry.Instance = new StorageRegistry(Orm.DbProviderFactory.Default);
                UiRegistry.ResetInstance();

                using var frm = UiRegistry.Instance.UilDriveAccount.CreateList();
                frm.ShowDialog(this);
            }
            catch (SqlException)
            {
                MessageBox.Show(this, "SQL Server or Database not properly set.");
            }
        }

        void SettingsChanged(object? sender, EventArgs e)
        {
            if (!loadSettings)
            {
                Settings.Default.SqlServer = SqlServerTextBox.Text.Trim();
                Settings.Default.Database = DatabaseTextBox.Text.Trim();
                Settings.Default.Save();
            }
        }
    }
}
