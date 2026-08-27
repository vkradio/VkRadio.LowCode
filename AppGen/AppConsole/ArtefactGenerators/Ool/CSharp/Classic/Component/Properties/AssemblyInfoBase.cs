using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Root;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Component.Properties;

public class AssemblyInfoBase : AssemblyInfoAbstract
{
    public AssemblyInfoBase(PropertiesPackageBase package)
        : base(package)
    {
        var cSharpAppTarget = package.ParentPackage.ParentPackage.ArtefactGenerationTarget.Parent;

        _predefinedCode.Add($"using System.Reflection;");
        _predefinedCode.Add($"using System.Runtime.CompilerServices;");
        _predefinedCode.Add($"using System.Runtime.InteropServices;");
        _predefinedCode.Add(string.Empty);
        _predefinedCode.Add($"// General Information about an assembly is controlled through the following ");
        _predefinedCode.Add($"// set of attributes. Change these attribute values to modify the information");
        _predefinedCode.Add($"// associated with an assembly.");
        _predefinedCode.Add($"[assembly: AssemblyTitle(\"{package.ParentPackage.RootNamespace}\")]");
        _predefinedCode.Add($"[assembly: AssemblyDescription(\"\")]");
        _predefinedCode.Add($"[assembly: AssemblyConfiguration(\"\")]");
        _predefinedCode.Add($"[assembly: AssemblyCompany(\"\")]");
        _predefinedCode.Add($"[assembly: AssemblyProduct(\"{package.ParentPackage.RootNamespace}\")]");
        _predefinedCode.Add($"[assembly: AssemblyCopyright(\"Copyright ©  {DateTime.Today.Year}\")]");
        _predefinedCode.Add($"[assembly: AssemblyTrademark(\"\")]");
        _predefinedCode.Add($"[assembly: AssemblyCulture(\"\")]");
        _predefinedCode.Add(string.Empty);
        _predefinedCode.Add($"// Setting ComVisible to false makes the types in this assembly not visible ");
        _predefinedCode.Add($"// to COM components.  If you need to access a type in this assembly from ");
        _predefinedCode.Add($"// COM, set the ComVisible attribute to true on that type.");
        _predefinedCode.Add($"[assembly: ComVisible(false)]");
        _predefinedCode.Add(string.Empty);
        _predefinedCode.Add($"// The following GUID is for the ID of the typelib if this project is exposed to COM");
        _predefinedCode.Add($"[assembly: Guid(\"{cSharpAppTarget.Id}\")]");
        _predefinedCode.Add(string.Empty);
        _predefinedCode.Add($"// Version information for an assembly consists of the following four values:");
        _predefinedCode.Add($"//");
        _predefinedCode.Add($"//      Major Version");
        _predefinedCode.Add($"//      Minor Version ");
        _predefinedCode.Add($"//      Build Number");
        _predefinedCode.Add($"//      Revision");
        _predefinedCode.Add($"//");
        _predefinedCode.Add($"// You can specify all the values or you can default the Build and Revision Numbers ");
        _predefinedCode.Add($"// by using the '*' as shown below:");
        _predefinedCode.Add($"// [assembly: AssemblyVersion(\"1.0.*\")]");
        _predefinedCode.Add($"[assembly: AssemblyVersion(\"1.0.0.0\")]");
        _predefinedCode.Add($"[assembly: AssemblyFileVersion(\"1.0.0.0\")]");
    }
}
