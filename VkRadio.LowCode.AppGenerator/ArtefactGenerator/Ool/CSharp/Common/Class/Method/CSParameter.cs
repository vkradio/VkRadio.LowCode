using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Method
{
    public class CSParameter : ParameterTyped
    {
        public override string ToString()
        {
            var result = Type + " " + Name;
            if (!string.IsNullOrEmpty(Value))
                result += (" = " + Value);
            return result;
        }
    };
}
