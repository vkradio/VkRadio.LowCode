using System;
using VkRadio.LowCode.Libs.Orm;
using VkRadio.Patterns.Guards;

namespace VkRadio.LowCode.Libs.Gui.WinForms
{
    public class SelectedObjectEventArgs: EventArgs
    {
        readonly DbMappedDOT _selectedObject;

        public SelectedObjectEventArgs(DbMappedDOT selectedObject)
        {
            GuardRule.Forbid.Null(selectedObject, nameof(selectedObject));
            _selectedObject = selectedObject;
        }

        public DbMappedDOT SelectedObject => _selectedObject;
    }
}
