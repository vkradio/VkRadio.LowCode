using VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;
using VkRadio.LowCode.AppGenerator.Targets;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Package;

/// <summary>
/// Source code model of abstract object oriented language for containing package (project directory)
/// </summary>
public class ProjectPackage: Package
{
    protected MetaModel.MetaModel _domainModel;
    protected DBSchemaMetaModel _dbSchemaModel;
    protected ArtefactGenerationTarget _artefactGenerationTarget;

    /// <summary>
    /// Protected constructor for disabling object creation withot parameters
    /// </summary>
    protected ProjectPackage() {}
    /// <summary>
    /// Constructor with enforced parameters
    /// </summary>
    /// <param name="domainModel">Domain (business) model</param>
    /// <param name="in_artefactGenerator">Artefact generator</param>
    public ProjectPackage(MetaModel.MetaModel domainModel, ArtefactGenerationTarget target, DBSchemaMetaModel dbSchemaModel)
    {
        var di = new DirectoryInfo(target.OutputPath);
        _name = di.Name;
        _fullPath = target.OutputPath;
        _domainModel = domainModel;
        _artefactGenerationTarget = target;
        _dbSchemaModel = dbSchemaModel;
    }

    /// <summary>
    /// Obligatory virtual initialization of an instance of concrete class
    /// </summary>
    public virtual void Init()
    {
    }

    /// <summary>
    /// Domain (business) model
    /// </summary>
    public MetaModel.MetaModel DomainModel { get { return _domainModel; } }
    /// <summary>
    /// Database schema model
    /// </summary>
    public DBSchemaMetaModel DBbSchemaModel { get { return _dbSchemaModel; } }
    /// <summary>
    /// Artefact generation target
    /// </summary>
    public ArtefactGenerationTarget ArtefactGenerationTarget { get { return _artefactGenerationTarget; } }
}
