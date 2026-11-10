using VkRadio.LowCode.AppGen.ArtefactGenerators.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Class.Method;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Component;
using VkRadio.LowCode.AppGen.ArtefactGenerators.Sql.Core;
using VkRadio.LowCode.AppGen.Domain.Names;
using VkRadio.LowCode.AppGen.Domain.PropertyDefinition.ConcreteFunctionalTypes;
using PackNS = VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.Core.Package;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Package.Gui;

public class ListsPackage : PackNS.Package
{
    enum ContentAlignment
    {
        Left,
        Center,
        Right
    }

    const float c_pixelsPerSymbol = 200f / 36f;

    public ListsPackage(GuiPackage parentPackage, NaturalLanguageEnum preferNaturalLanguageForComments, NaturalLanguageEnum uiLanguage, bool supportHiDpi)
        : base(parentPackage, "Lists")
    {
        var model = ParentPackage.ParentPackage.ParentPackage.DomainModel;
        var dbModel = ParentPackage.ParentPackage.ParentPackage.DBbSchemaModel;

        // For each data object type definition create component with a corresponding class
        var entityDefs = model.AllEntityDefinitions.Values;

        foreach (var entityDef in entityDefs)
        {
            var typeName = CSharpHelper.GenerateEntityClassName(entityDef);
            var formName = "DOL" + typeName;

            #region Main comonent of a card
            #region Initialize
            var component = new CSComponentWMainClass
            {
                Package = this,
                Name = formName + ".cs",
                EntityDefinition = entityDef,
                Namespace = $"{ParentPackage.ParentPackage.RootNamespace}.Gui.Lists"
            };
            _components.Add(component.Name, component);
            component.SystemUsings.Add("System.Windows.Forms");
            component.UserUsings.Add("VkRadio.LowCode.Libs.Gui.WinForms");

            var cls = new CSClass
            {
                Component = component,
                DocComment = new XmlComment((preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Список объектов " : "List of objects ") + NameHelper.GetLocalNameUpperCase(entityDef.Names, preferNaturalLanguageForComments)),
                Name = formName,
                InheritsFrom = "DOList",
                Partial = true
            };
            component.Classes.Add(cls.Name, cls);
            component.MainClass = cls;

            var correspondence = (TableAndEntityCorrespondence)dbModel.TableAndSourceCorrespondence[entityDef.Id];

            var ctor = new CSConstructor(
                cls,
                new XmlComment(preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Конструктор списка" : "List constructor"),
                ElementVisibilityClassic.Public
            );
            cls.Constructors.Add(CSharpHelper.GenerateMethodKey(ctor), ctor);
            ctor.BodyStrings.Add("InitializeComponent();");
            ctor.BodyStrings.Add(string.Empty);
            ctor.BodyStrings.Add("DGV_ListProtected.Columns.AddRange(new DataGridViewColumn[]");
            ctor.BodyStrings.Add("{");

            // Default sort order
            bool reverseOrder;
            var orderByPropertyIndex = -1;
            var orderByProperty = GeneralHelper.GetListSortProperty(entityDef, out reverseOrder);

            var index = 0;
            var isFirst = true;
            var intDecimals = new List<int>();
            var alignments = new List<ContentAlignment>();

            foreach (var propCorr in correspondence.PropertyCorrespondences)
            {
                var vf = propCorr.TableField as ValueField;

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

                    if (vf.EntityPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTOrderNumber)
                    {
                        colWidth = 30;
                        align = ContentAlignment.Center;
                    }
                    else if (vf.EntityPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTBoolean)
                    {
                        colWidth = 30;
                        align = ContentAlignment.Center;
                    }
                    else if (vf.EntityPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTDecimal)
                    {
                        colWidth = 70;
                        align = ContentAlignment.Right;
                    }
                    else if (vf.EntityPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTDateAndTime)
                    {
                        colWidth = 100;
                    }
                    else if (vf.EntityPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTDate)
                    {
                        colWidth = 70;
                    }
                    else if (vf.EntityPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTTime)
                    {
                        colWidth = 50;
                    }
                    else if (vf.EntityPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTInteger)
                    {
                        colWidth = 60;
                        align = ContentAlignment.Right;
                    }

                    var colCaption = NameHelper.GetLocalNameUpperCase(propCorr.PropertyDefinition.Names, uiLanguage);
                    var measuredCaptionWidth = (int)Math.Floor(colCaption.Length * c_pixelsPerSymbol);

                    if (measuredCaptionWidth > colWidth)
                    {
                        //colCaption = colCaption.Shorten((int)Math.Floor(colWidth / c_pixelsPerSymbol));
                        colCaption = colCaption.ShortenLegacy(); // The upper shortening is too restrictive, need to think how to fit the descriptive headers without overflowing the width
                    }

                    colCaption = NameHelper.EncodeCSharpText(colCaption);
                    alignments.Add(align);

                    var colWidthExpression = supportHiDpi
                        ? $"LogicalToDeviceUnits({colWidth})"
                        : $"{colWidth}";
                    #endregion

                    ctor.BodyStrings.Add($"    new DataGridViewTextBoxColumn()");
                    ctor.BodyStrings.Add($"    {{");
                    ctor.BodyStrings.Add($"        DataPropertyName = \"{vf.Name}\",");
                    ctor.BodyStrings.Add($"        HeaderText = \"{colCaption}\",");
                    ctor.BodyStrings.Add($"        Name = \"COL_{NameHelper.NamesToPascalCase(propCorr.PropertyDefinition.Names)}\",");
                    ctor.BodyStrings.Add($"        ReadOnly = true,");
                    ctor.BodyStrings.Add($"        Visible = true,");
                    ctor.BodyStrings.Add($"        Width = {colWidthExpression},");
                    ctor.BodyStrings.Add($"        DisplayIndex = {index}");
                    ctor.BodyStrings.Add($"    }}");
                    index++;

                    // Fill list of decimalPositions by values of number of symbols after comma, if a column contains a type PFTMoney (otherwise leave 0).
                    var decimalPositions = 0;

                    if (vf.EntityPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTMoney)
                    {
                        var pftMoney = (PFTMoney)vf.EntityPropertyCorrespondence.PropertyDefinition.FunctionalType;
                        decimalPositions = pftMoney.DecimalPositions;
                    }
                    else if (vf.EntityPropertyCorrespondence.PropertyDefinition.FunctionalType is PFTDecimal)
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
                    ctor.BodyStrings.Add($"DGV_ListProtected.Columns[{i + 1}].DefaultCellStyle.Alignment = DataGridViewContentAlignment.Middle{(alignments[i] == ContentAlignment.Center ? "Center" : "Right")};");
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

            var intType = "int"; // ((ArtefactGeneratorCSharpClassic)in_parentPackage.ParentPackage.ParentPackage.ArtefactGenerationTarget.Generator).Target.IsDependantOnSQLite ? "long" : "int";
            string yesText, noText;

            if (uiLanguage == NaturalLanguageEnum.Ru)
            {
                yesText = "да";
                noText = "нет";
            }
            else
            {
                yesText = "yes";
                noText = "no";
            }

            var methodFormatter = new CSMethod(
                cls,
                "DGV_ListProtected_CellFormatting",
                new XmlComment(preferNaturalLanguageForComments == NaturalLanguageEnum.Ru ? "Обработчик форматирования некоторых значений" : "Formatter for some values"),
                ElementVisibilityClassic.Private,
                null,
                null,
                new List<string>
                {
                    "if (e.ColumnIndex == 0)",
                    "    return;",
                    "int intPositions = _decimalIntPositions[e.ColumnIndex - 1];",
                    "if (intPositions != 0)",
                    "{",
                    $"    {intType}? eValue = e.Value as {intType}?;",
                    "    if (eValue.HasValue)",
                    "    {",
                    "        e.Value = (eValue.Value / 100m).ToString($\"N{intPositions}\");",
                    //"        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;",
                    "        e.FormattingApplied = true;",
                    "    }",
                    "}",
                    "else",
                    "{",
                    "    bool? boolValue = e.Value as bool?;",
                    "    if (boolValue.HasValue)",
                    "    {",
                    $"        e.Value = boolValue.Value ? \"{yesText}\" : \"{noText}\";",
                    //"        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;",
                    "        e.FormattingApplied = true;",
                    "    }",
                    "}"
                },
                null,
                false,
                false
            );
            var param1 = new CSParameter("sender", new CSharpReferenceType(false, TypeConstants.Reference.C_OBJECT, false));
            var param2 = new CSParameter("e", new CSharpReferenceType(false, "DataGridViewCellFormattingEventArgs", false));
            methodFormatter.Params.Add(param1.Name, param1);
            methodFormatter.Params.Add(param2.Name, param2);
            cls.Methods.Add(CSharpHelper.GenerateMethodKey(methodFormatter), methodFormatter);
            #endregion
            #endregion

            #region *.Designer component
            var designerComponent = new ListDesignerComponent(this, formName, ParentPackage.ParentPackage.RootNamespace);
            _components.Add(designerComponent.Name, designerComponent);
            #endregion
        }
    }

    public new GuiPackage ParentPackage => (GuiPackage)_parentPackage;
}
