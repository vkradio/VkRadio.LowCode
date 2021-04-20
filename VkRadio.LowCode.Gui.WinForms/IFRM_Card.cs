using System;

namespace VkRadio.LowCode.Gui.WinForms
{
    public interface IFRM_Card: IDisposable
    {
        bool Changed { get; }
    };
}
