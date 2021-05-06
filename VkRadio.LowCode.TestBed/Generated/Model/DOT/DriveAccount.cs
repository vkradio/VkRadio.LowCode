using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using VkRadio.LowCode.Gui.Utils;
using VkRadio.LowCode.Orm;
using VkRadio.LowCode.TestBed.Generated.Model.Storage;

namespace VkRadio.LowCode.TestBed.Generated.Model.DOT
{
    /// <summary>
    /// Drive Account
    /// </summary>
    public class DriveAccount: DbMappedDOT
    {
        /// <summary>
        /// Name
        /// </summary>
        protected string? _name;
        /// <summary>
        /// Code
        /// </summary>
        protected string? _code;
        /// <summary>
        /// Default Value
        /// </summary>
        protected int? _defaultValue;

        /// <summary>
        /// Creating a new object and initialize default values
        /// </summary>
        public override void InitNew()
        {
            _name = string.Empty;
            _code = string.Empty;
        }

        /// <summary>
        /// Creating an object clone
        /// </summary>
        public override object Clone()
        {
            var clone = new DriveAccount();
            CloneBase(this, clone);
            clone._name = _name;
            clone._code = _code;
            clone._defaultValue = _defaultValue;
            return clone;
        }

        /// <summary>
        /// Default string representation of the object
        /// </summary>
        public override string ToString() => overrider != null && overrider.OverrideToString ? overrider.ToStringOverride() : (_name ?? string.Empty);

        /// <summary>
        /// Checking object state to be valid before saving to the Database
        /// </summary>
        public override string? Validate()
        {
            var baseResult = base.Validate();
            if (baseResult != null)
                return baseResult;
        
            baseResult = ValidateInner();
            if (baseResult != null)
                return baseResult;
        
            if (string.IsNullOrWhiteSpace(_name))
                return string.Format(CultureInfo.CurrentCulture, c_propertyValueNotSet, "Name");
            if (_name.Length < 1 || _name.Length > 100)
                return string.Format(CultureInfo.CurrentCulture, c_invalidPropertyLength, "Name", 1, 100);
            if (string.IsNullOrWhiteSpace(_code))
                return string.Format(CultureInfo.CurrentCulture, c_propertyValueNotSet, "Code");
            if (_code.Length < 1 || _code.Length > 255)
                return string.Format(CultureInfo.CurrentCulture, c_invalidPropertyLength, "Code", 1, 255);
        
            return null;
        }

        /// <summary>
        /// Name
        /// </summary>
        public string? Name { get => _name; set { Modify(); _name = value; } }
        /// <summary>
        /// Code
        /// </summary>
        public string? Code { get => _code; set { Modify(); _code = value; } }
        /// <summary>
        /// Default Value
        /// </summary>
        public int? DefaultValue { get => _defaultValue; set { Modify(); _defaultValue = value; } }
    }
}
