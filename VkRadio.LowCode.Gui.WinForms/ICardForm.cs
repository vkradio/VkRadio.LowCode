using System;

namespace VkRadio.LowCode.Gui.WinForms
{
    public interface ICardForm: IDisposable
    {
        bool Changed { get; }
    }
}
