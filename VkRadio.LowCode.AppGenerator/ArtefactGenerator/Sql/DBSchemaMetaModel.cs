using System;
using System.Collections.Generic;

using MetaModel.DOTDefinition;
using MetaModel.Names;
using MetaModel.PredefinedDO;
using MetaModel.PropertyDefinition;
using MetaModel.PropertyDefinition.ConcreteFunctionalTypes;
using MetaModel.PropertyDefinition.SystemFunctionalTypes;
using MetaModel.Relationship;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    public abstract class DBSchemaMetaModel
    {
        protected MetaModel.MetaModel _metaModel;
        protected ArtefactGeneratorSql _artefactGeneratorSql;
        protected SchemaDeploymentScript _schemaDeploymentScript;
        protected Dictionary<Guid, TableAndSourceCorrespondence> _tableAndSourceCorrespondence = new();
        protected bool _supportsForeignKeyConstraints;

        protected abstract Table CreateTable(DOTDefinition in_dotDefinition);
        protected virtual string GenerateTableName(DOTDefinition in_dotDefinition) { return NameHelper.NameToUnderscoreSeparatedName(in_dotDefinition.Names[HumanLanguageEnum.En]); }
        protected abstract ValueField CreateTableFieldValue(TableAndDOTCorrespondence in_correspondense, PropertyDefinition in_propertyDefinition);
        protected abstract ForeignKeyField CreateForeignKeyField(TableAndDOTCorrespondence in_correspondense, PropertyDefinition in_propertyDefinition);
        protected abstract PredefinedInsert CreatePredefinedInsert();
        protected abstract FieldValueKey CreateFieldValueKey(PredefinedInsert in_predefinedInsert, ITableField in_field, Guid in_value);
        protected abstract string GetValueStringForRefId(SRefObject in_value);
        protected abstract string GetValueStringForUniqueCode(Guid in_value);
        /// <summary>
        /// Получение строкового представления для функции генерации уникального кода (GUID)
        /// </summary>
        /// <returns></returns>
        protected abstract string GetDefaultStringRepForUniqueCodeGenerator();
        protected abstract SchemaDeploymentScript CreateSchemaDeploymentScript();

        public MetaModel.MetaModel MetaModel => _metaModel;
        public ArtefactGeneratorSql ArtefactGeneratorSql => _artefactGeneratorSql;
        public SchemaDeploymentScript SchemaDeploymentScript => _schemaDeploymentScript;
        public IDictionary<Guid, TableAndSourceCorrespondence> TableAndSourceCorrespondence => _tableAndSourceCorrespondence;
        /// <summary>
        /// Пишутся ли constraints табличных полей непосредственно на тех же строках, что и сами поля в create table (в противном случае они пишутся отдельно после объявления создания таблицы)
        /// </summary>
        public bool GenerateConstraintsInline { get; protected set; }

        public static string GetReferencedTableName(PropertyCorrespondence in_propertyCorrespondence, IDictionary<Guid, TableAndSourceCorrespondence> in_tableAndSourceCorrespondence)
        {
            string refTableName;
            if (in_propertyCorrespondence.PropertyDefinition.FunctionalType is PFTReferenceValue pftRefVal)
            {
                refTableName = ((TableAndDOTCorrespondence)in_tableAndSourceCorrespondence[pftRefVal.RelationshipReference.ReferenceDefinition.Id]).Table.Name;
            }
            else
            {
                var pftTableOwner = in_propertyCorrespondence.PropertyDefinition.FunctionalType as PFTTableOwner;
                if (pftTableOwner != null)
                {
                    refTableName = ((TableAndDOTCorrespondence)in_tableAndSourceCorrespondence[pftTableOwner.RelationshipTable.PropertyDefinitionInOwner.OwnerDefinition.Id]).Table.Name;
                }
                else
                {
                    var pftConnector = in_propertyCorrespondence.PropertyDefinition.FunctionalType as PFTConnector;
                    if (pftConnector != null)
                    {
                        var otherEnd = pftConnector.RelationshipConnector.End1.PropertyDefinition.Id == in_propertyCorrespondence.PropertyDefinition.Id ?
                            pftConnector.RelationshipConnector.End2.PropertyDefinition :
                            pftConnector.RelationshipConnector.End1.PropertyDefinition;
                        refTableName = ((TableAndDOTCorrespondence)in_tableAndSourceCorrespondence[otherEnd.OwnerDefinition.Id]).Table.Name;
                    }
                    else
                    {
                        throw new ApplicationException(string.Format("Unknown reference type for property Id {0}: {1}.", in_propertyCorrespondence.PropertyDefinition.Id, in_propertyCorrespondence.PropertyDefinition.FunctionalType.GetType().Name));
                    }
                }
            }
            return refTableName;
        }

        /// <summary>
        /// Инициализация метамодели схемы БД - генерирование схемы из основной метамодели
        /// </summary>
        public void Init()
        {
            _schemaDeploymentScript = CreateSchemaDeploymentScript();

            #region 1. Генерирование описаний таблиц, соответствующих DOTDefinition.
            foreach (var dotDef in _metaModel.AllDOTDefinitions.Values)
            {
                var tableDef = CreateTable(dotDef);

                var correspondence = new TableAndDOTCorrespondence { DBSchemaMetaModel = this, DOTDefinition = dotDef, Table = tableDef };
                _tableAndSourceCorrespondence.Add(correspondence.DOTDefinition.Id, correspondence);

                _schemaDeploymentScript.Tables.Add(tableDef.Name, tableDef);

                foreach (PropertyDefinition propDef in dotDef.PropertyDefinitions.Values)
                {
                    ITableField field = null;

                    // Свойство хранит непосредственное значение?
                    if (!(propDef.FunctionalType is PFTLink))
                    {
                        var vf = CreateTableFieldValue(correspondence, propDef);
                        tableDef.ValueFields.Add(vf);
                        field = vf;
                    }
                    else
                    {
                        // Если свойство является ссылкой на подчиненную таблицу, игнорируем его
                        // (т.к. в реляционных БД ставится только обратная ссылка с подчиненной таблицы
                        // на таблицу-владельца). В остальных случаях добавляем поле внешнего ключа.
                        if (!(propDef.FunctionalType is PFTTablePart || propDef.FunctionalType is PFTBackReferencedTable))
                        {
                            field = CreateForeignKeyField(correspondence, propDef);
                            tableDef.ForeignKeyFields.Add((ForeignKeyField)field);
                        }
                    }

                    if (field != null)
                        correspondence.PropertyCorrespondences.Add(field.DOTPropertyCorrespondence);
                }
            }
            #endregion

            // TODO: Не релизовано генерирование описаний таблиц, соответствующих RegisterDefinition.
            //#region 2. Генерирование описаний таблиц, соответствующих RegisterDefinition.
            //#endregion

            #region 3. Генерирование описаний вставок предопределенных объектов.
            foreach (DOTDefinition dotDef in _metaModel.AllDOTDefinitions.Values)
            {
                foreach (PredefinedDO pdo in dotDef.PredefinedDOs)
                {
                    var corr = (TableAndDOTCorrespondence)_tableAndSourceCorrespondence[dotDef.Id];

                    var pi = CreatePredefinedInsert();
                    pi.SchemaDeploymentScript = _schemaDeploymentScript;
                    pi.Table = corr.Table;
                    _schemaDeploymentScript.PredefinedInserts.Add(pi);

                    pi.FieldValues.Add(CreateFieldValueKey(pi, (PKSingle)corr.Table.PrimaryKey, pdo.Id));

                    foreach (var pCorr in corr.PropertyCorrespondences)
                    {
                        var valueObject = pdo.PropertyValues[pCorr.PropertyDefinition.Id].ValueObject;
                        string valueObjectAsString;
                        if (valueObject == null)
                        {
                            valueObjectAsString = "NULL";
                        }
                        else if (valueObject is SDateTime)
                        {
                            SDateTime dtValue = (SDateTime)valueObject;
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
                            SRefObject sref = (SRefObject)valueObject;
                            valueObjectAsString = GetValueStringForRefId(sref);
                        }
                        else if (valueObject is SGuid)
                        {
                            SGuid sguid = (SGuid)valueObject;
                            valueObjectAsString = sguid.GenerateValueAtRunTime ?
                                GetDefaultStringRepForUniqueCodeGenerator() :
                                GetValueStringForUniqueCode(sguid.FixedValue);
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
                            // TODO: Уточнить все типы данных, правильно ли они будут преобразоваться в значение SQL.
                            // TODO: Здесь не учтена возможная проблема с эскейпами в случае, если это будет строковый литерал.
                            valueObjectAsString = valueObject.ToString();
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

            #region 4. Генерирование связок по внешним ключам.
            if (_supportsForeignKeyConstraints)
            {
                foreach (var srcCorr in _tableAndSourceCorrespondence.Values)
                {
                    var corr = srcCorr as TableAndDOTCorrespondence;
                    if (corr == null)
                        continue;
                    foreach (var propCorr in corr.PropertyCorrespondences)
                    {
                        if (propCorr.PropertyDefinition == null)
                            continue;
                        var dependentLink = propCorr.PropertyDefinition.FunctionalType as IPFTDependentLink;
                        if (dependentLink != null && dependentLink.OnDeleteAction != OnDeleteActionEnum.Ingnore)
                        {
                            ForeignKeyConstraint fkConstraint = new ForeignKeyConstraint(
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
                int result = string.Compare(fk1.TableName, fk2.TableName);
                if (result == 0)
                {
                    result = string.Compare(fk1.RefFieldName, fk2.RefFieldName);
                    //if (result == 0)
                    //    result = string.Compare(fk1.RefTableName, fk2.RefTableName);
                }
                return result;
            });
            #endregion
        }

        /// <summary>
        /// Конструктор метамодели схемы БД
        /// </summary>
        /// <param name="in_metaModel">Основная метамодель ПОБ</param>
        /// <param name="in_artefactGeneratorSql">Генератор артефактов SQL</param>
        public DBSchemaMetaModel(MetaModel.MetaModel in_metaModel, ArtefactGeneratorSql in_artefactGeneratorSql)
        {
            _metaModel = in_metaModel;
            _artefactGeneratorSql = in_artefactGeneratorSql;
        }

        /// <summary>
        /// Получение строкового литерала для строкового значения свойства ТОД или ПОД
        /// </summary>
        /// <param name="in_value">Строковое значение свойства ТОД или ПОД</param>
        /// <returns>Адаптированный к диалекту SQL строковый литерал</returns>
        public virtual string GetValueStringForString(string in_value) { return "'" + in_value.Replace("'", "''").Replace("\n", "' + char(10) + '") + "'"; } // CR = char(13), LF = char(10)
    };
}
