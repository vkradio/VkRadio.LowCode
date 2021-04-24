using System;
using VkRadio.LowCode.Orm;

namespace VkRadio.LowCode.Gui.WinForms
{
    public class SelectedObjectEventArgs: EventArgs
    {
        DbMappedDOT _selectedObject;

        public SelectedObjectEventArgs(DbMappedDOT in_selectedObject) { _selectedObject = in_selectedObject; }

        public DbMappedDOT SelectedObject { get { return _selectedObject; } }
    };
}
