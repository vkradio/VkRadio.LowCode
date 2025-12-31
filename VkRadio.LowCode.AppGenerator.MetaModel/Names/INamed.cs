namespace VkRadio.LowCode.AppGenerator.MetaModel.Names;

/// <summary>
/// Named entity (having names in different languages)
/// </summary>
public interface INamed
{
    /// <summary>
    /// Names in different languages
    /// </summary>
    IDictionary<HumanLanguageEnum, string> Names { get; }
}
