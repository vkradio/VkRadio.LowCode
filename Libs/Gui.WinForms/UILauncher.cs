using System.Data.Common;
using System.Windows.Forms;
using VkRadio.LowCode.Libs.Orm;

namespace VkRadio.LowCode.Libs.Gui.WinForms
{
    public abstract class UILauncher
    {
        protected IDOStorage _storage = default!;
        protected string _dotName = default!;

        protected abstract DOCard CreateDOCard(DbMappedDOT dataObject);
        protected abstract DOList CreateDOList();

        public virtual ICardForm CreateCard(DbMappedDOT dataObject)
        {
            var frm = new DOCardForm();
            var doCard = CreateDOCard(dataObject);
            doCard.Init();
            frm.InitFromDOCard(doCard);
            return frm;
        }
        public virtual Form CreateList() => CreateList(false, null);
        public virtual Form CreateList(bool selectMode, FilterAbstract? filter, int? defaultSortFieldIndex = null, bool? defaultSortAscending = null, DbParameter[]? dbParameters = null)
        {
            var frm = new DOListForm(_dotName, _storage, selectMode);
            var dol = CreateDOList();
            dol.Init(_storage, this, filter, defaultSortFieldIndex, defaultSortAscending, dbParameters);
            frm.InitByDOList(dol);
            return frm;
        }
        public virtual object? CreateFilterForm() => null;

        public string DotName => _dotName;
    }
}
