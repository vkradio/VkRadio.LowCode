using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

using ArtefactGenerationProject.ArtefactGenerator.Sql;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Modular
{
    /**
     * NOTE: In JSON file all paths are relative to project root. Directory, containing
     * project JSON file, is the root of project. After deserialization, all respective
     * relative paths converted to absolute paths.
     */

    public class TargetCSharpApp : ArtefactGenerationTargetJson
    {
        [JsonProperty]
        public Guid Id { get; private set; }

        [JsonProperty]
        public string DotNetStandard { get; private set; }

        [JsonProperty]
        public string OrmLibProjectDir { get; private set; }

        [JsonProperty]
        public string VersionControlWcRoot { get; private set; }

        /// <summary>
        /// Ormlib project name without any .csproj extension and path (as displayed in VS solution tree)
        /// </summary>
        [JsonIgnore]
        public string OrmLibProjectNameDb { get; private set; }

        [JsonIgnore]
        public string OrmLibProjectNameDbConcrete { get; private set; }

        [JsonIgnore]
        public string OrmLibProjectNameUtil { get; private set; }

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
            OrmLibProjectNameDb = "orm.Db";
            OrmLibProjectNameDbConcrete = "orm.Db.MsSql";
            OrmLibProjectNameUtil = "orm.Util";
            if (!File.Exists(GetOrmLibProjectFilePathDb()))
                throw new GeneratorException($"File not exists: {GetOrmLibProjectFilePathDb()}");
            if (!File.Exists(GetOrmLibProjectFilePathDbConcrete()))
                throw new GeneratorException($"File not exists: {GetOrmLibProjectFilePathDbConcrete()}");
            if (!File.Exists(GetOrmLibProjectFilePathUtil()))
                throw new GeneratorException($"File not exists: {GetOrmLibProjectFilePathUtil()}");
            if (!Path.IsPathRooted(VersionControlWcRoot))
                VersionControlWcRoot = Path.Combine(Project.ProjectBasePath, VersionControlWcRoot);
        }

        protected override void CreateGenerator() => Generator = new DummyGenerator();

        public string GetOrmLibProjectFilePathDb() =>
            Path.Combine(OrmLibProjectDir, OrmLibProjectNameDb, OrmLibProjectNameDb + ".csproj");
        public string GetOrmLibProjectFilePathDbConcrete() =>
            Path.Combine(OrmLibProjectDir, OrmLibProjectNameDbConcrete, OrmLibProjectNameDbConcrete + ".csproj");
        public string GetOrmLibProjectFilePathUtil() =>
            Path.Combine(OrmLibProjectDir, OrmLibProjectNameUtil, OrmLibProjectNameUtil + ".csproj");

        public static readonly string C_ORMLIB_PROJECT_DB_GUID_STRING = "F689F7B9-05E7-46D4-AD3F-5ABDDF2DF35A";
        public static readonly string C_ORMLIB_PROJECT_DB_CONCRETE_GUID_STRING = "A78F2C8A-B953-4B9E-A277-919BDC795435";
        public static readonly string C_ORMLIB_PROJECT_UTIL_GUID_STRING = "017E8933-4D09-4562-BAC6-17178FC53855";
    };
}
