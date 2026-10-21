using Ardalis.GuardClauses;
using System;
using VkRadio.LowCode.Libs.Orm;

namespace VkRadio.LowCode.Libs.Gui.WinForms
{
    public class SelectedObjectEventArgs: EventArgs
    {
        readonly DbMappedDOT _selectedObject;

        public SelectedObjectEventArgs(DbMappedDOT selectedObject)
        {
            Guard.Against.Null(selectedObject, nameof(selectedObject));
            _selectedObject = selectedObject;
        }

        public DbMappedDOT SelectedObject => _selectedObject;
    }
}
