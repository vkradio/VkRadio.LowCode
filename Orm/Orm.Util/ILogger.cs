namespace VkRadio.Orm.Util;

public interface ILogger
{
    void WriteException(Exception exception);

    void WriteMessage(string message);
}
