using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Property;

public class CSPropertyCollectionReader : CSPropertyPredefined
{
    public CSPropertyCollectionReader(string name, string predefinedValue, XmlComment? docComment = null)
        : base(name, predefinedValue, docComment)
    {
        // Здесь надо написать приватный метод, который будет брать propertyDefinition и на его основе форировать условный Name, а главное - predefined код
    }
}
