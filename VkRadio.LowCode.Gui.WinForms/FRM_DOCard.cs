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
                    string question = "Следует ли сохранить " + (_doCard.IsNewObject ? "объект?" : "изменения объекта?");
                    System.Windows.Forms.DialogResult userReply = MessageBox.Show(this, question, "Сохранение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                    switch (userReply)
                    {
                        case System.Windows.Forms.DialogResult.Yes:
                            bool saved = _doCard.Commit();
                            // Если при сохранении состояния объекта получен отказ, отменяем
                            // закрытие формы, чтобы позволить пользователю разобраться с этим.
                            if (!saved)
                                e.Cancel = true;
                            break;
                        case System.Windows.Forms.DialogResult.No:
                            // Ничего не делаем, просто позволяем форме закрыться.
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
