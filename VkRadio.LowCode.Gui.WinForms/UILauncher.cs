using System.Data.Common;
using System.Windows.Forms;
using VkRadio.LowCode.Orm;

namespace VkRadio.LowCode.Gui.WinForms
{
    public abstract class UILauncher
    {
        protected IDOStorage _storage;
        protected string _dotName;

        protected abstract DOCard CreateDOCard(DbMappedDOT in_o);
        protected abstract DOList CreateDOList();

        public virtual IFRM_Card CreateCard(DbMappedDOT in_o)
        {
            FRM_DOCard frm = new FRM_DOCard();
            DOCard doCard = CreateDOCard(in_o);
            doCard.Init();
            frm.InitFromDOCard(doCard);
            return frm;
        }
        public virtual Form CreateList() { return CreateList(false, null); }
        public virtual Form CreateList(bool in_selectMode, FilterAbstract in_filter, int? in_defaultSortFieldIndex = null, bool? in_defaultSortAscending = null, DbParameter[] in_params = null)
        {
            FRM_DOList frm = new FRM_DOList(_dotName, _storage, in_selectMode);
            DOList dol = CreateDOList();
            dol.Init(_storage, this, in_filter, in_defaultSortFieldIndex, in_defaultSortAscending, in_params);
            frm.InitByDOList(dol);
            return frm;
        }
        public virtual object CreateFilterForm() { return null; }

        public string DotName { get { return _dotName; } }
    };
}
