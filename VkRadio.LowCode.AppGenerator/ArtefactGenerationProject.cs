using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using mm = MetaModel;
using MetaModel.Names;

namespace VkRadio.LowCode.AppGenerator;

/// <summary>
/// Описание проекта генерации артефактов
/// </summary>
public class ArtefactGenerationProject: IUnique
{
    /// <summary>
    /// Уникальный идентификатор проекта
    /// </summary>
    public Guid Id { get; private set; }
    /// <summary>
    /// Наименование проекта
    /// </summary>
    public string Name { get; private set; }
    /// <summary>
    /// Метамодель
    /// </summary>
    public mm.MetaModel MetaModel { get; private set; }
    /// <summary>
    /// Цели генерирования артефактов проекта
    /// </summary>
    public IList<ArtefactGenerationTarget> ArtefactGenerationTargets { get; private set; } = new List<ArtefactGenerationTarget>();
    /// <summary>
    /// Директория проекта.
    /// </summary>
    public string ProjectRootPath { get; private set; }
    /// <summary>
    /// Корневая директория рабочей копии SVN
    /// </summary>
    public string SvnWcRootPath { get; private set; }
    /// <summary>
    /// Часовой пояс исполняемого кода.
    /// </summary>
    public TimeZoneInfo TimeZone { get; private set; }

    /// <summary>
    /// Добавление сообщения о генерации (используется в методе GenerateArtefacts)
    /// </summary>
    /// <param name="in_oldMessage">Предыдущее накопленное сообщение (может быть null)</param>
    /// <param name="in_newMessage">Сообщение, которое следует добавить</param>
    /// <returns>Сообщения, разделенные переводом строки</returns>
    string AppendMessage(string in_oldMessage, string in_newMessage)
    {
        if (in_oldMessage == null)
            return in_newMessage;
        else
            return in_oldMessage + Environment.NewLine + in_newMessage;
    }

    /// <summary>
    /// Загрузка проекта генерирации артефактов (ПГА) из файла
    /// </summary>
    /// <param name="in_filePath">Путь к файлу проекта</param>
    /// <returns>Проект генерации артефактов</returns>
    public static ArtefactGenerationProject Load(string in_filePath)
    {
        var xelRoot = XElement.Load(in_filePath);
        if (xelRoot.Name != "ArtefactGenerationProject")
            throw new ApplicationException("XML root element is not ArtefactGenerationProject.");

        var id = new Guid(xelRoot.Element("Id").Value);
        var name = xelRoot.Element("Name").Value;
        var metaModelFilePath = xelRoot.Element("MetaModelFilePath").Value;
        var projectRootPath = Path.GetDirectoryName(in_filePath);
        metaModelFilePath = Path.Combine(projectRootPath, metaModelFilePath);
        var xelSvnWcRootRelativeDir = xelRoot.Element("SvnWcRootRelativeDir");
        if (xelSvnWcRootRelativeDir == null || string.IsNullOrWhiteSpace(xelSvnWcRootRelativeDir.Value))
            throw new ApplicationException("В корне файла <ArtefactGenerationProject> не задан элемент <SvnWcRootRelativeDir>.");
        var svnWcRootDirRelativePath = xelSvnWcRootRelativeDir.Value.Trim();

        #region Установка часового пояса исполнения программного кода.
        // Полный список часовых поясов:
        // http://unicode.org/repos/cldr-tmp/trunk/diff/supplemental/territory_containment_un_m_49.html
        var xelTz = xelRoot.Element("TimeZone");
        TimeZoneInfo tz = null;
        if (xelTz == null || xelTz.Value.Contains("00:00"))
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
            if (tz == null)
                throw new ApplicationException(string.Format(@"Time zone {0} not found in system time zone collection. (See HKEY_LOCAL_MACHINE\Software\Microsoft\Windows NT\CurrentVersion\Time Zones\Display.)", xelTz.Value));
        }
        #endregion

        var metaModel = mm.MetaModel.Load(metaModelFilePath);

        var project = new ArtefactGenerationProject
        {
            Id = id,
            Name = name,
            MetaModel = metaModel,
            ProjectRootPath = projectRootPath,
            TimeZone = tz,
            SvnWcRootPath = Path.Combine(projectRootPath, svnWcRootDirRelativePath)
        };

        var xelTargets = xelRoot.Element("ArtefactGenerationTargets");
        foreach (var xel in xelTargets.Elements("Target"))
            project.ArtefactGenerationTargets.Add(ArtefactGenerationTarget.LoadFromXElement(project, xel));

        // Отложенное связывание зависимостей целей.
        foreach (var tgt in project.ArtefactGenerationTargets)
            tgt.DeferredLinkDependencies(project.ArtefactGenerationTargets);

        return project;
    }

    /// <summary>
    /// Генерирование всех артефактов проекта
    /// </summary>
    /// <returns>null, если генерация успешна, или сообщение о проблемах</returns>
    public string GenerateArtefacts()
    {
        string message = null;

        // TODO: Пока не учитываем зависимости - считаем, что в файле описания проекта
        // цели записаны в нужном порядке.
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
                message = AppendMessage(message, "Генерация следующих целей не производится.");
                break;
            }

            var targetMessage = target.GenerateArtefacts();
            if (targetMessage != null)
                message = AppendMessage(message, targetMessage);
        }

        return message;
    }
}
