using Ardalis.GuardClauses;
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
        public override void FillDOFromReader(DbDataReader reader, DriveAccount dataObject)
        {
            Guard.Against.Null(dataObject, nameof(dataObject));

            var factory = dbProviderFactory;
            dataObject.Name = factory.ReadStringFromReader(reader, 1, false);
            dataObject.Code = factory.ReadStringFromReader(reader, 2, false);
            dataObject.DefaultValue = factory.ReadIntNullableFromReader(reader, 3);
        }

        /// <summary>
        /// Filling parameters to write the Data Object state to the Database
        /// </summary>
        protected override void FillParameters(DbParameterCollection parameters, DriveAccount dataObject)
        {
            Guard.Against.Null(parameters, nameof(parameters));
            Guard.Against.Null(dataObject, nameof(dataObject));

            var factory = dbProviderFactory;
            parameters.Add(factory.CreateParameter(base.parameters[1], dataObject.Name, typeof(string), false));
            parameters.Add(factory.CreateParameter(base.parameters[2], dataObject.Code, typeof(string), false));
            parameters.Add(factory.CreateParameter(base.parameters[3], dataObject.DefaultValue, typeof(int?), true));
        }

        /// <summary>
        /// Reading the object by its Name property
        /// </summary>
        public virtual DriveAccount? ReadByName(string name, DbTransaction? transaction = null)
        {
            Guard.Against.Null(name, nameof(name));

            var dbParams = new DbParameter[] { dbProviderFactory.CreateParameter("@in_name", name, typeof(string), false) };
            var result = ReadAsCollection(
                where: NAME_Q + " = @in_name",
                parameters: dbParams,
                transaction: transaction
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
    }
}
