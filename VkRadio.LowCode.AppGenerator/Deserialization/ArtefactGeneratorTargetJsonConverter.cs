using Newtonsoft.Json.Linq;
using System;

using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular;
using ArtefactGenerationProject.ArtefactGenerator.Sql;

namespace ArtefactGenerationProject.Deserialization
{
    public class ArtefactGeneratorTargetJsonConverter : JsonCreateConverter<ArtefactGenerationTargetJson>
    {
        public override bool CanConvert(Type objectType) => typeof(ArtefactGenerationTargetJson).IsAssignableFrom(objectType);

        protected override ArtefactGenerationTargetJson Create(Type objectType, JObject jObject)
        {
            ArtefactGenerationTargetJson result;
            var typeString = jObject["type"]?.Value<string>();
            switch (typeString)
            {
                case "C# app legacy":
                    result = new TargetCSharpAppLegacy();
                    break;
                case "C# app":
                    result = new TargetCSharpApp();
                    break;
                case "C# solution legacy":
                    result = new TargetCSharpSolutionLegacy();
                    break;
                case "C# solution":
                    result = new TargetCSharpSolution();
                    break;
                case "MS SQL":
                    result = new TargetSqlJson();
                    break;
                default:
                    throw new ApplicationException($"Unspecified or unsupported target type: {typeString}.");
            }
            return result;
        }
    };
}
