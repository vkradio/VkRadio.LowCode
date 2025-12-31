using System;
using System.Collections.Generic;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Property.Getter
{
    public class CSPropertyGetterCachedObject: CSPropertyGetter
    {
        public CSPropertyGetterCachedObject(CSProperty property)
            : base(property, true) {}

        public override string[] GenerateText()
        {
            var text = new List<string>();

            if (SingleLineHint)
            {
                var line = string.Format("get {{ if ({0} == null && {0}Id.HasValue) {0} = ({1})StorageRegistry.Instance.{1}Storage.Restore({0}Id.Value); return {0}; }}",
                    Property.NameFieldCorresponding,
                    Property.Type);
                text.Add(line);
            }
            else
            {
                throw new NotImplementedException("Generating not-single-line getter for CSPropertyGetterCachedObject not implemented.");
            }

            return text.ToArray();
        }
    };
}
