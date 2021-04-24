using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using VkRadio.LowCode.Gui.WinForms;
using VkRadio.LowCode.TestBed.Generated.Gui;
using VkRadio.LowCode.TestBed.Generated.Model.Storage;

namespace VkRadio.LowCode.TestBed
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        void MainForm_Load(object sender, EventArgs e)
        {
            // I just had no time to discover how to properly config WinForms apps in .NET 5,
            // so I use a quick and dirty trick here. This is a plain text file with three rows:
            // app name (any), server address (ex: "localhost"), database name (ex: "sometestdb").
            var dbParams = File.ReadAllLines(@"C:\Users\vitaliy\Desktop\testbed.txt");

            var builder = new SqlConnectionStringBuilder
            {
                ApplicationName = dbParams[0],
                DataSource = dbParams[1],
                InitialCatalog = dbParams[2],
                IntegratedSecurity = true
            };

            // Legacy way of using lots of Singletons everywhere. Now I'll almost never do that, I'll prefer
            // DI instead (such as partially implemented in SqlClientFactory constructor here).
            Orm.DbProviderFactory.Default = new Orm.SqlClientFactory(builder.ConnectionString);
            StorageRegistry.Instance = new StorageRegistry(Orm.DbProviderFactory.Default);
            if (UiRegistry.Instance == null)
                throw new ApplicationException("UiRegistryExt.Instance == null");
            QuickSelectStorage.ProgramFolder = dbParams[0];
        }

        void ObjectListButton_Click(object sender, EventArgs e)
        {
            using var frm = UiRegistry.Instance.UilDriveAccount.CreateList();
            frm.ShowDialog(this);
        }
    }
}
