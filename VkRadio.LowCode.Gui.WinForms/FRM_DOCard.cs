using System.Windows.Forms;

namespace VkRadio.LowCode.Gui.WinForms
{
    public partial class FRM_DOCard : Form, IFRM_Card
    {
        protected DOCard _doCard;

        public FRM_DOCard()
        {
            InitializeComponent();
        }

        public virtual void InitFromDOCard(DOCard in_doCard)
        {
            _doCard = in_doCard;
            Controls.Add(_doCard);
            _doCard.SyncFromDO();
            _doCard.Dock = DockStyle.Fill;
        }

        public virtual bool Changed { get { return _doCard == null ? false : _doCard.Changed; } }

        private void FRM_DOCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (_doCard.NotSaved)
                {
                    var question = _doCard.IsNewObject ?
                        "Do you need to save the object?" :
                        "Do you need to save changes?";
                    var userReply = MessageBox.Show(this, question, "Сохранение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (userReply)
                    {
                        case DialogResult.Yes:
                            bool saved = _doCard.Commit();
                            // If the user answered "Cancel", canceling form closing.
                            if (!saved)
                                e.Cancel = true;
                            break;
                        case DialogResult.No:
                            // Doing nothing, just allow the form to close.
                            break;
                        default:
                            e.Cancel = true;
                            break;
                    }
                }
            }
        }

        private void FRM_DOCard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
