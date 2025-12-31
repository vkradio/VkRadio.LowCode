using System.Reflection;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Component;

public class ComponentFromEmbeddedResource : Component
{
    string _resourceName;
    string _namespace;

    public ComponentFromEmbeddedResource(Package.Package in_package, string in_name, string in_namespace) : this(in_package, in_name, in_name, in_namespace) { }
    public ComponentFromEmbeddedResource(Package.Package in_package, string in_name, string in_resourceName, string in_namespace)
    {
        Package = in_package;
        Name = in_name;
        _resourceName = in_resourceName;
        _namespace = in_namespace;
    }

    public override void GenerateComponent()
    {
        if (File.Exists(FullPath))
        {
            File.Delete(FullPath);
        }

        var assembly = Assembly.GetExecutingAssembly();

        using var src = assembly.GetManifestResourceStream($"{_namespace}.{_resourceName}")!;
        using var dest = File.Create(FullPath);
        src.CopyTo(dest);
    }
}
