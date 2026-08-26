namespace VkRadio.LowCode.AppGen.Domain.Names;

/// <summary>
/// Named entity (having names in different languages)
/// </summary>
public interface INamed
{
    /// <summary>
    /// Names in different languages
    /// </summary>
    IDictionary<NaturalLanguageEnum, string> Names { get; }
}
