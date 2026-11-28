namespace VkRadio.LowCode.AppGen.Domain.Names;

/// <summary>
/// Named entity (having names in different languages)
/// </summary>
public interface INamed
{
    /// <summary>
    /// Names in different languages
    /// </summary>
    IReadOnlyDictionary<NaturalLanguageEnum, string> Names { get; }
}
