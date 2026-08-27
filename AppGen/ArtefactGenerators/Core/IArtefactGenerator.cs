namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Core;

public interface IArtefactGenerator
{
    Task InitializeArtefactModel();

    Task GenerateArtefacts();
}
