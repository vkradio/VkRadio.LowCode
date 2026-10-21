using System;

namespace VkRadio.LowCode.Libs.Orm
{
    /// <summary>
    /// Plug-in to override DbMappedDOT behavior
    /// </summary>
    public abstract class DOTOverrider: ICloneable
    {
        protected DbMappedDOT _dot = default!;
        protected bool _overrideToString;

        public virtual string ToStringOverride() => throw new NotImplementedException("DOTOverrider::ToStringOverride");

        public DbMappedDOT DOT { get => _dot; set => _dot = value; }

        public bool OverrideToString => _overrideToString;

        public abstract object Clone();
    }
}
