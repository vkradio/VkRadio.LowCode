using System.Xml.Linq;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    public class TargetSql : ArtefactGenerationTarget
    {
        public DbParams DbParams { get; private set; }

        protected override void InitConcrete(XElement in_xelTarget)
        {
            var xel = in_xelTarget.Element("DevelopmentDbParams");
            if (xel != null)
                DbParams = DbParams.ReadFromXElement(xel);
        }

        public new ArtefactGeneratorSql Generator => (ArtefactGeneratorSql)base.Generator;
    }
}
