using System.Collections.Generic;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql.MsSql
{
    public interface IMsSqlConstraint
    {
        IList<string> GenerateConstraints();
    };
}
