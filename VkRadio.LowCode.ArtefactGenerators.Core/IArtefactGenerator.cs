namespace VkRadio.LowCode.ArtefactGenerators.Core;

public interface IArtefactGenerator
{
    Task InitializeArtefactModel();

    Task GenerateArtefacts();
}
