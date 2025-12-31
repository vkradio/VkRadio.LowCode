using System.Collections.Generic;
using System.IO;

using CompNS = ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Component;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Package
{
    /// <summary>
    /// Пакет исходного кода
    /// </summary>
    public abstract class Package
    {
        protected Package _parentPackage;
        protected Dictionary<string, Package> _subpackages = new Dictionary<string, Package>();
        protected string _fullPath;
        protected string _name;
        protected Dictionary<string, CompNS.Component> _components = new Dictionary<string, CompNS.Component>();

        public Package() {}
        public Package(Package in_parentPackage, string in_name)
        {
            _parentPackage = in_parentPackage;
            Name = in_name;
        }

        /// <summary>
        /// Родительский пакет (может отсутствовать, если текущий пакет является
        /// пакетом корневого уровня)
        /// </summary>
        public Package ParentPackage { get { return _parentPackage; } }
        /// <summary>
        /// Вложенные пакеты
        /// </summary>
        public IDictionary<string, Package> Subpackages { get { return _subpackages; } } 
        /// <summary>
        /// Полный путь директории пакета
        /// </summary>
        public string FullPath { get { return _fullPath; } }
        /// <summary>
        /// Имя пакета (т.е. имя директории пакета)
        /// </summary>
        public string Name { get { return _name; } set { _name = value; _fullPath = Path.Combine(_parentPackage.FullPath, _name); } }
        /// <summary>
        /// Вложенные компоненты (файлы исходного кода)
        /// </summary>
        public IDictionary<string, CompNS.Component> Components { get { return _components; } }

        public virtual void GeneratePackage()
        {
            if (!Directory.Exists(_fullPath))
                Directory.CreateDirectory(_fullPath);

            foreach (CompNS.Component component in _components.Values)
                component.GenerateComponent();

            foreach (Package subpackage in _subpackages.Values)
                subpackage.GeneratePackage();
        }
    };
}
