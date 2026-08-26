namespace VkRadio.LowCode.AppGenerator;

public class GeneratorException : ApplicationException
{
    public GeneratorException() : base() { }
    public GeneratorException(string message) : base(message) { }
    public GeneratorException(string message, Exception innerException) : base(message, innerException) { }
}
