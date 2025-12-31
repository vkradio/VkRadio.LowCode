using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace VkRadio.LowCode.Gui.WinForms
{
    public class QuickSelectStorage
    {
        const int c_maxRowCount = 20;

        /// <summary>
        /// Application folder name, located in AppData, that contains QuickSelect.xml cache.
        /// </summary>
        public static string ProgramFolder { get; set; }

        public class SelectableRow
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        };

        string _filePath;
        XElement _xel;

        string GetFilePath()
        {
            if (_filePath == null)
            {
                var dirPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), !string.IsNullOrEmpty(ProgramFolder) ? ProgramFolder : "VkRadio.LowCode.Gui.WinForms");
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
                _filePath = Path.Combine(dirPath, "QuickSelect.xml");
            }
            return _filePath;
        }
        XElement GetXel()
        {
            if (_xel == null)
            {
                string filePath = GetFilePath();
                if (!File.Exists(filePath))
                    _xel = new XElement("QuickSelect");
                else
                    _xel = XElement.Load(_filePath);
            }
            return _xel;
        }

        public List<SelectableRow> GetRowsForDOT(string in_dot)
        {
            var result = new List<SelectableRow>();

            var xelRoot = GetXel();
            var xelDot = xelRoot.Element(in_dot);
            if (xelDot != null)
            {
                foreach (XElement xelRow in xelDot.Elements())
                {
                    SelectableRow row = new SelectableRow() { Id = new Guid(xelRow.Element("Id").Value), Name = xelRow.Element("N").Value };
                    result.Add(row);
                }
            }

            return result;
        }
        public SelectableRow GetRowById(List<SelectableRow> in_list, Guid in_id)
        {
            foreach (SelectableRow row in in_list)
            {
                if (row.Id == in_id)
                    return row;
            }
            return null;
        }
        public void SetFirstRow(List<SelectableRow> in_list, SelectableRow in_firstRow)
        {
            SelectableRow rowToRemove = null;
            foreach (SelectableRow row in in_list)
            {
                if (row.Id == in_firstRow.Id)
                {
                    rowToRemove = row;
                    break;
                }
            }
            if (rowToRemove != null)
                in_list.Remove(rowToRemove);

            in_list.Insert(0, in_firstRow);
            if (in_list.Count > c_maxRowCount)
                in_list.RemoveAt(c_maxRowCount);
        }
        public void RemoveRow(List<SelectableRow> in_list, Guid in_id)
        {
            SelectableRow rowToRemove = null;
            foreach (SelectableRow row in in_list)
            {
                if (row.Id == in_id)
                {
                    rowToRemove = row;
                    break;
                }
            }
            if (rowToRemove != null)
                in_list.Remove(rowToRemove);
        }
        public void SaveList(string in_dot, List<SelectableRow> in_list)
        {
            XElement xelRoot = GetXel();
            XElement xelDot = xelRoot.Element(in_dot);
            if (xelDot == null)
            {
                xelDot = new XElement(in_dot);
                xelRoot.Add(xelDot);
            }
            else
            {
                xelDot.Elements().Remove();
            }

            foreach (SelectableRow row in in_list)
            {
                xelDot.Add(new XElement("Row",
                    new XElement("Id", row.Id.ToString()),
                    new XElement("N", row.Name)
                ));
            }
            xelRoot.Save(GetFilePath());
        }
    };
}
