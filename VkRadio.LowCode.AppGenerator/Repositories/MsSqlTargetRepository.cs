using System.Xml.Linq;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql.MsSql;
using VkRadio.LowCode.AppGenerator.Targets;

namespace VkRadio.LowCode.AppGenerator.Repositories;

public class MsSqlTargetRepository : BaseTargetRepository
{
    public override ArtefactTypeCodeEnum ArtefactType => ArtefactTypeCodeEnum.MsSql;

    public override Task<ArtefactGenerationTarget> LoadTarget(XElement containingXel)
    {
        var (id, artefactType, outputPath) = LoadBaseValues(containingXel);

        if (artefactType != ArtefactType)
        {
            throw new ApplicationException($"Target {id} is not of {ArtefactType} type");
        }

        if (string.IsNullOrWhiteSpace(outputPath))
        {
            throw new ApplicationException($"OutputPath is required for Target Id {id}");
        }

        string? devDbHost = null;
        string? devDbName = null;
        bool? devDbOsSecurityUseCurrentUser = null;

        var devDbParamsXel = containingXel.Element("DevelopmentDbParams");

        if (devDbParamsXel is not null)
        {
            devDbHost = devDbParamsXel.Element("Host")?.Value;
            devDbName = devDbParamsXel.Element("DbName")?.Value;

            var xelOsSecurityUseCurrentUser = devDbParamsXel.Element("OsSecurityUseCurrentUser")?.Value;

            if (!string.IsNullOrWhiteSpace(xelOsSecurityUseCurrentUser))
            {
                if (bool.TryParse(xelOsSecurityUseCurrentUser, out var tmpResult))
                {
                    devDbOsSecurityUseCurrentUser = tmpResult;
                }
            }
        }

        var target = new MsSqlTarget(id, outputPath, devDbHost, devDbName, devDbOsSecurityUseCurrentUser);

        return Task.FromResult((ArtefactGenerationTarget)target);
    }
}
