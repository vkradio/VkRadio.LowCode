using CompNS = VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Component;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Package;

/// <summary>
/// Source code package
/// </summary>
public abstract class Package
{
    protected Package? _parentPackage;
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
    /// Parent package (can be missing, if the current package is a root-level package)
    /// </summary>
    public Package? ParentPackage { get { return _parentPackage; } }
    /// <summary>
    /// Subpackages
    /// </summary>
    public IDictionary<string, Package> Subpackages { get { return _subpackages; } } 
    /// <summary>
    /// Full path to a package directory
    /// </summary>
    public string FullPath { get { return _fullPath; } }
    /// <summary>
    /// Package name
    /// </summary>
    public string Name { get { return _name; } set { _name = value; _fullPath = Path.Combine(_parentPackage.FullPath, _name); } }
    /// <summary>
    /// Inner components (source code files)
    /// </summary>
    public IDictionary<string, CompNS.Component> Components { get { return _components; } }

    public virtual void GeneratePackage()
    {
        if (!Directory.Exists(_fullPath))
        {
            Directory.CreateDirectory(_fullPath);
        }

        foreach (var component in _components.Values)
        {
            component.GenerateComponent();
        }

        foreach (var subpackage in _subpackages.Values)
        {
            subpackage.GeneratePackage();
        }
    }
}
