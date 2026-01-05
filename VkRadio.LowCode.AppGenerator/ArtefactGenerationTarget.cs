using System.Xml.Linq;
using VkRadio.LowCode.AppGenerator.MetaModel.Names;
using AG = VkRadio.LowCode.AppGenerator.ArtefactGenerator;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql.MsSql;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp2;
//using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql.MySql;
//using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql.SQLite;

namespace VkRadio.LowCode.AppGenerator;

/// <summary>
/// Description of an artefact generatiopn target
/// </summary>
public abstract class ArtefactGenerationTarget : IUnique
{
    protected Guid? _useOutputPathFromTargetId;
    protected List<Guid> _dependsOnIds;

    protected virtual void InitConcrete(XElement in_xelTarget) { }

    /// <summary>
    /// Artefact generation project
    /// </summary>
    public ArtefactGenerationProject Project { get; private set; }
    /// <summary>
    /// Parent (upper level) target
    /// </summary>
    public ArtefactGenerationTarget ParentTarget { get; private set; }
    /// <summary>
    /// Unique identifier of a target
    /// </summary>
    public Guid Id { get; private set; }
    /// <summary>
    /// Full path to a folder with generated artefacts
    /// </summary>
    public string OutputPath { get; private set; }
    /// <summary>
    /// Type of a generated artefact
    /// </summary>
    public ArtefactTypeCodeEnum Type { get; private set; }
    /// <summary>
    /// Targets, on that success this target depends on
    /// </summary>
    public IDictionary<Guid, ArtefactGenerationTarget> DependsOn { get; private set; }
    /// <summary>
    /// Targets, that depends on success of this target
    /// </summary>
    public IDictionary<Guid, ArtefactGenerationTarget> Dependants { get; private set; } = new Dictionary<Guid, ArtefactGenerationTarget>();
    /// <summary>
    /// Successful result
    /// </summary>
    public bool GenerateSuccess { get; set; } = true;
    /// <summary>
    /// Subtargets
    /// </summary>
    public List<ArtefactGenerationTarget> Subtargets { get; private set; } = new List<ArtefactGenerationTarget>();
    public AG.ArtefactGenerator Generator { get; private set; }

    /// <summary>
    /// Load target from an XML node
    /// </summary>
    /// <param name="project">Artefact generation project</param>
    /// <param name="xelTarget">XML node containing generation target description</param>
    /// <param name="xelTarget">Upper level target, if any</param>
    /// <returns>Artefact generation target</returns>
    public static ArtefactGenerationTarget LoadFromXElement(ArtefactGenerationProject project, XElement xelTarget, ArtefactGenerationTarget? parentTarget = null)
    {
        var id = new Guid(xelTarget.Element("Id")!.Value);
        var xelOutputPath = xelTarget.Element("OutputPath");
        string? outputPath = null;

        if (xelOutputPath is not null)
        {
            outputPath = Path.Combine(project.ProjectRootPath, xelOutputPath.Value);
        }

        var dependsOnIds = new List<Guid>();
        var xelDependencies = xelTarget.Element("DependsOn");
        Guid? useOutputPathFromTargetId = null;

        if (xelDependencies is not null)
        {
            foreach (var xelDepId in xelDependencies.Elements("TargetId"))
            {
                var dependencyId = new Guid(xelDepId.Value);
                dependsOnIds.Add(dependencyId);

                if (outputPath == null)
                {
                    var xat = xelDepId.Attribute("useOutputPath");

                    if (xat is not null && xat.Value == "True")
                    {
                        useOutputPathFromTargetId = dependencyId;
                    }
                }
            }
        }

        var artefactTypeCode = ArtefactType.Parse(xelTarget.Element("ArtefactType")!.Value);
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
        target.Project = project;
        target._dependsOnIds = dependsOnIds;
        target._useOutputPathFromTargetId = useOutputPathFromTargetId;

        target.InitConcrete(xelTarget);

        //var xelSubtargets = in_xelTarget.Element("Subtargets");
        //if (xelSubtargets != null)
        //{
        //    foreach (var xelSubtarget in xelSubtargets.Elements("Target"))
        //        target.Subtargets.Add(LoadFromXElement(in_project, xelSubtarget, target));
        //    foreach (var subtarget in target.Subtargets)
        //        subtarget.DeferredLinkDependencies(target.Subtargets);
        //}

        target.Generator = AG.ArtefactGenerator.CreateConcrete(target, project.MetaModel);

        return target;
    }

    /// <summary>
    /// Generate target artefacts
    /// </summary>
    /// <returns>If success - returns null, otherwise returns an error message</returns>
    public string GenerateArtefacts()
    {
        var result = string.Empty;

        foreach (var subtarget in Subtargets)
        {
            var submessage = subtarget.GenerateArtefacts();

            if (!string.IsNullOrEmpty(submessage))
            {
                if (result != string.Empty)
                {
                    result += "; ";
                }

                result += submessage;
            }
        }

        var rootMessage = Generator.Generate();

        if (result != string.Empty)
        {
            result += "; ";
        }

        result += rootMessage;

        return result;
    }

    /// <summary>
    /// Deferred linking
    /// </summary>
    /// <param name="subtargets">Subtargets inside of a target</param>
    public void DeferredLinkDependencies(IList<ArtefactGenerationTarget> subtargets)
    {
        DependsOn = new Dictionary<Guid, ArtefactGenerationTarget>(_dependsOnIds.Count);

        foreach (var depId in _dependsOnIds)
        {
            var dep = subtargets
                .Where(x => x.Id == depId)
                .Single();

            DependsOn.Add(depId, dep);

            dep.Dependants.Add(Id, this);
        }

        if (OutputPath is null && _useOutputPathFromTargetId.HasValue)
        {
            OutputPath = subtargets
                .Where(x => x.Id == _useOutputPathFromTargetId.Value)
                .Single()
                .OutputPath;
        }
    }
}
