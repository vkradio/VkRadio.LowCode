using VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;

namespace VkRadio.LowCode.AppGenerator.Targets;

public abstract class SqlBaseTarget : ArtefactGenerationTarget
{
    public SqlBaseTarget(Guid id, string outputPath, DbParams dbParams)
        : base(id, outputPath ?? throw new ArgumentNullException(nameof(outputPath)))
    {
        DbParams = dbParams;
    }

    public DbParams DbParams { get; private set; }

    //protected override void InitConcrete(XElement xelTarget)
    //{
    //    var xel = xelTarget.Element("DevelopmentDbParams");

    //    if (xel is not null)
    //    {
    //        DbParams = DbParams.ReadFromXElement(xel);
    //    }
    //}
}
