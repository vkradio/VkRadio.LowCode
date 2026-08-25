using VkRadio.LowCode.ArtefactGenerators.Core;

namespace VkRadio.LowCode.ArtefactGenerators.SqlCore;

public abstract class ArtefactGeneratorSql : IArtefactGenerator
{
    public Task GenerateArtefacts()
    {
        throw new NotImplementedException();
    }

    public Task InitializeArtefactModel()
    {
        throw new NotImplementedException();
    }
}
