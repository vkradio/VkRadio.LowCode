using System;

namespace VkRadio.LowCode.Libs.Orm
{
    public class ZeroRowsAffectedException: Exception
    {
        public ZeroRowsAffectedException() { }

        public ZeroRowsAffectedException(string message) : base(message) { }

        public ZeroRowsAffectedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
