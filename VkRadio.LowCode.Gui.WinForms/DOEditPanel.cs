using Ardalis.GuardClauses;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using VkRadio.LowCode.Orm;

namespace VkRadio.LowCode.Gui.WinForms
{
    public partial class DOEditPanel : UserControl
    {
        protected string c_noWayNoObjectSelected = "Object not selected.";
        protected string c_captionFault = "Refusal";

        protected DbMappedDOT? dataObject;

        public DOEditPanel() => InitializeComponent();

        public virtual void SyncFromDOTBase(DbMappedDOT dataObject)
        {
            Guard.Against.Null(dataObject, nameof(dataObject));

            this.dataObject = dataObject;
            SyncFromDOT(this.dataObject);
        }

        public virtual void SyncFromDOT(DbMappedDOT dataObject) { }

        public virtual string? SyncToDOT(DbMappedDOT dataObject) => null;


        [SuppressMessage("Security", "CA2109:Review visible event handlers", Justification = "We have no dangerous security functionality here")]
        public virtual void AnyValueChanged(object? sender, EventArgs e) => ((DOCard?)Parent?.Parent)?.AnyValueChanged(sender, e);
    };
}
