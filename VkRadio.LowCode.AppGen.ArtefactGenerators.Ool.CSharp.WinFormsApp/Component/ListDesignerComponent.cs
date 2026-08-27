using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Component;
using PackNS = VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Package;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Component;

public class ListDesignerComponent : ComponentWPredefinedCode
{
    public ListDesignerComponent(PackNS.Package package, string className, string rootNamespace)
    {
        _emitUtf8Bom = true;
        _lastLineWNewLine = true;
        Package = package;
        Name = className + ".Designer.cs";

        _predefinedCode =
        [
           $"namespace {rootNamespace}.Gui.Lists",
            "{",
           $"    partial class {className}",
            "    {",
            "        /// <summary> ",
            "        /// Required designer variable.",
            "        /// </summary>",
            "        private System.ComponentModel.IContainer components = null;",
            "",
            "        /// <summary> ",
            "        /// Clean up any resources being used.",
            "        /// </summary>",
            "        /// <param name=\"disposing\">true if managed resources should be disposed; otherwise, false.</param>",
            "        protected override void Dispose(bool disposing)",
            "        {",
            "            if (disposing && (components != null))",
            "            {",
            "                components.Dispose();",
            "            }",
            "            base.Dispose(disposing);",
            "        }",
            "",
            "        #region Component Designer generated code",
            "",
            "        /// <summary> ",
            "        /// Required method for Designer support - do not modify ",
            "        /// the contents of this method with the code editor.",
            "        /// </summary>",
            "        private void InitializeComponent()",
            "        {",
            "            components = new System.ComponentModel.Container();",
            "            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;",
            "        }",
            "",
            "        #endregion",
            "    }",
            "}"
        ];
    }
}
