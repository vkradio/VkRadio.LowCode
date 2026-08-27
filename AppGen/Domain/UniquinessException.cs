namespace VkRadio.LowCode.AppGen.Domain;

public class UniquinessException : Exception
{
    public UniquinessException(Guid id, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        Id = id;
    }

    public Guid Id { get; private set; }
}
