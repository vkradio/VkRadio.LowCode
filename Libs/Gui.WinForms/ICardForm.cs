using System;

namespace VkRadio.LowCode.Libs.Gui.WinForms
{
    public interface ICardForm: IDisposable
    {
        bool Changed { get; }
    }
}
