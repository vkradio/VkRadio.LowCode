using System.Collections.Generic;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Setter
{
    public class CSPropertySetterCachedObjectId: CSPropertySetter
    {
        public CSPropertySetterCachedObjectId(CSProperty in_property) : base(in_property) {}

        protected override string[] GenerateLinesSetValue()
        {
            var text = new List<string>();
            text.Add($"{Property.NameFieldCorresponding} = value;");
            var objectFieldName = Property.NameFieldCorresponding.Substring(0, Property.NameFieldCorresponding.Length - 2);
            text.Add($"{objectFieldName} = null;");
            return text.ToArray();
        }
    };
}
