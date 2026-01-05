namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql.MsSql;

public interface IMsSqlConstraint
{
    IList<string> GenerateConstraints();
}
