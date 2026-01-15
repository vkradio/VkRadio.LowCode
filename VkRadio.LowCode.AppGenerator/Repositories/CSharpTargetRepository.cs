using System.Xml.Linq;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic;
using VkRadio.LowCode.AppGenerator.Targets;

namespace VkRadio.LowCode.AppGenerator.Repositories;

public class CSharpTargetRepository : BaseTargetRepository
{
    public override ArtefactTypeCodeEnum ArtefactType => ArtefactTypeCodeEnum.CSharp;

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

        var target = new CSharpTarget(id, outputPath);

        return Task.FromResult((ArtefactGenerationTarget)target);
    }
}
