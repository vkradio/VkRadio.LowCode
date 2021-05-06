using Ardalis.GuardClauses;
using System.Windows.Forms;

namespace VkRadio.LowCode.Gui.WinForms
{
    public partial class DOCardForm : Form, ICardForm
    {
        protected DOCard doCard = default!;

        public DOCardForm() => InitializeComponent();

        public virtual void InitFromDOCard(DOCard doCard)
        {
            Guard.Against.Null(doCard, nameof(doCard));

            this.doCard = doCard;
            Controls.Add(this.doCard);
            this.doCard.SyncFromDO();
            this.doCard.Dock = DockStyle.Fill;
        }

        public virtual bool Changed => doCard?.Changed ?? false;

        void FRM_DOCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (doCard.NotSaved)
                {
                    var question = doCard.IsNewObject ?
                        "Do you need to save the object?" :
                        "Do you need to save changes?";
                    var userReply = MessageBox.Show(this, question, "Сохранение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (userReply)
                    {
                        case DialogResult.Yes:
                            var saved = doCard.Commit();
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

        void FRM_DOCard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                Close();
        }
    }
}
