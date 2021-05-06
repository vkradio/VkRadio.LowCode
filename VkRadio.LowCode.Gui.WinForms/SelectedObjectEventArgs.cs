using Ardalis.GuardClauses;
using System;
using VkRadio.LowCode.Orm;

namespace VkRadio.LowCode.Gui.WinForms
{
    public class SelectedObjectEventArgs: EventArgs
    {
        readonly DbMappedDOT selectedObject;

        public SelectedObjectEventArgs(DbMappedDOT selectedObject)
        {
            Guard.Against.Null(selectedObject, nameof(selectedObject));
            this.selectedObject = selectedObject;
        }

        public DbMappedDOT SelectedObject => selectedObject;
    }
}
