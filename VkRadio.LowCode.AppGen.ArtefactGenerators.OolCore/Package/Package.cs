using CompNS = VkRadio.LowCode.AppGen.ArtefactGenerators.OolCore.Component;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.OolCore.Package;

/// <summary>
/// Source code package
/// </summary>
public abstract class Package
{
    protected Package? _parentPackage;
    protected Dictionary<string, Package> _subpackages = [];
    protected string _fullPath;
    protected string _name;
    protected Dictionary<string, CompNS.Component> _components = [];

    public Package()
    {
    }

    public Package(Package parentPackage, string name)
    {
        _parentPackage = parentPackage;
        Name = name;
    }

    /// <summary>
    /// Parent package (can be missing, if the current package is a root-level package)
    /// </summary>
    public Package? ParentPackage => _parentPackage;

    /// <summary>
    /// Subpackages
    /// </summary>
    public IDictionary<string, Package> Subpackages => _subpackages;

    /// <summary>
    /// Full path to a package directory
    /// </summary>
    public string FullPath => _fullPath;

    /// <summary>
    /// Package name
    /// </summary>
    public string Name
    {
        get => _name;
        
        set
        {
            _name = value;
            _fullPath = Path.Combine(_parentPackage!.FullPath, _name);
        }
    }

    /// <summary>
    /// Inner components (source code files)
    /// </summary>
    public IDictionary<string, CompNS.Component> Components => _components;

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
