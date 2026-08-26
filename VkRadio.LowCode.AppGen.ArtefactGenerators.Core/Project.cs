using Ardalis.GuardClauses;
using VkRadio.LowCode.AppGenerator.MetaModel;
using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.ArtefactGenerators.Core;

/// <summary>
/// Artefact generation project description
/// </summary>
public class Project : IUnique
{
    #region Internals
    /// <summary>
    /// Append message about generation
    /// </summary>
    /// <param name="oldMessage"></param>
    /// <param name="newMessage"></param>
    /// <returns>Newline separated messages</returns>
    private static string AppendMessage(string? oldMessage, string newMessage)
    {
        if (oldMessage is null)
        {
            return newMessage;
        }
        else
        {
            return oldMessage + Environment.NewLine + newMessage;
        }
    }
    #endregion

    /// <summary>
    /// Unique identifier of a project
    /// </summary>
    public Guid Id { get; private set; }
    /// <summary>
    /// Project name
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// MetaModel
    /// </summary>
    public DomainModel MetaModel { get; private set; } = default!;
    /// <summary>
    /// Targets of artefact generation
    /// </summary>
    public List<Target> Targets { get; private set; }
    /// <summary>
    /// Project root path
    /// </summary>
    public string ProjectRootPath { get; private set; }
    /// <summary>
    /// Root folder of an SVN repository
    /// </summary>
    public string? SvnWcRootPath { get; private set; }
    /// <summary>
    /// Timezone of an executable code
    /// </summary>
    //public TimeZoneInfo TimeZone { get; private set; }

    public Project(
        Guid id,
        string name,
        string projectRootPath,
        string? svnWcRootPath,
        IEnumerable<Target> targets
    )
    {
        Id = id;
        Name = Guard.Against.NullOrEmpty(name, nameof(name));
        ProjectRootPath = projectRootPath;
        SvnWcRootPath = svnWcRootPath;

        Targets = [.. targets ?? []];
        Targets.ForEach(x => x.WireToProject(this));
    }

    public async Task InitializeAfterLoad()
    {
        Guard.Against.Null(Targets);

        var initTasks = Targets.Select(x => x.InitializeAfterLoad());

        await Task.WhenAll(initTasks);
    }
}
