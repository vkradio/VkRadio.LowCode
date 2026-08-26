using VkRadio.LowCode.AppGen.Domain;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.SystemFunctionalTypes;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.SqlCore;

public abstract class DBSchemaDomainModel
{
    protected DomainModel _domainModel;
    protected ArtefactGeneratorSql _artefactGeneratorSql;
    protected SchemaDeploymentScript _schemaDeploymentScript;
    protected Dictionary<Guid, TableAndSourceCorrespondence> _tableAndSourceCorrespondence = new();
    protected bool _supportsForeignKeyConstraints;

    protected abstract Table CreateTable(EntityDefinition entityDefinition);

    protected virtual string GenerateTableName(EntityDefinition entityDefinition) => NameHelper.NameToUnderscoreSeparatedName(entityDefinition.Names[NaturalLanguageEnum.En]);

    protected abstract ValueField CreateTableFieldValue(TableAndEntityCorrespondence correspondense, PropertyDefinition propertyDefinition);

    protected abstract ForeignKeyField CreateForeignKeyField(TableAndEntityCorrespondence correspondense, PropertyDefinition propertyDefinition);

    protected abstract PredefinedInsert CreatePredefinedInsert();

    protected abstract FieldValueKey CreateFieldValueKey(PredefinedInsert predefinedInsert, ITableField field, Guid value);

    protected abstract string GetValueStringForRefId(SRefObject value);

    protected abstract string GetValueStringForUniqueCode(Guid value);

    /// <summary>
    /// Get a string representation of a call to unique code (GUID) generation function
    /// </summary>
    /// <returns></returns>
    protected abstract string GetDefaultStringRepForUniqueCodeGenerator();

    protected abstract SchemaDeploymentScript CreateSchemaDeploymentScript();

    public DomainModel DomainModel => _domainModel;

    public ArtefactGeneratorSql ArtefactGeneratorSql => _artefactGeneratorSql;

    public SchemaDeploymentScript SchemaDeploymentScript => _schemaDeploymentScript;

    public IDictionary<Guid, TableAndSourceCorrespondence> TableAndSourceCorrespondence => _tableAndSourceCorrespondence;

    public bool GenerateConstraintsInline { get; protected set; }

    public static string GetReferencedTableName(PropertyCorrespondence propertyCorrespondence, IDictionary<Guid, TableAndSourceCorrespondence> tableAndSourceCorrespondence)
    {
        string refTableName;

        if (propertyCorrespondence.PropertyDefinition.FunctionalType is PFTReferenceValue pftRefVal)
        {
            refTableName = ((TableAndEntityCorrespondence)tableAndSourceCorrespondence[pftRefVal.RelationshipReference.ReferenceDefinition.Id]).Table.Name;
        }
        else
        {
            var pftTableOwner = propertyCorrespondence.PropertyDefinition.FunctionalType as PFTTableOwner;

            if (pftTableOwner is not null)
            {
                refTableName = ((TableAndEntityCorrespondence)tableAndSourceCorrespondence[pftTableOwner.RelationshipTable.PropertyDefinitionInOwner.OwnerDefinition.Id]).Table.Name;
            }
            else
            {
                var pftConnector = propertyCorrespondence.PropertyDefinition.FunctionalType as PFTConnector;

                if (pftConnector is not null)
                {
                    var otherEnd = pftConnector.RelationshipConnector.End1.PropertyDefinition.Id == propertyCorrespondence.PropertyDefinition.Id
                        ? pftConnector.RelationshipConnector.End2.PropertyDefinition
                        : pftConnector.RelationshipConnector.End1.PropertyDefinition;

                    refTableName = ((TableAndEntityCorrespondence)tableAndSourceCorrespondence[otherEnd.OwnerDefinition.Id]).Table.Name;
                }
                else
                {
                    throw new ApplicationException(string.Format("Unknown reference type for property Id {0}: {1}.", propertyCorrespondence.PropertyDefinition.Id, propertyCorrespondence.PropertyDefinition.FunctionalType.GetType().Name));
                }
            }
        }

        return refTableName;
    }

    public void Init()
    {
        _schemaDeploymentScript = CreateSchemaDeploymentScript();

        #region 1. Generate table declarations based on data object type definitions
        foreach (var dotDef in _domainModel.AllEntityDefinitions.Values)
        {
            var tableDef = CreateTable(dotDef);

            var correspondence = new TableAndEntityCorrespondence
            {
                DBSchemaDomainModel = this,
                EntityDefinition = dotDef,
                Table = tableDef
            };

            _tableAndSourceCorrespondence.Add(correspondence.EntityDefinition.Id, correspondence);

            _schemaDeploymentScript.Tables.Add(tableDef.Name, tableDef);

            foreach (var propDef in dotDef.PropertyDefinitions.Values)
            {
                ITableField? field = null;

                // Does field stores an explicit value?
                if (!(propDef.FunctionalType is PFTLink))
                {
                    var vf = CreateTableFieldValue(correspondence, propDef);
                    tableDef.ValueFields.Add(vf);
                    field = vf;
                }
                else
                {
                    // If a property is a reference on a table, ignore it (because in relational databases only settings FKs
                    // back to owner tables). Otherwise add field of a Foreign Key.
                    if (!(propDef.FunctionalType is PFTTablePart || propDef.FunctionalType is PFTBackReferencedTable))
                    {
                        field = CreateForeignKeyField(correspondence, propDef);
                        tableDef.ForeignKeyFields.Add((ForeignKeyField)field);
                    }
                }

                if (field is not null)
                {
                    correspondence.PropertyCorrespondences.Add(field.EntityPropertyCorrespondence);
                }
            }
        }
        #endregion

        #region 2. Generate descriptions for predefined inserts of data rows
        foreach (var entDef in _domainModel.AllEntityDefinitions.Values)
        {
            foreach (var pde in entDef.PredefinedEntityInstances)
            {
                var corr = (TableAndEntityCorrespondence)_tableAndSourceCorrespondence[entDef.Id];

                var pi = CreatePredefinedInsert();
                pi.SchemaDeploymentScript = _schemaDeploymentScript;
                pi.Table = corr.Table;
                _schemaDeploymentScript.PredefinedInserts.Add(pi);

                pi.FieldValues.Add(CreateFieldValueKey(pi, (PKSingle)corr.Table.PrimaryKey, pde.Id));

                foreach (var pCorr in corr.PropertyCorrespondences)
                {
                    var valueObject = pde.PropertyValues[pCorr.PropertyDefinition.Id].ValueObject;
                    string valueObjectAsString;

                    if (valueObject is null)
                    {
                        valueObjectAsString = "NULL";
                    }
                    else if (valueObject is SDateTime)
                    {
                        var dtValue = (SDateTime)valueObject;

                        if (dtValue.UseModelRuntimeValue)
                        {
                            valueObjectAsString = "CURRENT_TIMESTAMP";
                        }
                        else
                        {
                            //TimeZoneInfo tzi = _artefactGeneratorSql.Target.Project.TimeZone;
                            //DateTime dtTimeZoneCorrected = dtValue.FixedValue.Subtract(tzi.BaseUtcOffset);
                            //valueObjectAsString = "'" + dtTimeZoneCorrected.ToString("yyyy-MM-dd HH:mm:ss") + "'";

                            valueObjectAsString = $"'{dtValue.FixedValue.ToString("yyyy-MM-dd HH:mm:ss")}'";
                        }
                    }
                    else if (valueObject is SRefObject)
                    {
                        var sref = (SRefObject)valueObject;
                        valueObjectAsString = GetValueStringForRefId(sref);
                    }
                    else if (valueObject is SGuid)
                    {
                        var sguid = (SGuid)valueObject;

                        valueObjectAsString = sguid.GenerateValueAtRunTime
                            ? GetDefaultStringRepForUniqueCodeGenerator()
                            : GetValueStringForUniqueCode(sguid.FixedValue);
                    }
                    else if (valueObject is string)
                    {
                        valueObjectAsString = GetValueStringForString((string)valueObject);
                    }
                    else if (valueObject is bool)
                    {
                        valueObjectAsString = (bool)valueObject ? "1" : "0";
                    }
                    else
                    {
                        // TODO: Make sure for all data types, if they will be correctly translated to SQL values.
                        // TODO: Need to think about a potential problem with escapes in case it will be a string literal.
                        valueObjectAsString = valueObject?.ToString() ?? string.Empty;
                    }

                    var fv = new FieldValue
                    {
                        PredefinedInsert = pi,
                        Field = pCorr.TableField,
                        Value = valueObjectAsString
                    };

                    pi.FieldValues.Add(fv);
                }
            }
        }

        _schemaDeploymentScript.PredefinedInserts.Sort(PredefinedInsert.PredefinedInsertComparer);
        #endregion

        #region 3. Generate FK links
        if (_supportsForeignKeyConstraints)
        {
            foreach (var srcCorr in _tableAndSourceCorrespondence.Values)
            {
                var corr = srcCorr as TableAndEntityCorrespondence;

                if (corr is null)
                {
                    continue;
                }

                foreach (var propCorr in corr.PropertyCorrespondences)
                {
                    if (propCorr.PropertyDefinition is null)
                    {
                        continue;
                    }

                    var dependentLink = propCorr.PropertyDefinition.FunctionalType as IPFTDependentLink;

                    if (dependentLink is not null && dependentLink.OnDeleteAction != OnDeleteActionEnum.Ingnore)
                    {
                        var fkConstraint = new ForeignKeyConstraint(
                            corr.Table.Name,
                            GetReferencedTableName(propCorr, _tableAndSourceCorrespondence),
                            propCorr.TableField.Name,
                            dependentLink.OnDeleteAction
                        );

                        _schemaDeploymentScript.FKConstraints.Add(fkConstraint);
                    }
                }
            }
        }

        _schemaDeploymentScript.FKConstraints.Sort((fk1, fk2) =>
        {
            var result = string.Compare(fk1.TableName, fk2.TableName);

            if (result == 0)
            {
                result = string.Compare(fk1.RefFieldName, fk2.RefFieldName);

                //if (result == 0)
                //{
                //    result = string.Compare(fk1.RefTableName, fk2.RefTableName);
                //}
            }

            return result;
        });
        #endregion
    }

    public DBSchemaDomainModel(DomainModel domainModel, ArtefactGeneratorSql artefactGeneratorSql)
    {
        _domainModel = domainModel;
        _artefactGeneratorSql = artefactGeneratorSql;
    }

    /// <summary>
    /// Get a string literal for a Entity or predefined object property
    /// </summary>
    /// <param name="value">String value of a DOT of predefined object</param>
    /// <returns>String literal adapted for a particular SQL dialect</returns>
    public virtual string GetValueStringForString(string value) { return "'" + value.Replace("'", "''").Replace("\n", "' + char(10) + '") + "'"; } // CR = char(13), LF = char(10)
}
