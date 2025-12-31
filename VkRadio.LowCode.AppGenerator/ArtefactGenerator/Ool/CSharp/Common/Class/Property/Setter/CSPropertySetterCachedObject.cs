using System.Collections.Generic;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Setter
{
    public class CSPropertySetterCachedObject: CSPropertySetter
    {
        public CSPropertySetterCachedObject(CSProperty in_property)
            : base(in_property) {}

        protected override string[] GenerateLinesSetValue()
        {
            var text = new List<string>();
            text.Add($"{Property.NameFieldCorresponding} = value;");
            text.Add(string.Format("{0}Id = {0} != null ? (Guid?){0}.Id : null;", Property.NameFieldCorresponding));
            return text.ToArray();
        }
    };
}
