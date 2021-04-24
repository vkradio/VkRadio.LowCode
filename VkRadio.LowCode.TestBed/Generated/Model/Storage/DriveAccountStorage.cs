using System;
using System.Collections.Generic;
using System.Data.Common;

using VkRadio.LowCode.Gui.Utils;
using VkRadio.LowCode.Orm;
using VkRadio.LowCode.TestBed.Generated.Model.DOT;

namespace VkRadio.LowCode.TestBed.Generated.Model.Storage
{
    /// <summary>
    /// Storage of objects Drive Account
    /// </summary>
    public class DriveAccountStorage: DOStorage<DriveAccountStorage, DriveAccount>
    {
        /// <summary>
        /// Storage Constructor
        /// </summary>
        public DriveAccountStorage(IDbProviderFactory dbProviderFactory) : base(dbProviderFactory)
        {
            tableName = "drive_account";
            tableNameQ = "\"drive_account\"";
            decimalFields.Add(0);
            decimalFields.Add(0);
            decimalFields.Add(0);
            decimalFields.Add(0);
            fields = new string[]
            {
                c_fieldId,
                NAME,
                CODE,
                DEFAULT_VALUE,
            };
            fieldsHuman = new string[]
            {
                "id",
                "name",
                "code",
                "default value",
            };
            InitParams();
            defaultOrderBy = "\"name\"";
        }

        /// <summary>
        /// Filling the Data Object properties from DbDataReader
        /// </summary>
        public override void FillDOFromReader(DbDataReader in_reader, DriveAccount in_o)
        {
            var factory = dbProviderFactory;
            in_o.Name = factory.ReadStringFromReader(in_reader, 1, false);
            in_o.Code = factory.ReadStringFromReader(in_reader, 2, false);
            in_o.DefaultValue = factory.ReadIntNullableFromReader(in_reader, 3);
        }
        /// <summary>
        /// Filling parameters to write the Data Object state to the Database
        /// </summary>
        protected override void FillParameters(DbParameterCollection in_params, DriveAccount in_o)
        {
            var factory = dbProviderFactory;
            in_params.Add(factory.CreateParameter(parameters[1], in_o.Name, typeof(string), false));
            in_params.Add(factory.CreateParameter(parameters[2], in_o.Code, typeof(string), false));
            in_params.Add(factory.CreateParameter(parameters[3], in_o.DefaultValue, typeof(int?), true));
        }
        /// <summary>
        /// Reading the object by its Name property
        /// </summary>
        public virtual DriveAccount ReadByName(string in_name, DbTransaction in_transaction = null)
        {
            if (in_name == null)
                throw new ArgumentException("name");
            DbParameter[] dbParams = new DbParameter[] { dbProviderFactory.CreateParameter("@in_name", in_name, typeof(string), false) };
            List<DriveAccount> result = ReadAsCollection(
                where: NAME_Q + " = @in_name",
                parameters: dbParams,
                transaction: in_transaction
            );
            return result.Count != 0 ? result[0] : null;
        }

        /// <summary>
        /// Table field Name (variant without quotes)
        /// </summary>
        public const string NAME = "name";
        /// <summary>
        /// Table field Name (variant with quotes)
        /// </summary>
        public const string NAME_Q = "\"name\"";
        /// <summary>
        /// Table field Code (variant without quotes)
        /// </summary>
        public const string CODE = "code";
        /// <summary>
        /// Table field Code (variant with quotes)
        /// </summary>
        public const string CODE_Q = "\"code\"";
        /// <summary>
        /// Table field Default Value (variant without quotes)
        /// </summary>
        public const string DEFAULT_VALUE = "default_value";
        /// <summary>
        /// Table field Default Value (variant with quotes)
        /// </summary>
        public const string DEFAULT_VALUE_Q = "\"default_value\"";
    };
}
