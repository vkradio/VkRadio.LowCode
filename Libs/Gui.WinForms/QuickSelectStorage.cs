using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace VkRadio.LowCode.Libs.Gui.WinForms
{
    /// <summary>
    /// This class should be redesigned:
    /// - We need to abstract the storage mechanism away from the hard-coded AppData file storage;
    /// - We need to work with collections in modern functional way with immutability.
    /// </summary>
    public class QuickSelectStorage
    {
        const int c_maxRowCount = 20;

        /// <summary>
        /// Application folder name, located in AppData, that contains QuickSelect.xml cache.
        /// </summary>
        public static string? ProgramFolder { get; set; }

        [SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "I do not want to review this simple legacy design")]
        public class SelectableRow
        {
            public Guid Id { get; set; }
            public string? Name { get; set; }
        };

        string? _filePath;
        XElement? _rootXElement;

        string GetFilePath()
        {
            if (_filePath == null)
            {
                var dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), !string.IsNullOrEmpty(ProgramFolder) ? ProgramFolder : typeof(QuickSelectStorage).Namespace!);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
                _filePath = Path.Combine(dirPath, "QuickSelect.xml");
            }
            return _filePath;
        }

        XElement GetRootXElement()
        {
            if (_rootXElement == null)
            {
                var filePath = GetFilePath();
                if (!File.Exists(filePath))
                    _rootXElement = new XElement("QuickSelect");
                else
                    _rootXElement = XElement.Load(filePath);
            }
            return _rootXElement;
        }

        public IList<SelectableRow> GetRowsForDOT(string dataObjectType)
        {
            var xelDot = GetRootXElement().Element(dataObjectType);

            if (xelDot != null)
            {
                return xelDot
                    .Elements()
                    .Select(e => new SelectableRow { Id = new Guid(e.Element("Id")!.Value), Name = e.Element("N")!.Value })
                    .ToList();
            }
            else
            {
                return new List<SelectableRow>();
            }
        }

        public static SelectableRow? GetRowById(IList<SelectableRow> list, Guid id)
        {
            Guard.Against.Null(list, nameof(list));
            return list.FirstOrDefault(r => r.Id == id);
        }

        public static void SetFirstRow(IList<SelectableRow> list, SelectableRow firstRow)
        {
            Guard.Against.Null(list, nameof(list));
            Guard.Against.Null(firstRow, nameof(firstRow));

            var rowToRemove = list.FirstOrDefault(r => r.Id == firstRow.Id);
            if (rowToRemove != null)
                list.Remove(rowToRemove);

            list.Insert(0, firstRow);
            if (list.Count > c_maxRowCount)
                list.RemoveAt(c_maxRowCount);
        }

        public static void RemoveRow(IList<SelectableRow> list, Guid id)
        {
            Guard.Against.Null(list, nameof(list));

            var rowToRemove = list.FirstOrDefault(r => r.Id == id);
            if (rowToRemove != null)
                list.Remove(rowToRemove);
        }

        public void SaveList(string dataObjectType, IList<SelectableRow> list)
        {
            Guard.Against.NullOrEmpty(dataObjectType, nameof(dataObjectType));
            Guard.Against.Null(list, nameof(list));

            var xelRoot = GetRootXElement();
            var xelDot = xelRoot.Element(dataObjectType);
            if (xelDot == null)
            {
                xelDot = new XElement(dataObjectType);
                xelRoot.Add(xelDot);
            }
            else
            {
                xelDot.Elements().Remove();
            }

            foreach (var row in list)
            {
                xelDot.Add(new XElement("Row",
                    new XElement("Id", row.Id.ToString()),
                    new XElement("N", row.Name)
                ));
            }

            xelRoot.Save(GetFilePath());
        }
    }
}
