using System.Reflection;
using PackNS = VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Package;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Component;

public class ComponentFromEmbeddedResource : Component
{
    string _resourceName;
    string _namespaceName;

    public ComponentFromEmbeddedResource(PackNS.Package package, string name, string namespaceName) : this(package, name, name, namespaceName)
    {
    }

    public ComponentFromEmbeddedResource(PackNS.Package package, string name, string resourceName, string namespaceName)
    {
        Package = package;
        Name = name;
        _resourceName = resourceName;
        _namespaceName = namespaceName;
    }

    public override void GenerateComponent()
    {
        if (File.Exists(FullPath))
        {
            File.Delete(FullPath);
        }

        var assembly = Assembly.GetExecutingAssembly();

        using var src = assembly.GetManifestResourceStream($"{_namespaceName}.{_resourceName}")!;
        using var dest = File.Create(FullPath);
        src.CopyTo(dest);
    }
}
