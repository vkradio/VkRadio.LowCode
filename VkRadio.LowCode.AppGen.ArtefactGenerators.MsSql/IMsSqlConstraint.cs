namespace VkRadio.LowCode.AppGen.ArtefactGenerators.MsSql;

public interface IMsSqlConstraint
{
    IList<string> GenerateConstraints();
}
