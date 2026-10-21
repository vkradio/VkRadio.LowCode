using Ardalis.GuardClauses;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using VkRadio.LowCode.Libs.Orm;

namespace VkRadio.LowCode.Libs.Gui.WinForms
{
    public partial class DOCard : UserControl, ICardForm
    {
        protected const string c_captionFailure = "Refusal";

        protected DbMappedDOT _dataObject = default!;
        protected IDOStorage _storage = default!;
        protected bool _dataObjectWasChanged;
        protected string _dataObjectTypeName = default!;
        protected bool _controlsInitialized;

        protected bool CommitRollbackButtonsEnabled { get { return B_Rollback.Enabled; } set { B_Commit.Enabled = value; B_Rollback.Enabled = value; } }

        protected virtual void SyncFromDOConcrete() {}
        protected virtual string? SyncToDOConcrete() => null;
        protected virtual void InitializeComponentConcrete() {}

        public DOCard() => InitializeComponent();

        public DOCard(IDOStorage storage, DbMappedDOT dataObject, string dataObjectTypeName, DOEditPanel editPanel) : this()
        {
            Guard.Against.Null(storage, nameof(storage));
            Guard.Against.Null(dataObject, nameof(dataObject));
            Guard.Against.Null(dataObjectTypeName, nameof(dataObjectTypeName));
            Guard.Against.Null(editPanel, nameof(editPanel));

            _dataObject = dataObject;
            _storage = storage;
            _dataObjectTypeName = dataObjectTypeName;
            
            PAN_Content.Controls.Add(editPanel);
            editPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        }

        public virtual void SyncFromDO()
        {
            Text = _dataObjectTypeName + " - " + (_dataObject.Id.HasValue ?
                "Edit" :
                "Create");
            if (Parent != null && Parent is DOCardForm card)
                card.Text = Text;

            if (_controlsInitialized)
                SyncFromDOConcrete();

            ((DOEditPanel)PAN_Content.Controls[0]).SyncFromDOTBase(_dataObject);

            B_Commit.Enabled = !_dataObject.Id.HasValue;
            B_Rollback.Enabled = !_dataObject.Id.HasValue;
        }

        public virtual string? SyncToDO() =>
            ((DOEditPanel)PAN_Content.Controls[0]).SyncToDOT(_dataObject);

        public virtual bool Commit()
        {
            var errMessage = SyncToDO();
            if (errMessage == null)
                errMessage = _dataObject.Validate();
            if (errMessage != null)
            {
                MessageBox.Show(this, errMessage, c_captionFailure, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
                errMessage = _storage.Flush(_dataObject);
                if (errMessage != null)
                {
                    MessageBox.Show(this, errMessage, c_captionFailure, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                else
                {
                    _dataObject = (DbMappedDOT)_dataObject.Clone();
                    SyncFromDO();
                    _dataObjectWasChanged = true;
                    return true;
                }
            }
        }

        public virtual void Rollback()
        {
            DbMappedDOT? tmpObj = null;

            if (_dataObject.Id.HasValue)
            {
                tmpObj = _storage.Restore(_dataObject.Id.Value);
                if (tmpObj == null)
                    MessageBox.Show(this, "The object was deleted in another window or application instance.", c_captionFailure, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            if (tmpObj == null)
                tmpObj = _storage.CreateNew(); // Here we process both situation: 1) rolling back newly created object to blanc card; 2) clearing up deleted object (see above).

            _dataObject = (DbMappedDOT)tmpObj!.Clone();
            SyncFromDO();
        }

        /// <summary>
        /// Flag, indicating whether the editable object&apos;s state has been
        /// changed in the Database. Used in the calling form after closing this form.
        /// </summary>
        public bool Changed => _dataObjectWasChanged;
        /// <summary>
        /// Flag, indicating that changes of object state have not been saved in the
        /// Database. Used to decide whether there is a need to ask a user about save
        /// on closing the form.
        /// </summary>
        public bool NotSaved => !_dataObject.InSync || CommitRollbackButtonsEnabled;
        public bool IsNewObject => !_dataObject.Id.HasValue;

        [SuppressMessage("Security", "CA2109:Review visible event handlers", Justification = "We have no dangerous security functionality here")]
        public virtual void AnyValueChanged(object? sender, System.EventArgs e) => CommitRollbackButtonsEnabled = true;

        void B_Commit_Click(object sender, System.EventArgs e) => Commit();
        void B_Rollback_Click(object sender, System.EventArgs e) => Rollback();

        public virtual void Init()
        {
            InitializeComponentConcrete();
            _controlsInitialized = true;
        }

        public string DOTName => _dataObjectTypeName;
    }
}
