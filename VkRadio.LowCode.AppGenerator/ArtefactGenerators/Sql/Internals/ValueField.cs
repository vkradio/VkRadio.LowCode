using System.Globalization;
using VkRadio.LowCode.AppGenerator.MetaModel.Names;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;

/// <summary>
/// Table field that stores an explicit value (not a foreign key or something alike)
/// </summary>
public abstract class ValueField : ITableField
{
    protected string _name;
    protected bool _nullable;
    protected string _sqlType;
    protected Table _table;
    protected PropertyCorrespondence _dotPropertyCorrespondence;
    protected string _defaultValue;
    protected bool _unique;
    protected string[] _sqlTypesWDeniedDefaults;
    protected string _boolSqlType;
    protected string _quoteSymbol;
    protected bool _generateConstraintsInline = true;

    ValueField() => throw new Exception();

    protected abstract void SetupStringField(PropertyDefinition propertyDefinition);
    protected virtual void SetupStringFieldDefault(bool defaultDeniedBySql, PropertyDefinition propertyDefinition)
    {
        if (!defaultDeniedBySql && propertyDefinition.DefaultValue is not null)
        {
            // TODO: Use the simplest SQL Escape. Indeally we need to implement a fully secure escape mechanism according to all rules
            _defaultValue = DOTPropertyCorrespondence.TableAndDOTCorrespondence.DBSchemaMetaModel.GetValueStringForString((string)propertyDefinition.DefaultValue);
            //_defaultValue = "N'" + ((string)in_propertyDefinition.DefaultValue).Replace("'", "''") + "'";
        }
    }
    protected abstract void SetupUniqueCodeField(PropertyDefinition propertyDefinition);
    protected virtual void SetupDateTimeField(PropertyDefinition propertyDefinition) { _sqlType = "datetime"; }

    /// <summary>
    /// Field name (without qout symbols)
    /// </summary>
    public string Name { get { return _name; } }
    /// <summary>
    /// Are NULL values allowed
    /// </summary>
    public bool Nullable { get { return _nullable; } }
    /// <summary>
    /// SQL type (string literal)
    /// </summary>
    public string SqlType { get { return _sqlType; } }
    /// <summary>
    /// Table that owns this field
    /// </summary>
    public Table Table { get { return _table; } }
    /// <summary>
    /// Data object type property definition, that corresponds to this field
    /// </summary>
    public PropertyCorrespondence? DOTPropertyCorrespondence => _dotPropertyCorrespondence;
    /// <summary>
    /// Default value (string literal, or null, if there is no default value)
    /// </summary>
    public string? DefaultValue { get { return _defaultValue; } }
    /// <summary>
    /// Is value unique throughout a table
    /// </summary>
    public bool Unique { get { return _unique; } }

    /// <summary>
    /// Table field constructor with an explicit value, based on a property definition
    /// </summary>
    /// <param name="tableAndDOTCorrespondence"></param>
    /// <param name="propertyDefinition"></param>
    public ValueField(TableAndDOTCorrespondence tableAndDOTCorrespondence, PropertyDefinition propertyDefinition)
    {
        _table = tableAndDOTCorrespondence.Table;
        _name = NameHelper.NameToUnderscoreSeparatedName(propertyDefinition.Names[HumanLanguageEnum.En]);
        _nullable = propertyDefinition.FunctionalType.Nullable;
        _unique = propertyDefinition.FunctionalType.Unique;
        _dotPropertyCorrespondence = new PropertyCorrespondence
        {
            PropertyDefinition = propertyDefinition,
            TableField = this,
            TableAndDOTCorrespondence = tableAndDOTCorrespondence
        };
    }

    public void Init()
    {
        var propDef = _dotPropertyCorrespondence.PropertyDefinition;

        // Set the SQL type and default value.
        // TODO: Make SQL types as close to SQL standard as possible
        if (propDef.FunctionalType is PFTBoolean)
        {
            _sqlType = _boolSqlType;

            if (_dotPropertyCorrespondence.PropertyDefinition.DefaultValue is not null)
            {
                _defaultValue = (bool)_dotPropertyCorrespondence.PropertyDefinition.DefaultValue
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

            if (_dotPropertyCorrespondence.PropertyDefinition.DefaultValue is not null)
            {
                _defaultValue = ((decimal)_dotPropertyCorrespondence.PropertyDefinition.DefaultValue).ToString(CultureInfo.InvariantCulture);
            }
        }
        else if (propDef.FunctionalType is PFTInteger)
        {
            _sqlType = "integer";

            if (_dotPropertyCorrespondence.PropertyDefinition.DefaultValue is not null)
            {
                _defaultValue = ((int)_dotPropertyCorrespondence.PropertyDefinition.DefaultValue).ToString();
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
