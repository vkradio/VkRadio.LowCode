using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

using VkRadio.LowCode.Gui.WinForms;
using VkRadio.LowCode.Orm;
using VkRadio.LowCode.TestBed.Generated.Model.DOT;
using VkRadio.LowCode.TestBed.Generated.Model.Storage;

namespace VkRadio.LowCode.TestBed.Generated.Gui.Elements
{
    /// <summary>
    /// Editable Card for object Drive Account
    /// </summary>
    public partial class DOPDriveAccount: DOEditPanel
    {
        /// <summary>
        /// Constructor of the View/Edit Card
        /// </summary>
        public DOPDriveAccount()
        {
            InitializeComponent();
        
            SF_Name.ValueChanged += (s, e) => AnyValueChanged(s, e);
            SF_Code.ValueChanged += (s, e) => AnyValueChanged(s, e);
            SF_DefaultValue.ValueChanged += (s, e) => AnyValueChanged(s, e);
        }

        /// <summary>
        /// Synchronize the widget state from the object state
        /// </summary>
        public override void SyncFromDOT(DbMappedDOT dataObject)
        {
            Guard.Against.Null(dataObject, nameof(dataObject));

            DriveAccount o = (DriveAccount)dataObject;
        
            SF_Name.SetValue(o.Name);
            SF_Code.SetValue(o.Code);
            SF_DefaultValue.SetValue(o.DefaultValue);
        }
        /// <summary>
        /// Synchronize the object state from the widget state
        /// </summary>
        public override string? SyncToDOT(DbMappedDOT dataObject)
        {
            Guard.Against.Null(dataObject, nameof(dataObject));

            DriveAccount o = (DriveAccount)dataObject;
        
            o.Name = SF_Name.GetValueAsString();
            o.Code = SF_Code.GetValueAsString();
            o.DefaultValue = SF_DefaultValue.GetValueAsIntNullable();
        
            return null;
        }
    }
}
