using System.Text.RegularExpressions;
using System.Xml.Linq;

using MM = VkRadio.LowCode.AppGenerator.MetaModel;
using VkRadio.LowCode.AppGenerator.MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator;

/// <summary>
/// Artefact generation project description
/// </summary>
public class ArtefactGenerationProject : IUnique
{
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
    public MM.MetaModel MetaModel { get; private set; }
    /// <summary>
    /// Targets of artefact generation
    /// </summary>
    public IList<ArtefactGenerationTarget> ArtefactGenerationTargets { get; private set; } = new List<ArtefactGenerationTarget>();
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
    public TimeZoneInfo TimeZone { get; private set; }

    /// <summary>
    /// Append message about generation
    /// </summary>
    /// <param name="oldMessage"></param>
    /// <param name="newMessage"></param>
    /// <returns>Newline separated messages</returns>
    private string AppendMessage(string? oldMessage, string newMessage)
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

    /// <summary>
    /// Load an artefact generation project from a file
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static ArtefactGenerationProject Load(string filePath)
    {
        var xelRoot = XElement.Load(filePath);

        if (xelRoot.Name != "ArtefactGenerationProject")
        {
            throw new ApplicationException("XML root element is not ArtefactGenerationProject.");
        }

        var id = new Guid(xelRoot.Element("Id")!.Value);
        var name = xelRoot.Element("Name")!.Value;
        var metaModelFilePath = xelRoot.Element("MetaModelFilePath")!.Value;
        var projectRootPath = Path.GetDirectoryName(filePath)!;
        metaModelFilePath = Path.Combine(projectRootPath, metaModelFilePath);
        var xelSvnWcRootRelativeDir = xelRoot.Element("SvnWcRootRelativeDir");

        if (xelSvnWcRootRelativeDir is null || string.IsNullOrWhiteSpace(xelSvnWcRootRelativeDir.Value))
        {
            // TODO: Remove it, we mostly use git now
            throw new ApplicationException("In the root <ArtefactGenerationProject> there is not value of <SvnWcRootRelativeDir>.");
        }

        var svnWcRootDirRelativePath = xelSvnWcRootRelativeDir.Value.Trim();

        #region Set the timezone of an executing code
        // Full list of timezones:
        // http://unicode.org/repos/cldr-tmp/trunk/diff/supplemental/territory_containment_un_m_49.html
        var xelTz = xelRoot.Element("TimeZone");
        TimeZoneInfo? tz = null;
        if (xelTz is null || xelTz.Value.Contains("00:00"))
        {
            tz = TimeZoneInfo.Utc;
        }
        else
        {
            var r = new Regex(@"^\((GMT|UTC)(?<gmtVal>.+)\).+$");

            var sysTimeZones = TimeZoneInfo.GetSystemTimeZones();

            foreach (var stz in sysTimeZones)
            {
                var m = r.Match(stz.DisplayName);

                if (m.Success)
                {
                    var offsetValueStr = m.Groups["gmtVal"].Value;

                    if (offsetValueStr == xelTz.Value)
                    {
                        tz = stz;
                        break;
                    }
                }
            }

            if (tz is null)
            {
                throw new ApplicationException(string.Format(@"Time zone {0} not found in system time zone collection. (See HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion\Time Zones\Display.)", xelTz.Value));
            }
        }
        #endregion

        var metaModel = MM.MetaModel.Load(metaModelFilePath);

        var project = new ArtefactGenerationProject
        {
            Id = id,
            Name = name,
            MetaModel = metaModel,
            ProjectRootPath = projectRootPath,
            TimeZone = tz,
            SvnWcRootPath = Path.Combine(projectRootPath, svnWcRootDirRelativePath)
        };

        var xelTargets = xelRoot.Element("ArtefactGenerationTargets")!;

        foreach (var xel in xelTargets.Elements("Target"))
        {
            project.ArtefactGenerationTargets.Add(ArtefactGenerationTarget.LoadFromXElement(project, xel));
        }

        // Deferred linking of target dependencies
        foreach (var tgt in project.ArtefactGenerationTargets)
        {
            tgt.DeferredLinkDependencies(project.ArtefactGenerationTargets);
        }

        return project;
    }

    /// <summary>
    /// Generate all artefacts of a project
    /// </summary>
    /// <returns>null for successful generation, or an error message otherwise</returns>
    public string? GenerateArtefacts()
    {
        string? message = null;

        // TODO: For not ignore dependencies and execute targets in their defined order
        foreach (var target in ArtefactGenerationTargets)
        {
            var prevTargetsFailed = false;

            foreach (var depTarget in target.DependsOn.Values)
            {
                if (!depTarget.GenerateSuccess)
                {
                    prevTargetsFailed = true;
                    break;
                }
            }

            if (prevTargetsFailed)
            {
                message = AppendMessage(message, "Next targets won't be generated.");
                break;
            }

            var targetMessage = target.GenerateArtefacts();

            if (targetMessage is not null)
            {
                message = AppendMessage(message, targetMessage);
            }
        }

        return message;
    }
}
