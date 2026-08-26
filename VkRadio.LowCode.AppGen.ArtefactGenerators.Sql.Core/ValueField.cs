using System.Globalization;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core;

/// <summary>
/// Table field that stores an explicit value (not a foreign key or something alike)
/// </summary>
public abstract class ValueField : ITableField
{
    protected string _name;
    protected bool _nullable;
    protected string _sqlType;
    protected Table _table;
    protected PropertyCorrespondence _entityPropertyCorrespondence;
    protected string _defaultValue;
    protected bool _unique;
    protected string[] _sqlTypesWDeniedDefaults;
    protected string _boolSqlType;
    protected string _quoteSymbol;
    protected bool _generateConstraintsInline = true;

    ValueField() => throw new NotImplementedException();

    protected abstract void SetupStringField(PropertyDefinition propertyDefinition);

    protected virtual void SetupStringFieldDefault(bool defaultDeniedBySql, PropertyDefinition propertyDefinition)
    {
        if (!defaultDeniedBySql && propertyDefinition.DefaultValue is not null)
        {
            // TODO: Use the simplest SQL Escape. Indeally we need to implement a fully secure escape mechanism according to all rules
            _defaultValue = EntityPropertyCorrespondence!.TableAndEntityCorrespondence.DBSchemaDomainModel.GetValueStringForString((string)propertyDefinition.DefaultValue);
            //_defaultValue = "N'" + ((string)in_propertyDefinition.DefaultValue).Replace("'", "''") + "'";
        }
    }
    protected abstract void SetupUniqueCodeField(PropertyDefinition propertyDefinition);

    protected virtual void SetupDateTimeField(PropertyDefinition propertyDefinition) { _sqlType = "datetime"; }

    /// <summary>
    /// Field name (without qout symbols)
    /// </summary>
    public string Name => _name;

    /// <summary>
    /// Are NULL values allowed
    /// </summary>
    public bool Nullable => _nullable;

    /// <summary>
    /// SQL type (string literal)
    /// </summary>
    public string SqlType => _sqlType;

    /// <summary>
    /// Table that owns this field
    /// </summary>
    public Table Table => _table;

    /// <summary>
    /// Data object type property definition, that corresponds to this field
    /// </summary>
    public PropertyCorrespondence? EntityPropertyCorrespondence => _entityPropertyCorrespondence;

    /// <summary>
    /// Default value (string literal, or null, if there is no default value)
    /// </summary>
    public string? DefaultValue => _defaultValue;

    /// <summary>
    /// Is value unique throughout a table
    /// </summary>
    public bool Unique => _unique;

    /// <summary>
    /// Table field constructor with an explicit value, based on a property definition
    /// </summary>
    /// <param name="tableAndEntityCorrespondence"></param>
    /// <param name="propertyDefinition"></param>
    public ValueField(TableAndEntityCorrespondence tableAndEntityCorrespondence, PropertyDefinition propertyDefinition)
    {
        _table = tableAndEntityCorrespondence.Table;
        _name = NameHelper.NameToUnderscoreSeparatedName(propertyDefinition.Names[NaturalLanguageEnum.En]);
        _nullable = propertyDefinition.FunctionalType.Nullable;
        _unique = propertyDefinition.FunctionalType.Unique;
        _entityPropertyCorrespondence = new PropertyCorrespondence
        {
            PropertyDefinition = propertyDefinition,
            TableField = this,
            TableAndEntityCorrespondence = tableAndEntityCorrespondence
        };
    }

    public void Init()
    {
        var propDef = _entityPropertyCorrespondence.PropertyDefinition;

        // Set the SQL type and default value.
        // TODO: Make SQL types as close to SQL standard as possible
        if (propDef.FunctionalType is PFTBoolean)
        {
            _sqlType = _boolSqlType;

            if (_entityPropertyCorrespondence.PropertyDefinition.DefaultValue is not null)
            {
                _defaultValue = (bool)_entityPropertyCorrespondence.PropertyDefinition.DefaultValue
                    ? "1"
                    : "0";
            }
        }
        else if (propDef.FunctionalType is PFTDateTime)
        {
            SetupDateTimeField(propDef);
        }
        else if (propDef.FunctionalType is PFTDecimal)
        {
            _sqlType = "decimal(10, 2)";

            if (_entityPropertyCorrespondence.PropertyDefinition.DefaultValue is not null)
            {
                _defaultValue = ((decimal)_entityPropertyCorrespondence.PropertyDefinition.DefaultValue).ToString(CultureInfo.InvariantCulture);
            }
        }
        else if (propDef.FunctionalType is PFTInteger)
        {
            _sqlType = "integer";

            if (_entityPropertyCorrespondence.PropertyDefinition.DefaultValue is not null)
            {
                _defaultValue = ((int)_entityPropertyCorrespondence.PropertyDefinition.DefaultValue).ToString();
            }
        }
        else if (propDef.FunctionalType is PFTUniqueCode)
        {
            SetupUniqueCodeField(propDef);
        }
        else if (propDef.FunctionalType is PFTString)
        {
            SetupStringField(propDef);
        }
        else
        {
            throw new ApplicationException(string.Format("Unsupported PropertyFunctionalType for ValueField: {0}.", propDef.FunctionalType.GetType().Name));
        }
    }

    /// <summary>
    /// Generate text representation of a declaration of SQL table field
    /// </summary>
    /// <returns>SQL declaration of a table field</returns>
    public virtual string[] GenerateText()
    {
        var result = string.Format("{0}{1}{2} {3} {4}", _quoteSymbol, _name, _quoteSymbol, _sqlType, _nullable ? "null" : "not null");

        if (_generateConstraintsInline)
        {
            if (_unique)
            {
                result += " " + DBSchemaHelper.C_KEYWORD_UNIQUE;
            }

            if (_defaultValue is not null)
            {
                result += string.Format(" {0} {1}", DBSchemaHelper.C_KEYWORD_DEFAULT, _defaultValue);
            }
        }

        return [result];
    }
}
