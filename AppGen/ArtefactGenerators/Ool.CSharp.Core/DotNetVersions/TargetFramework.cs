using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.DotNetVersions;

public static class TargetFramework
{
    public const string C_TFM_47 = "net47";

    public const string C_TFM_471 = "net471";

    public const string C_TFM_48 = "net48";

    public const string C_TFM_481 = "net481";

    public const string C_TFM_WINDOWS = "net{0}.0-windows";

    public static bool IsModern(this Version version) => version.Major > 4;

    public static string VersionToTfm(Version version)
    {
        if (version.Major != 4 && version.Major < 8)
        {
            throw new GeneratorException($"Not supported major .NET version: {version.Major}. Supported only version Framework 4.7 and higher, or .NET 8 and higher");
        }

        if (version.IsModern())
        {
            if (version.Minor != 0 || (version.Build != 0 && version.Build != -1))
            {
                throw new GeneratorException($"Not supported .NET version: {version}. No minor or build version other that 0 are supported.");
            }

            return string.Format(C_TFM_WINDOWS, version.Major);
        }
        else
        {
            if (version.Minor < 7)
            {
                throw new GeneratorException($"Not supported minor .NET Framework version: {version.Minor}. Supported only version 4.7 and higher");
            }

            if (version.Minor > 8)
            {
                throw new GeneratorException($"Not supported minor .NET Framework version: {version.Minor}. Supported only versions up to 4.8.1");
            }

            if (version.Build != 0 && version.Build != 1)
            {
                throw new GeneratorException($"Not supported .NET Framework build version: {version.Build}");
            }

            if (version.Minor == 7)
            {
                return version.Build == 0
                    ? C_TFM_47
                    : C_TFM_471;
            }
            else
            {
                return version.Build == 0
                    ? C_TFM_48
                    : C_TFM_481;
            }
        }
    }
}
