using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

using ArtefactGenerationProject.ArtefactGenerator.Sql;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic
{
    /**
     * NOTE: In JSON file all paths are relative to project root. Directory, containing
     * project JSON file, is the root of project. After deserialization, all respective
     * relative paths converted to absolute paths.
     */

    //[JsonConverter(typeof(ArtefactGeneratorTargetJsonConverter))]
    public class TargetCSharpAppLegacy : ArtefactGenerationTargetJson
    {
        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public string DotNetFramework { get; private set; }

        [JsonProperty]
        public string OrmLibProjectDir { get; private set; }

        [JsonProperty]
        public string VersionControlWcRoot { get; private set; }

        /// <summary>
        /// Ormlib project name without any .csproj extension and path (as displayed in VS solution tree)
        /// </summary>
        [JsonIgnore]
        public string OrmLibProjectName { get; private set; }

        [JsonIgnore]
        public TargetSqlJson TargetSql
        {
            get => Subtargets
                .Where(t => t is TargetSqlJson)
                .Select(t => (TargetSqlJson)t)
                .SingleOrDefault();
        }

        protected override void AfterDeserializeConcreteRoot()
        {
            if (!Path.IsPathRooted(OrmLibProjectDir))
                OrmLibProjectDir = Path.Combine(Project.ProjectBasePath, OrmLibProjectDir);
            OrmLibProjectName = "orm_" + DotNetFramework;
            if (!File.Exists(GetOrmLibProjectFilePath()))
                throw new GeneratorException($"File not exists: {GetOrmLibProjectFilePath()}");
            if (!Path.IsPathRooted(VersionControlWcRoot))
                VersionControlWcRoot = Path.Combine(Project.ProjectBasePath, VersionControlWcRoot);
        }

        protected override void CreateGenerator() => Generator = new DummyGenerator();

        public string GetOrmLibProjectFilePath() =>
            Path.Combine(OrmLibProjectDir, OrmLibProjectName + ".csproj");

        public static readonly string C_ORMLIB_PROJECT_GUID_STRING = "BC2581BB-BC55-4E13-9AED-69CE482E092D";
    };
}
