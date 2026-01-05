using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator.MetaModel.RegisterDefinition;

/// <summary>
/// Definition of abstract register
/// </summary>
public abstract class RegisterDefinition : IUniqueNamed
{
    // TODO: The whole folder with register code was removed, see a previous git revision. The register functionality was never completed.

    public Guid Id => throw new NotImplementedException();

    public IDictionary<HumanLanguageEnum, string> Names => throw new NotImplementedException();
}
