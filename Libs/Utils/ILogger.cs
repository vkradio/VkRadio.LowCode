namespace VkRadio.LowCode.Libs.Utils;

public interface ILogger
{
    void WriteException(Exception exception);

    void WriteMessage(string message);
}
