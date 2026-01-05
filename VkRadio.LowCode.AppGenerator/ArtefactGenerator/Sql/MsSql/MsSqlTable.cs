namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql.MsSql;

public class MsSqlTable : Table
{
    public MsSqlTable(string name, SchemaDeploymentScript schemaDeploymentScript)
        : base(name, schemaDeploymentScript)
    {
        _quoteSymbol = "\"";
    }

    public IList<string> GenerateConstraints()
    {
        var result = new List<string>();

        //if (_primaryKey != null)
        //    result.AddRange(((MsSqlPKSingle)_primaryKey).GenerateConstraints());
        foreach (var field in AllFields)
        {
            var constraintField = (IMsSqlConstraint)field;
            result.AddRange(constraintField.GenerateConstraints());
        }

        if (result.Count != 0)
        {
            result.Add("go");
        }

        return result;
    }
}
