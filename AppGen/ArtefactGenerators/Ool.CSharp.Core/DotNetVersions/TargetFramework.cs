using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.DotNetVersions;

public static class TargetFramework
{
    public const string C_TFM_47 = "net47";

    public const string C_TFM_471 = "net471";

    public const string C_TFM_48 = "net48";

    public const string C_TFM_481 = "net481";

    public static string VersionToTfm(string version)
    {
        var parsedVersion = new Version(version);

        if (parsedVersion.Major != 4)
        {
            throw new GeneratorException($"Not supported major .NET Framework version: {parsedVersion.Major}. Supported only version 4.7 and higher");
        }

        if (parsedVersion.Minor < 7)
        {
            throw new GeneratorException($"Not supported minor .NET Framework version: {parsedVersion.Minor}. Supported only version 4.7 and higher");
        }

        if (parsedVersion.Minor > 8)
        {
            throw new GeneratorException($"Not supported minor .NET Framework version: {parsedVersion.Minor}. Supported only versions up to 4.8.1");
        }

        if (parsedVersion.Build != 0 && parsedVersion.Build != 1)
        {
            throw new GeneratorException($"Not supported .NET Framework build version: {parsedVersion.Build}");
        }

        if (parsedVersion.Minor == 7)
        {
            return parsedVersion.Build == 0
                ? C_TFM_47
                : C_TFM_471;
        }
        else
        {
            return parsedVersion.Build == 0
                ? C_TFM_48
                : C_TFM_481;
        }
    }
}
