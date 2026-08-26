using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core;
using VkRadio.LowCode.AppGen.Domain;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Package;

/// <summary>
/// Source code model of abstract object oriented language for containing package (project directory)
/// </summary>
public class ProjectPackage : Package
{
    protected DomainModel _domainModel;
    protected DBSchemaDomainModel _dbSchemaModel;
    protected Target _artefactGenerationTarget;

    /// <summary>
    /// Protected constructor for disabling object creation withot parameters
    /// </summary>
    protected ProjectPackage()
    {
    }

    /// <summary>
    /// Constructor with enforced parameters
    /// </summary>
    /// <param name="domainModel">Domain (business) model</param>
    /// <param name="in_artefactGenerator">Artefact generator</param>
    public ProjectPackage(DomainModel domainModel, Target target, DBSchemaDomainModel dbSchemaModel)
    {
        var di = new DirectoryInfo(target.OutputPath!);
        _name = di.Name;
        _fullPath = target.OutputPath!;
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
    public DomainModel DomainModel => _domainModel;

    /// <summary>
    /// Database schema model
    /// </summary>
    public DBSchemaDomainModel DBbSchemaModel => _dbSchemaModel;

    /// <summary>
    /// Artefact generation target
    /// </summary>
    public Target ArtefactGenerationTarget => _artefactGenerationTarget;
}
