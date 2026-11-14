using CompNS = VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Component;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class;

/// <summary>
/// Subclass of data object type class for fast retrieving of predefined objects
/// </summary>
public class CSClassPredefinedDataObject : CSClass
{
    public CSClassPredefinedDataObject(string name, CompNS.Component component, XmlComment? docComment = null, string? inheritsFrom = null)
        : base(name, component, docComment, inheritsFrom)
    {
    }
}
