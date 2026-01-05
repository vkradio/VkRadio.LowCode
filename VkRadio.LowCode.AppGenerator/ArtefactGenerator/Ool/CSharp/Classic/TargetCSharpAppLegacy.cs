using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic;

public class TargetCSharpAppLegacy : ArtefactGenerationTarget
{
    public Guid Id { get; private set; }

    public string DotNetFramework { get; private set; }

    public string OrmLibProjectDir { get; private set; }

    public string VersionControlWcRoot { get; private set; }

    /// <summary>
    /// Ormlib project name without any .csproj extension and path (as displayed in VS solution tree)
    /// </summary>
    public string OrmLibProjectName { get; private set; }

    public TargetSql? TargetSql
    {
        get => Subtargets
            .Where(t => t is TargetSql)
            .Select(t => (TargetSql)t)
            .SingleOrDefault();
    }

    //protected override void AfterDeserializeConcreteRoot()
    //{
    //    if (!Path.IsPathRooted(OrmLibProjectDir))
    //    {
    //        OrmLibProjectDir = Path.Combine(Project.ProjectBasePath, OrmLibProjectDir);
    //    }

    //    OrmLibProjectName = "orm_" + DotNetFramework;

    //    if (!File.Exists(GetOrmLibProjectFilePath()))
    //    {
    //        throw new GeneratorException($"File not exists: {GetOrmLibProjectFilePath()}");
    //    }

    //    if (!Path.IsPathRooted(VersionControlWcRoot))
    //    {
    //        VersionControlWcRoot = Path.Combine(Project.ProjectBasePath, VersionControlWcRoot);
    //    }
    //}

    //protected override void CreateGenerator() => Generator = new DummyGenerator();

    public string GetOrmLibProjectFilePath() => Path.Combine(OrmLibProjectDir, OrmLibProjectName + ".csproj");

    public static readonly string C_ORMLIB_PROJECT_GUID_STRING = "BC2581BB-BC55-4E13-9AED-69CE482E092D";
}
