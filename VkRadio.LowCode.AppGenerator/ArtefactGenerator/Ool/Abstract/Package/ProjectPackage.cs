using System.IO;

using ArtefactGenerationProject.ArtefactGenerator.Sql;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Package
{
    /// <summary>
    /// Source code model of abstract object oriented language for containing package (project directory)
    /// </summary>
    public class ProjectPackage: Package
    {
        protected MetaModel.MetaModel _domainModel;
        protected DBSchemaMetaModelJson _dbSchemaModel;
        protected ArtefactGenerationTargetJson _artefactGenerationTarget;

        /// <summary>
        /// Protected constructor for disabling object creation withot parameters
        /// </summary>
        protected ProjectPackage() {}
        /// <summary>
        /// Constructor with enforced parameters
        /// </summary>
        /// <param name="in_domainModel">Domain (business) model</param>
        /// <param name="in_artefactGenerator">Artefact generator</param>
        public ProjectPackage(MetaModel.MetaModel in_domainModel, ArtefactGenerationTargetJson in_target, DBSchemaMetaModelJson in_dbSchemaModel)
        {
            var di = new DirectoryInfo(in_target.OutputPath);
            _name = di.Name;
            _fullPath = in_target.OutputPath;
            _domainModel = in_domainModel;
            _artefactGenerationTarget = in_target;
            _dbSchemaModel = in_dbSchemaModel;
        }

        /// <summary>
        /// Обязательная виртуальная инициализация экземпляра конкретного класса.
        /// </summary>
        public virtual void Init() {}

        /// <summary>
        /// Domain (business) model
        /// </summary>
        public MetaModel.MetaModel DomainModel { get { return _domainModel; } }
        /// <summary>
        /// Database schema model
        /// </summary>
        public DBSchemaMetaModelJson DBbSchemaModel { get { return _dbSchemaModel; } }
        /// <summary>
        /// Artefact generation target
        /// </summary>
        public ArtefactGenerationTargetJson ArtefactGenerationTarget { get { return _artefactGenerationTarget; } }
    };
}
