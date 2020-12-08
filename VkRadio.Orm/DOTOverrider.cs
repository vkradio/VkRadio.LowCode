using System;

namespace VkRadio.Orm
{
    /// <summary>
    /// Plug-in to override DbMappedDOT behavior
    /// </summary>
    public abstract class DOTOverrider: ICloneable
    {
        protected DbMappedDOT dot = default!;
        protected bool overrideToString;

        public virtual string ToStringOverride() => throw new NotImplementedException("DOTOverrider::ToStringOverride");

        public DbMappedDOT DOT { get => dot; set => dot = value; }

        public bool OverrideToString => overrideToString;

        public abstract object Clone();
    }
}
