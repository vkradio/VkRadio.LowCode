using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using MetaModel.Names;
using ag = ArtefactGenerationProject.ArtefactGenerator;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular;
using ArtefactGenerationProject.ArtefactGenerator.Sql.MsSql;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp2;
//using ArtefactGenerationProject.ArtefactGenerator.Sql.MySql;
//using ArtefactGenerationProject.ArtefactGenerator.Sql.SQLite;

namespace VkRadio.LowCode.AppGenerator;

/// <summary>
/// Описание цели генерации артефактов
/// </summary>
public abstract class ArtefactGenerationTarget : IUnique
{
    protected Guid? _useOutputPathFromTargetId;
    protected List<Guid> _dependsOnIds;

    protected virtual void InitConcrete(XElement in_xelTarget) { }

    /// <summary>
    /// Проект генерации артефактов
    /// </summary>
    public ArtefactGenerationProject Project { get; private set; }
    /// <summary>
    /// Вышестоящая цель
    /// </summary>
    public ArtefactGenerationTarget ParentTarget { get; private set; }
    /// <summary>
    /// Уникальный идетификатор цели
    /// </summary>
    public Guid Id { get; private set; }
    /// <summary>
    /// Полный путь к папке генерирования артефактов
    /// </summary>
    public string OutputPath { get; private set; }
    /// <summary>
    /// Тип генерируемого артефакта
    /// </summary>
    public ArtefactTypeCodeEnum Type { get; private set; }
    /// <summary>
    /// Цели, от результатов генерирования которых зависит данная цель.
    /// Эти цели должны быть достигнуты перед запуском данной цели.
    /// </summary>
    public IDictionary<Guid, ArtefactGenerationTarget> DependsOn { get; private set; }
    /// <summary>
    /// Цели, зависящие от данной цели.
    /// Эти цели могут исполняться не раньше, чем будет исполнена данная цель.
    /// </summary>
    public IDictionary<Guid, ArtefactGenerationTarget> Dependants { get; private set; } = new Dictionary<Guid, ArtefactGenerationTarget>();
    /// <summary>
    /// Генерация цели выполнена успешно
    /// </summary>
    public bool GenerateSuccess { get; set; } = true;
    /// <summary>
    /// Подцели
    /// </summary>
    public List<ArtefactGenerationTarget> Subtargets { get; private set; } = new List<ArtefactGenerationTarget>();
    public ag.ArtefactGenerator Generator { get; private set; }

    /// <summary>
    /// Загрузка цели из узла XML
    /// </summary>
    /// <param name="in_project">Проект генерации артефактов</param>
    /// <param name="in_xelTarget">Узел XML с описанием цели генерации</param>
    /// <param name="in_xelTarget">Вышестоящая цель генерации, если есть</param>
    /// <returns>Цель генерации артефактов</returns>
    public static ArtefactGenerationTarget LoadFromXElement(ArtefactGenerationProject in_project, XElement in_xelTarget, ArtefactGenerationTarget in_parentTarget = null)
    {
        var id = new Guid(in_xelTarget.Element("Id").Value);
        var xelOutputPath = in_xelTarget.Element("OutputPath");
        string outputPath = null;
        if (xelOutputPath != null)
            outputPath = Path.Combine(in_project.ProjectRootPath, xelOutputPath.Value);

        var dependsOnIds = new List<Guid>();
        var xelDependencies = in_xelTarget.Element("DependsOn");
        Guid? useOutputPathFromTargetId = null;
        if (xelDependencies != null)
        {
            foreach (var xelDepId in xelDependencies.Elements("TargetId"))
            {
                var dependencyId = new Guid(xelDepId.Value);
                dependsOnIds.Add(dependencyId);

                if (outputPath == null)
                {
                    var xat = xelDepId.Attribute("useOutputPath");
                    if (xat != null && xat.Value == "True")
                        useOutputPathFromTargetId = dependencyId;
                }
            }
        }

        var artefactTypeCode = ArtefactType.Parse(in_xelTarget.Element("ArtefactType").Value);
        ArtefactGenerationTarget target;
        switch (artefactTypeCode)
        {
            //case ArtefactTypeCodeEnum.MySql:
            //    target = new TargetMySql();
            //    break;
            case ArtefactTypeCodeEnum.MsSql:
                target = new TargetMsSql();
                break;
            //case ArtefactTypeCodeEnum.SQLite:
            //    target = new TargetSQLite();
            //    break;
            //case ArtefactTypeCodeEnum.PhpZf:
            //    generator = new ArtefactGeneratorPhpZf() { _code = in_type, _metaModel = in_metaModel, _target = in_target };
            //    break;
            case ArtefactTypeCodeEnum.CSharp:
                target = new GenerationTargetCSharp();
                break;
            //case ArtefactTypeCodeEnum.CSharpApplication:
            //    target = new TargetCSharpApplication();
            //    break;
            //case ArtefactTypeCodeEnum.CSharpProjectModel:
            //    target = new TargetCSharpProjectModel();
            //    break;
            //case ArtefactTypeCodeEnum.CSharpOldVersionSave:
            //    generator = new ArtefactGeneratorCSharpOldVersionSave() { _code = in_type, _metaModel = in_metaModel, _target = in_target };
            //    break;
            //case ArtefactTypeCodeEnum.CSharpProjectVersion:
            //    generator = new ArtefactGeneratorCSharpProjectVersion() { _code = in_type, _metaModel = in_metaModel, _target = in_target };
            //    break;
            //case ArtefactTypeCodeEnum.InnoSetup:
            //    generator = new ArtefactGeneratorInnoSetup() { _code = in_type, _metaModel = in_metaModel, _target = in_target };
            //    break;
            //case ArtefactTypeCodeEnum.MSBuild:
            //    generator = new ArtefactGeneratorMSBuild() { _code = in_type, _metaModel = in_metaModel, _target = in_target };
            //    break;
            default:
                throw new ApplicationException($"Unrecognized ArtefactTypeCodeEnum value: {Enum.GetName(typeof(ArtefactTypeCodeEnum), artefactTypeCode)}.");
        }

        target.Id = id;
        target.OutputPath = outputPath;
        target.Project = in_project;
        target._dependsOnIds = dependsOnIds;
        target._useOutputPathFromTargetId = useOutputPathFromTargetId;

        target.InitConcrete(in_xelTarget);

        //var xelSubtargets = in_xelTarget.Element("Subtargets");
        //if (xelSubtargets != null)
        //{
        //    foreach (var xelSubtarget in xelSubtargets.Elements("Target"))
        //        target.Subtargets.Add(LoadFromXElement(in_project, xelSubtarget, target));
        //    foreach (var subtarget in target.Subtargets)
        //        subtarget.DeferredLinkDependencies(target.Subtargets);
        //}

        target.Generator = ag.ArtefactGenerator.CreateConcrete(target, in_project.MetaModel);

        return target;
    }

    /// <summary>
    /// Генерирование артефактов цели
    /// </summary>
    /// <returns>null, если генерация успешна, или сообщение о проблемах</returns>
    public string GenerateArtefacts()
    {
        string result = string.Empty;
        foreach (var subtarget in Subtargets)
        {
            var submessage = subtarget.GenerateArtefacts();
            if (!string.IsNullOrEmpty(submessage))
            {
                if (result != string.Empty)
                    result += "; ";
                result += submessage;
            }
        }
        var rootMessage = Generator.Generate();
        if (result != string.Empty)
            result += "; ";
        result += rootMessage;
        return result;
    }

    ///// <summary>
    ///// Отложенное связывание зависимостей
    ///// </summary>
    ///// <param name="in_allTargets">Все цели проекта</param>
    //public void DeferredLinkDependencies(IDictionary<Guid, ArtefactGenerationTarget> in_allTargets)
    //{
    //    DependsOn = new Dictionary<Guid, ArtefactGenerationTarget>(_dependsOnIds.Count);
    //    foreach (var depId in _dependsOnIds)
    //    {
    //        ArtefactGenerationTarget dep = in_allTargets[depId];
    //        DependsOn.Add(depId, dep);
    //        dep.Dependants.Add(Id, this);
    //    }

    //    if (OutputPath == null && _useOutputPathFromTargetId.HasValue)
    //        OutputPath = in_allTargets[_useOutputPathFromTargetId.Value].OutputPath;
    //}
    /// <summary>
    /// Отложенное связывание зависимостей
    /// </summary>
    /// <param name="in_subtargets">Подцели внутри цели</param>
    public void DeferredLinkDependencies(IList<ArtefactGenerationTarget> in_subtargets)
    {
        DependsOn = new Dictionary<Guid, ArtefactGenerationTarget>(_dependsOnIds.Count);
        foreach (var depId in _dependsOnIds)
        {
            ArtefactGenerationTarget dep = in_subtargets.Where(s => s.Id == depId).Single();
            DependsOn.Add(depId, dep);
            dep.Dependants.Add(Id, this);
        }

        if (OutputPath == null && _useOutputPathFromTargetId.HasValue)
            OutputPath = in_subtargets.Where(s => s.Id == _useOutputPathFromTargetId.Value).Single().OutputPath;
    }
}
