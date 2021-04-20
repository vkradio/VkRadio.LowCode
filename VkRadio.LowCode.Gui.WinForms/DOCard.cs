using System.Windows.Forms;
using VkRadio.LowCode.Orm;

namespace VkRadio.LowCode.Gui.WinForms
{
    public partial class DOCard : UserControl, IFRM_Card
    {
        protected const string c_captionFailure = "Refusal";

        protected DbMappedDOT _o;
        protected IDOStorage _storage;
        protected bool _changed;
        protected string _dotName;
        protected bool _controlsInitialized;

        protected bool CommitRollbackButtonsEnabled { get { return B_Rollback.Enabled; } set { B_Commit.Enabled = value; B_Rollback.Enabled = value; } }

        protected virtual void SyncFromDOConcrete() {}
        protected virtual string SyncToDOConcrete() { return null; }
        protected virtual void InitializeComponentConcrete() {}

        public DOCard()
        {
            InitializeComponent();
        }
        public DOCard(IDOStorage in_storage, DbMappedDOT in_o, string in_dotName, DOEditPanel in_panel) : this()
        {
            _o = in_o;
            _storage = in_storage;
            _dotName = in_dotName;
            
            PAN_Content.Controls.Add(in_panel);
            in_panel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        }

        public virtual void SyncFromDO()
        {
            this.Text = _dotName + " - " + (_o.Id.HasValue ?
                "Edit" :
                "Create");
            if (Parent != null && Parent is FRM_DOCard)
                ((FRM_DOCard)Parent).Text = Text;

            if (_controlsInitialized)
                SyncFromDOConcrete();

            ((DOEditPanel)PAN_Content.Controls[0]).SyncFromDOTBase(_o);

            B_Commit.Enabled = !_o.Id.HasValue;
            B_Rollback.Enabled = !_o.Id.HasValue;
        }
        public virtual string SyncToDO()
        {
            return ((DOEditPanel)PAN_Content.Controls[0]).SyncToDOT(_o);
        }

        public virtual bool Commit()
        {
            string errMessage = SyncToDO();
            if (errMessage == null)
                errMessage = _o.Validate();
            if (errMessage != null)
            {
                MessageBox.Show(this, errMessage, c_captionFailure, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
                errMessage = _storage.Flush(_o);
                if (errMessage != null)
                {
                    MessageBox.Show(this, errMessage, c_captionFailure, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
                else
                {
                    _o = (DbMappedDOT)_o.Clone();
                    SyncFromDO();
                    _changed = true;
                    return true;
                }
            }
        }
        public virtual void Rollback()
        {
            _o = _o.Id.HasValue ?
                _storage.Restore(_o.Id.Value) :
                _storage.CreateNew();
            _o = (DbMappedDOT)_o.Clone();
            SyncFromDO();
        }

        /// <summary>
        /// Flag, indicating whether the editable object&apos;s state has been
        /// changed in the Database. Used in the calling form after closing this form.
        /// </summary>
        public bool Changed { get { return _changed; } }
        /// <summary>
        /// Flag, indicating that changes of object state have not been saved in the
        /// Database. Used to decide whether there is a need to ask a user about save
        /// on closing the form.
        /// </summary>
        public bool NotSaved { get { return !_o.InSync || CommitRollbackButtonsEnabled; } }
        public bool IsNewObject { get { return !_o.Id.HasValue; } }

        public virtual void AnyValueChanged(object sender, System.EventArgs e) { CommitRollbackButtonsEnabled = true; }
        void B_Commit_Click(object sender, System.EventArgs e) { Commit(); }
        void B_Rollback_Click(object sender, System.EventArgs e) { Rollback(); }

        public virtual void Init()
        {
            InitializeComponentConcrete();
            _controlsInitialized = true;
        }

        public string DOTName { get { return _dotName; } }
    }
}
