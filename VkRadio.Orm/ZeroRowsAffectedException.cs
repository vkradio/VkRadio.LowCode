using System;

namespace VkRadio.Orm
{
    public class ZeroRowsAffectedException: Exception
    {
        public ZeroRowsAffectedException() { }

        public ZeroRowsAffectedException(string message) : base(message) { }

        public ZeroRowsAffectedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
