namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property;

public interface ICSProperty
{
    string Name { get; }

    string[] GenerateText();
}
