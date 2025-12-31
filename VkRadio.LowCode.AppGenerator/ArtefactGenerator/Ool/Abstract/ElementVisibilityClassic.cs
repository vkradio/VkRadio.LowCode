using System;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract
{
    public class ElementVisibilityClassic: ElementVisibilityAbstract
    {
        public override string ToString()
        {
            string result;

            switch (_value)
            {
                case ElementVisibilityEnum.Private:
                    result = "private";
                    break;
                case ElementVisibilityEnum.Protected:
                    result = "protected";
                    break;
                case ElementVisibilityEnum.Public:
                    result = "public";
                    break;
                default:
                    throw new ApplicationException(string.Format("ElementVisibilityEnum value not supported: {0}.", _value.ToString()));
            }

            return result;
        }
        
        public static ElementVisibilityClassic Private { get { return new ElementVisibilityClassic() { Value = ElementVisibilityEnum.Private }; } }
        public static ElementVisibilityClassic Protected { get { return new ElementVisibilityClassic() { Value = ElementVisibilityEnum.Protected }; } }
        public static ElementVisibilityClassic Public { get { return new ElementVisibilityClassic() { Value = ElementVisibilityEnum.Public }; } }
    };
}
