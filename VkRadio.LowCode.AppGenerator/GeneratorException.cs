using System;

namespace VkRadio.LowCode.AppGenerator;

public class GeneratorException : ApplicationException
{
    public GeneratorException() : base() { }
    public GeneratorException(string in_message) : base(in_message) { }
    public GeneratorException(string in_message, Exception in_innerException) : base(in_message, in_innerException) { }
};
