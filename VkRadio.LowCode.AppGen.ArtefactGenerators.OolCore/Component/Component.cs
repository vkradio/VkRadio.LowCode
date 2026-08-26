using VkRadio.LowCode.AppGen.Domain;
using ClassNS = VkRadio.LowCode.AppGen.ArtefactGenerators.OolCore.Class;
using PackNS = VkRadio.LowCode.AppGen.ArtefactGenerators.OolCore.Package;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.OolCore.Component;

/// <summary>
/// Component (source code file)
/// </summary>
public abstract class Component
{
    protected string _name;

    /// <summary>
    /// Owning package
    /// </summary>
    public PackNS.Package Package { get; set; }

    /// <summary>
    /// Full path to a component file
    /// </summary>
    public string FullPath { get; protected set; }

    /// <summary>
    /// Do not owerwrite component, if it already exists (helpful for manually edited files)
    /// </summary>
    public bool DoNotOverwriteIfAlreadyExists { get; protected set; }

    /// <summary>
    /// Component (file) name
    /// </summary>
    public string Name
    {
        get => _name;
        
        set
        {
            _name = value;
            FullPath = Path.Combine(Package.FullPath, _name);
        }
    }

    /// <summary>
    /// Component name without file extension
    /// </summary>
    public string NameWOExtension
    {
        get
        {
            var nameParts = _name.Split(['.']);
            var result = string.Empty;

            for (var i = 0; i < nameParts.Length - 1; i++)
            {
                if (result != string.Empty)
                {
                    result += ".";
                }

                result += nameParts[i];
            }

            return result;
        }
    }

    /// <summary>
    /// Containg classes
    /// </summary>
    public Dictionary<string, ClassNS.Class> Classes { get; private set; } = [];

    /// <summary>
    /// Corresponding DOT definition
    /// </summary>
    public EntityDefinition EntityDefinition { get; set; }

    public abstract void GenerateComponent();
}
