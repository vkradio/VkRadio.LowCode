using MetaModel.PredefinedDO;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Getter
{
    public class CSPropertyGetterPredefinedObject: CSPropertyGetter
    {
        public CSPropertyGetterPredefinedObject(CSProperty property) : base(property) {}

        public override string[] GenerateText()
        {
            var dotClassName = CSharpHelper.GenerateDOTClassName(CorrespondingPDO.DOTDefinition);
            return new string[] { $"get => ({dotClassName})StorageRegistry.Instance.{dotClassName}Storage.Restore({IdConstName});" };
        }

        public PredefinedDO CorrespondingPDO { get; set; }
        public string IdConstName { get; set; }
    };
}
