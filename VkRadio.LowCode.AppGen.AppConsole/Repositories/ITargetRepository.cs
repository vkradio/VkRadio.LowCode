using System.Xml.Linq;
using VkRadio.LowCode.AppGenerator.Targets;

namespace VkRadio.LowCode.AppGenerator.Repositories;

public interface ITargetRepository
{
    Task<ArtefactGenerationTarget> LoadTarget(XElement containingXel);

    ArtefactTypeCodeEnum ArtefactType { get; }
}
