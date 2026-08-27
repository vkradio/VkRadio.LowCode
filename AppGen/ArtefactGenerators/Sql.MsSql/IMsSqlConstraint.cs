namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.MsSql;

public interface IMsSqlConstraint
{
    IList<string> GenerateConstraints();
}
