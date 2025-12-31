using System;

namespace MetaModel
{
    public class UniquinessException : Exception
    {
        public UniquinessException(Guid in_id, string in_message, Exception in_innerException = null)
            : base(in_message, in_innerException)
        {
            Id = in_id;
        }

        public Guid Id { get; private set; }
    };
}
