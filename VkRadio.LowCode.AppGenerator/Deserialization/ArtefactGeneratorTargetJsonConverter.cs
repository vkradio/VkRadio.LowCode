using Newtonsoft.Json.Linq;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Modular;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql;

namespace VkRadio.LowCode.AppGenerator.Deserialization;

public class ArtefactGeneratorTargetJsonConverter //: JsonCreateConverter<ArtefactGenerationTargetJson>
{
    //public override bool CanConvert(Type objectType) => typeof(ArtefactGenerationTargetJson).IsAssignableFrom(objectType);

    //protected override ArtefactGenerationTargetJson Create(Type objectType, JObject jObject)
    //{
    //    ArtefactGenerationTargetJson result;
    //    var typeString = jObject["type"]?.Value<string>();
    //    switch (typeString)
    //    {
    //        case "C# app legacy":
    //            result = new TargetCSharpAppLegacy();
    //            break;
    //        case "C# app":
    //            result = new TargetCSharpApp();
    //            break;
    //        case "C# solution legacy":
    //            result = new TargetCSharpSolutionLegacy();
    //            break;
    //        case "C# solution":
    //            result = new TargetCSharpSolution();
    //            break;
    //        case "MS SQL":
    //            result = new TargetSqlJson();
    //            break;
    //        default:
    //            throw new ApplicationException($"Unspecified or unsupported target type: {typeString}.");
    //    }
    //    return result;
    //}
}
