namespace VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.MsSql.Internals;

public interface IMsSqlConstraint
{
    IList<string> GenerateConstraints();
}
