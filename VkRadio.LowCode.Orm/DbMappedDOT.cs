using Ardalis.GuardClauses;
using System;
using System.Globalization;

namespace VkRadio.LowCode.Orm
{
    public abstract class DbMappedDOT : ICloneable
    {
        protected static readonly DateTime C_MIN_SQL_DATE_TIME = new DateTime(1753, 1, 1, 0, 0, 0);
        protected static readonly DateTime C_MAX_SQL_DATE_TIME = new DateTime(9999, 12, 31, 23, 59, 59);

        protected const string PARAM_ID = "@in_id";

        protected const string c_propertyValueNotSet = "The value of the property \"{0}\" is not set.";
        protected const string c_invalidPropertyLength = "The property \"{0}\" should have a length between {1} and {2} symbols.";
        protected const string c_invalidEmail = "The property \"{0}\" has an invalid value.";
        protected readonly string c_invalidPropertyDateTime = string.Format(CultureInfo.CurrentCulture, "Свойство \"{0}\" должно иметь значение не менее {1} и не более {2}.", "{0}", C_MIN_SQL_DATE_TIME, C_MAX_SQL_DATE_TIME);

        protected IDbProviderFactory dbProviderFactory = default!;

        public void SetDbProviderFactory(IDbProviderFactory dbProviderFactory) => this.dbProviderFactory = dbProviderFactory;

        protected Guid? id;

        /// <summary>
        /// Whether an object in memory is in sync with the Database
        /// </summary>
        protected bool inSync;

        protected DOTOverrider? overrider;

        /// <summary>
        /// Primary key
        /// </summary>
        public virtual Guid? Id { get => id; set => id = value; }

        public virtual void Modify() => inSync = false;

        public virtual bool InSync { get => inSync; set => inSync = value; }

        public void SetStorageConsistency() => inSync = true;

        public virtual string? ValidateInner() => null;

        public virtual string? Validate() => null;

        /// <summary>
        /// Reset all reference properties to null, leaving their id&apos;s intact. It is needed
        /// in the case when one or all of the cached reference objects are outdated compared
        /// to their database state.
        /// </summary>
        public virtual void ResetCachedRefProperties() { }

        public virtual void InitNew() { }

        public virtual void CloneBase(DbMappedDOT cloneFrom, DbMappedDOT cloneTo)
        {
            Guard.Against.Null(cloneFrom, nameof(cloneFrom));
            Guard.Against.Null(cloneTo, nameof(cloneTo));

            cloneTo.id = cloneFrom.id;
            cloneTo.inSync = cloneFrom.inSync;
            cloneTo.overrider = cloneFrom.overrider;
        }

        public abstract object Clone();

        public DOTOverrider? Overrider { get => overrider; set => overrider = value; }
    }
}
