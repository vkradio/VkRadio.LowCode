﻿using System;
using System.Windows.Forms;
using VkRadio.LowCode.Orm;

namespace VkRadio.LowCode.Gui.WinForms
{
    public partial class DOEditPanel : UserControl
    {
        protected string c_noWayNoObjectSelected = "Объект не выбран.";
        protected string c_captionFault = "Отказ";

        protected DbMappedDOT _o;

        public DOEditPanel()
        {
            InitializeComponent();
        }

        public virtual void SyncFromDOTBase(DbMappedDOT in_o) { _o = in_o; SyncFromDOT(_o); }
        public virtual void SyncFromDOT(DbMappedDOT in_o) {}
        public virtual string SyncToDOT(DbMappedDOT in_o) { return null; }

        public virtual void AnyValueChanged(object sender, EventArgs e)
        {
            if (Parent != null)
            {
                ((DOCard)Parent.Parent).AnyValueChanged(sender, e);
            }
        }
    };
}