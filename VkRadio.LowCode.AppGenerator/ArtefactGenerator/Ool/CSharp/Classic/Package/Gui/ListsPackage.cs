using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Component;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Common.Class.Method;
using VkRadio.LowCode.AppGenerator.ArtefactGenerator.Sql;
using VkRadio.LowCode.AppGenerator.MetaModel.PropertyDefinition.ConcreteFunctionalTypes;
using PackNS = VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.Abstract.Package;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerator.Ool.CSharp.Classic.Package.Gui;

public class ListsPackage : PackNS.Package
{
    enum ContentAlignment
    {
        Left,
        Center,
        Right
    }

    const float c_pixelsPerSymbol = 200f / 36f;

    public ListsPackage(GuiPackage in_parentPackage)
        : base(in_parentPackage, "Lists")
    {
        var model = ParentPackage.ParentPackage.ParentPackage.DomainModel;
        var dbModel = ParentPackage.ParentPackage.ParentPackage.DBbSchemaModel;

        // For each data object type definition create component with a corresponding class
        var dotDefs = model.AllDOTDefinitions.Values;

        foreach (var dotDef in dotDefs)
        {
            var typeName = CSharpHelper.GenerateDOTClassName(dotDef);
            var formName = "DOL" + typeName;

            #region Main comonent of a card
            #region Initialize
            var component = new CSComponentWMainClass
            {
                Package = this,
                Name = formName + ".cs",
                DOTDefinition = dotDef,
                Namespace = $"{ParentPackage.ParentPackage.RootNamespace}.Gui.Lists"
            };
            _components.Add(component.Name, component);
            component.SystemUsings.Add("System.Windows.Forms");
            component.UserUsings.Add("orm.Gui");

            var cls = new CSClass
            {
                Component = component,
                DocComment = new XmlComment("List of objects " + NameHelper.GetLocalNameUpperCase(dotDef.Names)),
                Name = formName,
                InheritsFrom = "DOList",
                Partial = true
            };
            component.Classes.Add(cls.Name, cls);
            component.MainClass = cls;

            var correspondence = (TableAndDOTCorrespondenceJson)dbModel.TableAndSourceCorrespondence[dotDef.Id];

            var ctor = new CSConstructor(cls)
            {
                DocComment = new XmlComment("List constructor"),
                Visibility = ElementVisibilityClassic.Public
            };
            cls.Constructors.Add(CSharpHelper.GenerateMethodKey(ctor), ctor);
            ctor.BodyStrings.Add("InitializeComponent();");
            ctor.BodyStrings.Add(string.Empty);
            ctor.BodyStrings.Add("DGV_ListProtected.Columns.AddRange(new DataGridViewColumn[]");
            ctor.BodyStrings.Add("{");

            // Default sort order
            bool reverseOrder;
            var orderByPropertyIndex = -1;
            var orderByProperty = GeneralHelper.GetListSortProperty(dotDef, out reverseOrder);

            var index = 0;
            var isFirst = true;
            var intDecimals = new List<int>();
            var alignments = new List<ContentAlignment>();

            foreach (var propCorr in correspondence.PropertyCorrespondences)
            {
                var vf = propCorr.TableField as ValueFieldJson;

                if (vf is not null)
                {
                    if (!isFirst)
                    {
                        ctor.BodyStrings[ctor.BodyStrings.Count - 1] += ",";
                    }

                    isFirst = false;

                    if (orderByProperty != null &&
                        orderByPropertyIndex == -1 &&
                        orderByProperty.Id == propCorr.PropertyDefinition.Id)
                    {
                        orderByPropertyIndex = index + 1;
                    }

                    #region Calculate the column widths
                    var align = ContentAlignment.Left;
                    var colWidth = 200;

                    if (vf.DOTPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTOrderNumber)
                    {
                        colWidth = 30;
                        align = ContentAlignment.Center;
                    }
                    else if (vf.DOTPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTBoolean)
                    {
                        colWidth = 30;
                        align = ContentAlignment.Center;
                    }
                    else if (vf.DOTPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTDecimal)
                    {
                        colWidth = 70;
                        align = ContentAlignment.Right;
                    }
                    else if (vf.DOTPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTDateAndTime)
                    {
                        colWidth = 100;
                    }
                    else if (vf.DOTPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTDate)
                    {
                        colWidth = 70;
                    }
                    else if (vf.DOTPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTTime)
                    {
                        colWidth = 50;
                    }
                    else if (vf.DOTPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTInteger)
                    {
                        colWidth = 60;
                        align = ContentAlignment.Right;
                    }

                    var colCaption = NameHelper.GetLocalNameUpperCase(propCorr.PropertyDefinition.Names);
                    var measuredCaptionWidth = (int)Math.Floor(colCaption.Length * c_pixelsPerSymbol);

                    if (measuredCaptionWidth > colWidth)
                    {
                        colCaption = colCaption.Shorten((int)Math.Floor((float)colWidth / c_pixelsPerSymbol));
                    }

                    colCaption = NameHelper.GetStringSuitableToCSharp(colCaption);
                    alignments.Add(align);
                    #endregion

                    ctor.BodyStrings.Add($"    new DataGridViewTextBoxColumn()");
                    ctor.BodyStrings.Add($"    {{");
                    ctor.BodyStrings.Add($"        DataPropertyName = \"{vf.Name}\",");
                    ctor.BodyStrings.Add($"        HeaderText = \"{colCaption}\",");
                    ctor.BodyStrings.Add($"        Name = \"COL_{NameHelper.NamesToHungarianName(propCorr.PropertyDefinition.Names)}\",");
                    ctor.BodyStrings.Add($"        ReadOnly = true,");
                    ctor.BodyStrings.Add($"        Visible = true,");
                    ctor.BodyStrings.Add($"        Width = {colWidth},");
                    ctor.BodyStrings.Add($"        DisplayIndex = {index}");
                    ctor.BodyStrings.Add($"    }}");
                    index++;

                    // Fill list of decimalPositions by values of number of symbols after comma, if a column contains a type PFTMoney (otherwise leave 0).
                    var decimalPositions = 0;

                    if (vf.DOTPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTMoney)
                    {
                        var pftMoney = (PFTMoney)vf.DOTPropertyCorrespondence.PropertyDefinition.FunctionalType;
                        decimalPositions = pftMoney.DecimalPositions;
                    }
                    else if (vf.DOTPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTDecimal)
                    {
                        decimalPositions = 2;
                    }

                    intDecimals.Add(decimalPositions);
                }
            }

            ctor.BodyStrings.Add("});");
            ctor.BodyStrings.Add("for (int i = 1; i < DGV_ListProtected.Columns.Count; i++)");
            ctor.BodyStrings.Add("    DGV_ListProtected.Columns[i].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;");
            for (var i = 0; i < alignments.Count; i++)
            {
                if (alignments[i] != ContentAlignment.Left)
                {
                    ctor.BodyStrings.Add($"DGV_ListProtected.Columns[{(i + 1)}].DefaultCellStyle.Alignment = DataGridViewContentAlignment.Middle{(alignments[i] == ContentAlignment.Center ? "Center" : "Right")};");
                }
            }

            // Default sort order
            if (orderByPropertyIndex != -1)
            {
                ctor.BodyStrings.Add(string.Empty);
                ctor.BodyStrings.Add($"_defaultSortFieldIndex = {orderByPropertyIndex};");

                if (reverseOrder)
                {
                    ctor.BodyStrings.Add("_defaultSortAscending = false;");
                }
            }
            #endregion

            #region Format some values
            ctor.BodyStrings.Add(string.Empty);
            ctor.BodyStrings.Add("_decimalIntPositions = new int[]");
            ctor.BodyStrings.Add("{");

            for (var i = 0; i < intDecimals.Count; i++)
            {
                ctor.BodyStrings.Add("    " + intDecimals[i].ToString() + (i != intDecimals.Count - 1 ? "," : string.Empty));
            }
            ctor.BodyStrings.Add("};");
            ctor.BodyStrings.Add(string.Empty);
            ctor.BodyStrings.Add("DGV_ListProtected.CellFormatting += new DataGridViewCellFormattingEventHandler(DGV_ListProtected_CellFormatting);");

            var methodFormatter = new CSMethod
            {
                Class = cls,
                DocComment = new XmlComment("Formatter for some values"),
                Name = "DGV_ListProtected_CellFormatting",
                ReturnType = "void",
                Visibility = ElementVisibilityClassic.Private
            };
            var param1 = new CSParameter
            {
                Name = "sender",
                Type = "object"
            };
            var param2 = new CSParameter
            {
                Name = "e",
                Type = "DataGridViewCellFormattingEventArgs"
            };
            methodFormatter.Params.Add(param1.Name, param1);
            methodFormatter.Params.Add(param2.Name, param2);
            cls.Methods.Add(CSharpHelper.GenerateMethodKey(methodFormatter), methodFormatter);
            var intType = "int"; // ((ArtefactGeneratorCSharpClassic)in_parentPackage.ParentPackage.ParentPackage.ArtefactGenerationTarget.Generator).Target.IsDependantOnSQLite ? "long" : "int";
            methodFormatter.BodyStrings.Add("if (e.ColumnIndex == 0)");
            methodFormatter.BodyStrings.Add("    return;");
            methodFormatter.BodyStrings.Add("int intPositions = _decimalIntPositions[e.ColumnIndex - 1];");
            methodFormatter.BodyStrings.Add("if (intPositions != 0)");
            methodFormatter.BodyStrings.Add("{");
            methodFormatter.BodyStrings.Add($"    {intType}? eValue = e.Value as {intType}?;");
            methodFormatter.BodyStrings.Add("    if (eValue.HasValue)");
            methodFormatter.BodyStrings.Add("    {");
            methodFormatter.BodyStrings.Add("        e.Value = (eValue.Value / 100m).ToString($\"N{intPositions}\");");
            //methodFormatter.BodyStrings.Add("        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;");
            methodFormatter.BodyStrings.Add("        e.FormattingApplied = true;");
            methodFormatter.BodyStrings.Add("    }");
            methodFormatter.BodyStrings.Add("}");
            methodFormatter.BodyStrings.Add("else");
            methodFormatter.BodyStrings.Add("{");
            methodFormatter.BodyStrings.Add("    bool? boolValue = e.Value as bool?;");
            methodFormatter.BodyStrings.Add("    if (boolValue.HasValue)");
            methodFormatter.BodyStrings.Add("    {");
            methodFormatter.BodyStrings.Add("        e.Value = boolValue.Value ? \"да\" : \"нет\";");
            //methodFormatter.BodyStrings.Add("        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;");
            methodFormatter.BodyStrings.Add("        e.FormattingApplied = true;");
            methodFormatter.BodyStrings.Add("    }");
            methodFormatter.BodyStrings.Add("}");
            #endregion
            #endregion

            #region *.Designer component
            var designerComponent = new ListDesignerComponent(this, formName, ParentPackage.ParentPackage.RootNamespace);
            _components.Add(designerComponent.Name, designerComponent);
            #endregion
        }
    }

    public new GuiPackage ParentPackage { get { return (GuiPackage)_parentPackage; } }
}
