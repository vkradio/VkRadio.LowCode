using System.Xml.Linq;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql;

public class TargetSql : ArtefactGenerationTarget
{
    public DbParams DbParams { get; private set; }

    protected override void InitConcrete(XElement xelTarget)
    {
        var xel = xelTarget.Element("DevelopmentDbParams");

        if (xel is not null)
        {
            DbParams = DbParams.ReadFromXElement(xel);
        }
    }

    public new ArtefactGeneratorSql Generator => (ArtefactGeneratorSql)base.Generator;
}
