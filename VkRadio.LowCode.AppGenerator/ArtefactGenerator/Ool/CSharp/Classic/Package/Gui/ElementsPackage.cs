using System;
using System.Collections.Generic;

using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract;
using ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Component;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Common.Class.Method;
using ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Component;
using ArtefactGenerationProject.ArtefactGenerator.Sql;
using MetaModel.DOTDefinition;
using MetaModel.PropertyDefinition;
using MetaModel.PropertyDefinition.ConcreteFunctionalTypes;
using PackNS = ArtefactGenerationProject.ArtefactGenerator.Ool.Abstract.Package;

namespace ArtefactGenerationProject.ArtefactGenerator.Ool.CSharp.Classic.Package.Gui
{
    public class ElementsPackage: PackNS.Package
    {
        Dictionary<string, CSComponentWMainClass> _mainComponents = new Dictionary<string, CSComponentWMainClass>();
        Dictionary<string, ComponentWPredefinedCode> _designerComponents = new Dictionary<string, ComponentWPredefinedCode>();
        Dictionary<string, ComponentWPredefinedCode> _resxComponents = new Dictionary<string, ComponentWPredefinedCode>();

        const int c_widgetWidth = 305;
        const int c_textWindgetWidth = c_widgetWidth * 2;
        const int c_panelWidth = c_textWindgetWidth + 3;
        const int c_checkBoxOffset = 3;

        #region Генерация основного компонента карточки элемента.
        /// <summary>
        /// Формирование строки вызова метода подсчета количества табличных объектов
        /// </summary>
        /// <param name="in_propertyName">Свойство класса ТОД</param>
        /// <returns>Строка вызова метода, подстчитывающего количество табличных объектов</returns>
        static string PropertyNameToCountMethod(string in_propertyName) { return "Get" + in_propertyName.Substring(0, in_propertyName.Length - 9) + "Count()"; }
        static List<CSharpHelper.PropertyWidgetDescriptor> GenerateConstructor(
            CSClass in_class,
            DOTDefinition in_dotDef,
            List<CSharpHelper.PropertyWidgetDescriptor> in_clearProps,
            List<CSharpHelper.PropertyWidgetDescriptor> in_selectProps,
            List<CSharpHelper.PropertyWidgetDescriptor> in_cardProps,
            List<CSharpHelper.PropertyWidgetDescriptor> in_listProps)
        {
            List<CSharpHelper.PropertyWidgetDescriptor> wDescs = new List<CSharpHelper.PropertyWidgetDescriptor>();

            CSConstructor ctor = new CSConstructor(in_class)
            {
                DocComment = new XmlComment("Конструктор панели редактирования/просмотра объекта данных"),
                Visibility = ElementVisibilityClassic.Public
            };
            in_class.Constructors.Add(CSharpHelper.GenerateMethodKey(ctor), ctor);

            ctor.BodyStrings.Add("InitializeComponent();");
            ctor.BodyStrings.Add(string.Empty);
            
            foreach (PropertyDefinition propDef in in_dotDef.PropertyDefinitions.Values)
            {
                CSharpHelper.PropertyWidgetDescriptor wDesc = CSharpHelper.GenerateWidgetDescForProperty(propDef);
                wDescs.Add(wDesc);

                if (!(propDef.FunctionalType is PFTLink))
                {
                    if (wDesc.WidgetName.Substring(0, 3) == "SF_")
                        ctor.BodyStrings.Add(string.Format("{0}.ValueChanged += (s, e) => AnyValueChanged(s, e);", wDesc.WidgetName));
                    else if (wDesc.WidgetName.Substring(0, 4) == "CHK_")
                        ctor.BodyStrings.Add(string.Format("{0}.CheckStateChanged += (s, e) => AnyValueChanged(s, e);", wDesc.WidgetName));
                    else
                        throw new ApplicationException(string.Format("Unsupported widget prefix (name: {0}).", wDesc.WidgetName));
                }
                else
                {
                    string propClassName = NameHelper.NamesToHungarianName(propDef.Names);

                    ctor.BodyStrings.Add(string.Format("{0}.ValueChanged += (s, e) => AnyValueChanged(s, e);", wDesc.WidgetName));

                    if (!(propDef.FunctionalType is PFTBackReferencedTable || propDef.FunctionalType is PFTTablePart))
                    {
                        ctor.BodyStrings.Add(string.Format("{0}.ClearClick += (s, e) => {{ ClearValue{1}(); }};", wDesc.WidgetName, wDesc.PropertyName));
                        in_clearProps.Add(wDesc);

                        ctor.BodyStrings.Add(string.Format("{0}.SelectClick += (s, e) => {{ SelectValue{1}(); }};", wDesc.WidgetName, wDesc.PropertyName));
                        ctor.BodyStrings.Add(string.Format("{0}.QuickSelectClick += (s, e) => {{ QuickSelectValue{1}(); }};", wDesc.WidgetName, wDesc.PropertyName));
                        in_selectProps.Add(wDesc);

                        ctor.BodyStrings.Add(string.Format("{0}.CardClick += (s, e) => {{ Card{1}(); }};", wDesc.WidgetName, wDesc.PropertyName));
                        in_cardProps.Add(wDesc);
                    }
                    else
                    {
                        ctor.BodyStrings.Add(string.Format("{0}.SelectClick += (s, e) => {{ ListValue{1}(); }};", wDesc.WidgetName, wDesc.PropertyName));
                        in_listProps.Add(wDesc);
                    }
                }
            }

            return wDescs;
        }
        static void GenerateMethodSyncFromDOT(CSClass in_class, DOTDefinition in_dotDef, List<CSharpHelper.PropertyWidgetDescriptor> in_wDescs)
        {
            CSMethod method = new CSMethod()
            {
                AdditionalKeywords = "override",
                Class = in_class,
                DocComment = new XmlComment("Синхронизация виджета из данных объекта"),
                Name = "SyncFromDOT",
                ReturnType = "void",
                Visibility = ElementVisibilityClassic.Public
            };
            CSParameter param = new CSParameter()
            {
                Name = "in_o",
                Type = "DbMappedDOT"
            };
            method.Params.Add(param.Name, param);
            in_class.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);

            string className = NameHelper.NamesToHungarianName(in_dotDef.Names);

            method.BodyStrings.Add(string.Format("{0} o = ({1})in_o;", className, className));
            method.BodyStrings.Add(string.Empty);

            foreach (CSharpHelper.PropertyWidgetDescriptor wDesc in in_wDescs)
            {
                if (wDesc.WidgetName.Substring(0, 3) == "SF_")
                {
                    method.BodyStrings.Add(string.Format("{0}.SetValue(o.{1});", wDesc.WidgetName, wDesc.PropertyName));
                }
                else if (wDesc.WidgetName.Substring(0, 4) == "CHK_")
                {
                    method.BodyStrings.Add(string.Format("{0}.Checked = o.{1};", wDesc.WidgetName, wDesc.PropertyName));
                }
                else if (wDesc.WidgetName.Substring(0, 4) == "SEL_")
                {
                    string value = wDesc.PropertyName;
                    if (wDesc.PropertyDefinition.FunctionalType is PFTBackReferencedTable ||
                        wDesc.PropertyDefinition.FunctionalType is PFTTablePart)
                    {
                        value = PropertyNameToCountMethod(value);
                    }
                    method.BodyStrings.Add(string.Format("{0}.SetValue(o.{1});", wDesc.WidgetName, value));
                }
                else
                {
                    throw new ApplicationException(string.Format("Unsupported widget prefix (name: {0}).", wDesc.WidgetName));
                }
            }
        }
        static void GenerateMethodSyncToDOT(CSClass in_class, DOTDefinition in_dotDef, List<CSharpHelper.PropertyWidgetDescriptor> in_wDescs)
        {
            CSMethod method = new CSMethod()
            {
                AdditionalKeywords = "override",
                Class = in_class,
                DocComment = new XmlComment("Синхронизация виджета из данных объекта"),
                Name = "SyncToDOT",
                ReturnType = "string",
                Visibility = ElementVisibilityClassic.Public
            };
            CSParameter param = new CSParameter()
            {
                Name = "in_o",
                Type = "DbMappedDOT"
            };
            method.Params.Add(param.Name, param);
            in_class.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);

            string className = NameHelper.NamesToHungarianName(in_dotDef.Names);

            method.BodyStrings.Add(string.Format("{0} o = ({1})in_o;", className, className));
            method.BodyStrings.Add(string.Empty);

            bool propSetted = false;
            foreach (CSharpHelper.PropertyWidgetDescriptor wDesc in in_wDescs)
            {
                if (wDesc.WidgetName.Substring(0, 3) == "SF_")
                {
                    bool isNullable, isId;
                    string typeName;
                    CSharpHelper.GetTypeInfoForFillDOFromReaderRow(wDesc.PropertyDefinition, out isNullable, out isId, out typeName);

                    string typeNameForMethod = string.Empty;
                    if (typeName.IndexOf("string") == 0)
                        typeNameForMethod = "String";
                    else if (typeName.IndexOf("int") == 0)
                        typeNameForMethod = "Int";
                    else if (typeName.IndexOf("decimal") == 0)
                        typeNameForMethod = "Decimal";
                    else if (typeName.IndexOf("DateTime") == 0)
                        typeNameForMethod = "DateTime";
                    else if (typeName.IndexOf("Guid") == 0)
                        typeNameForMethod = "Guid";
                    else
                        throw new ApplicationException(string.Format("Unsupported typeName: {0}.", typeName ?? "NULL"));

                    string gettingMethod = string.Format("GetValueAs{0}{1}({2})",
                        typeNameForMethod,
                        isNullable ? "Nullable" : string.Empty,
                        !isNullable && typeNameForMethod != "String" ? string.Format("o.{0}", wDesc.PropertyName) : string.Empty);

                    method.BodyStrings.Add(string.Format("o.{0} = {1}.{2};", wDesc.PropertyName, wDesc.WidgetName, gettingMethod));
                    propSetted = true;
                }
                else if (wDesc.WidgetName.Substring(0, 4) == "CHK_")
                {
                    method.BodyStrings.Add(string.Format("o.{0} = {1}.Checked;", wDesc.PropertyName, wDesc.WidgetName));
                    propSetted = true;
                }
                else if (wDesc.WidgetName.Substring(0, 4) == "SEL_")
                {
                    // Ничего не делаем.
                }
                else
                {
                    throw new ApplicationException(string.Format("Unsupported widget prefix (name: {0}).", wDesc.WidgetName));
                }
            }

            if (propSetted)
                method.BodyStrings.Add(string.Empty);
            method.BodyStrings.Add("return null;");
        }
        static void GenerateMethodsForClearProps(CSClass in_class, List<CSharpHelper.PropertyWidgetDescriptor> in_clearProps)
        {
            string dotClassName = in_clearProps.Count != 0 ?
                NameHelper.NamesToHungarianName(in_clearProps[0].PropertyDefinition.OwnerDefinition.Names) :
                string.Empty;

            foreach (CSharpHelper.PropertyWidgetDescriptor wDesc in in_clearProps)
            {
                CSMethod method = new CSMethod()
                {
                    Class = in_class,
                    DocComment = new XmlComment(string.Format("Очистка свойства {0}", NameHelper.GetStringSuitableToXmlText(wDesc.WidgetCaption))),
                    Name = string.Format(string.Format("ClearValue{0}", wDesc.PropertyName)),
                    ReturnType = "void",
                    Visibility = ElementVisibilityClassic.Private
                };
                in_class.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);

                method.BodyStrings.Add(string.Format("{0} o = ({0})_o;", dotClassName));
                method.BodyStrings.Add(string.Format("o.{0} = null;", wDesc.PropertyName));
                method.BodyStrings.Add(string.Format("{0}.SetValue(o.{1});", wDesc.WidgetName, wDesc.PropertyName));
            }
        }
        static void GenerateMethodsForSelectProps(CSClass in_class, List<CSharpHelper.PropertyWidgetDescriptor> in_selectProps)
        {
            string dotClassName = in_selectProps.Count != 0 ?
                NameHelper.NamesToHungarianName(in_selectProps[0].PropertyDefinition.OwnerDefinition.Names) :
                string.Empty;

            foreach (CSharpHelper.PropertyWidgetDescriptor wDesc in in_selectProps)
            {
                CSMethod method = new CSMethod()
                {
                    Class = in_class,
                    DocComment = new XmlComment(string.Format("Выбор значения свойства {0}", NameHelper.GetStringSuitableToXmlText(wDesc.WidgetCaption))),
                    Name = string.Format(string.Format("SelectValue{0}", wDesc.PropertyName)),
                    ReturnType = "void",
                    Visibility = ElementVisibilityClassic.Private
                };
                in_class.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);

                method.BodyStrings.Add(string.Format("using (Form frm = UiRegistry.Instance.Uil{0}.CreateList(true, null))", wDesc.PropertyClass));
                method.BodyStrings.Add("{");
                method.BodyStrings.Add("    if (frm.ShowDialog(this) == DialogResult.OK)");
                method.BodyStrings.Add("    {");
                method.BodyStrings.Add(string.Format("        {0} o = ({0})_o;", dotClassName));
                method.BodyStrings.Add(string.Empty);
                method.BodyStrings.Add(string.Format("        o.{0} = ({1})((FRM_DOList)frm).SelectedValue;", wDesc.PropertyName, wDesc.PropertyClass));
                method.BodyStrings.Add(string.Format("        {0}.SetValue(o.{1});", wDesc.WidgetName, wDesc.PropertyName));
                method.BodyStrings.Add(string.Empty);
                method.BodyStrings.Add(string.Format("        List<QuickSelectStorage.SelectableRow> qSelectList = UiRegistry.Instance.QuickSelectStorage.GetRowsForDOT(\"{0}\");", wDesc.PropertyClass));
                method.BodyStrings.Add(string.Format("        QuickSelectStorage.SelectableRow row = UiRegistry.Instance.QuickSelectStorage.GetRowById(qSelectList, o.{0}.Id.Value);", wDesc.PropertyName));
                method.BodyStrings.Add("        if (row == null)");
                method.BodyStrings.Add(string.Format("            row = new QuickSelectStorage.SelectableRow() {{ Id = o.{0}.Id.Value, Name = o.{0}.ToString() }};", wDesc.PropertyName));
                method.BodyStrings.Add("        else");
                method.BodyStrings.Add(string.Format("            row.Name = o.{0}.ToString();", wDesc.PropertyName));
                method.BodyStrings.Add("        UiRegistry.Instance.QuickSelectStorage.SetFirstRow(qSelectList, row);");
                method.BodyStrings.Add(string.Format("        UiRegistry.Instance.QuickSelectStorage.SaveList(\"{0}\", qSelectList);", wDesc.PropertyClass));
                method.BodyStrings.Add("    }");
                method.BodyStrings.Add("}");
            }
        }
        static void GenerateMethodsForQuickSelectProps(CSClass in_class, List<CSharpHelper.PropertyWidgetDescriptor> in_selectProps)
        {
            string dotClassName = in_selectProps.Count != 0 ?
                NameHelper.NamesToHungarianName(in_selectProps[0].PropertyDefinition.OwnerDefinition.Names) :
                string.Empty;

            foreach (CSharpHelper.PropertyWidgetDescriptor wDesc in in_selectProps)
            {
                CSMethod method = new CSMethod()
                {
                    Class = in_class,
                    DocComment = new XmlComment(string.Format("Быстрый выбор значения свойства {0}", NameHelper.GetStringSuitableToXmlText(wDesc.WidgetCaption))),
                    Name = string.Format(string.Format("QuickSelectValue{0}", wDesc.PropertyName)),
                    ReturnType = "void",
                    Visibility = ElementVisibilityClassic.Private
                };
                in_class.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);

                method.BodyStrings.Add(string.Format("List<QuickSelectStorage.SelectableRow> qSelectList = UiRegistry.Instance.QuickSelectStorage.GetRowsForDOT(\"{0}\");", wDesc.PropertyClass));
                method.BodyStrings.Add("ContextMenuStrip cmnu = new ContextMenuStrip();");
                method.BodyStrings.Add("ToolStripMenuItem mi;");
                method.BodyStrings.Add("if (qSelectList.Count == 0)");
                method.BodyStrings.Add("{");
                method.BodyStrings.Add("    mi = new ToolStripMenuItem() { Text = \"пусто\", Enabled = false };");
                method.BodyStrings.Add("    cmnu.Items.Add(mi);");
                method.BodyStrings.Add("}");
                method.BodyStrings.Add("else");
                method.BodyStrings.Add("{");
                method.BodyStrings.Add("    foreach (QuickSelectStorage.SelectableRow row in qSelectList)");
                method.BodyStrings.Add("    {");
                method.BodyStrings.Add("        mi = new ToolStripMenuItem() { Text = row.Name, Tag = row };");
                method.BodyStrings.Add(string.Format("        mi.Click += MI_QuickSelectValue{0}_Click;", wDesc.PropertyName));
                method.BodyStrings.Add("        cmnu.Items.Add(mi);");
                method.BodyStrings.Add("    }");
                method.BodyStrings.Add("}");
                method.BodyStrings.Add("cmnu.Show(Control.MousePosition.X, Control.MousePosition.Y);");
            }
        }
        static void GenerateMethodsForQuickSelectPropsMIClick(CSClass in_class, List<CSharpHelper.PropertyWidgetDescriptor> in_selectProps)
        {
            string dotClassName = in_selectProps.Count != 0 ?
                NameHelper.NamesToHungarianName(in_selectProps[0].PropertyDefinition.OwnerDefinition.Names) :
                string.Empty;

            foreach (CSharpHelper.PropertyWidgetDescriptor wDesc in in_selectProps)
            {
                CSMethod method = new CSMethod()
                {
                    Class = in_class,
                    DocComment = new XmlComment(string.Format("Быстрый выбор значения свойства {0} - нажатие пункта меню", NameHelper.GetStringSuitableToXmlText(wDesc.WidgetCaption))),
                    Name = string.Format(string.Format("MI_QuickSelectValue{0}_Click", wDesc.PropertyName)),
                    ReturnType = "void",
                    Visibility = ElementVisibilityClassic.Private
                };
                CSParameter param = new CSParameter() { Type = "object", Name = "sender" };
                method.Params.Add(param.Name, param);
                param = new CSParameter() { Type = "EventArgs", Name = "e" };
                method.Params.Add(param.Name, param);
                in_class.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);

                method.BodyStrings.Add(string.Format("List<QuickSelectStorage.SelectableRow> qSelectList = UiRegistry.Instance.QuickSelectStorage.GetRowsForDOT(\"{0}\");", wDesc.PropertyClass));
                method.BodyStrings.Add(string.Empty);
                method.BodyStrings.Add("ToolStripMenuItem mi = (ToolStripMenuItem)sender;");
                method.BodyStrings.Add("QuickSelectStorage.SelectableRow row = (QuickSelectStorage.SelectableRow)mi.Tag;");
                method.BodyStrings.Add("Guid id = row.Id;");
                method.BodyStrings.Add(string.Format("{0} selectedValue = StorageRegistry.Instance.{0}Storage.Restore(id) as {0};", wDesc.PropertyClass));
                method.BodyStrings.Add("if (selectedValue == null)");
                method.BodyStrings.Add("{");
                method.BodyStrings.Add("    MessageBox.Show(this, \"Этот объект больше не существует.\", \"Сообщение\", MessageBoxButtons.OK, MessageBoxIcon.Information);");
                method.BodyStrings.Add("    UiRegistry.Instance.QuickSelectStorage.RemoveRow(qSelectList, id);");
                method.BodyStrings.Add(string.Format("    UiRegistry.Instance.QuickSelectStorage.SaveList(\"{0}\", qSelectList);", wDesc.PropertyClass));
                method.BodyStrings.Add("    return;");
                method.BodyStrings.Add("}");
                method.BodyStrings.Add(string.Empty);
                method.BodyStrings.Add(string.Format("{0} o = ({0})_o;", dotClassName));
                method.BodyStrings.Add(string.Format("o.{0} = selectedValue;", wDesc.PropertyName));
                method.BodyStrings.Add(string.Format("SEL_{0}.SetValue(o.{0});", wDesc.PropertyName));
                method.BodyStrings.Add(string.Empty);
                method.BodyStrings.Add("UiRegistry.Instance.QuickSelectStorage.SetFirstRow(qSelectList, row);");
                method.BodyStrings.Add(string.Format("UiRegistry.Instance.QuickSelectStorage.SaveList(\"{0}\", qSelectList);", wDesc.PropertyClass));
            }
        }
        static void GenerateMethodsForListProps(CSClass in_class, List<CSharpHelper.PropertyWidgetDescriptor> in_listProps)
        {
            string dotClassName = in_listProps.Count != 0 ?
                NameHelper.NamesToHungarianName(in_listProps[0].PropertyDefinition.OwnerDefinition.Names) :
                string.Empty;

            foreach (CSharpHelper.PropertyWidgetDescriptor wDesc in in_listProps)
            {
                CSMethod method = new CSMethod()
                {
                    Class = in_class,
                    DocComment = new XmlComment(string.Format("Просмотр табличной части свойства {0}", NameHelper.GetStringSuitableToXmlText(wDesc.WidgetCaption))),
                    Name = string.Format(string.Format("ListValue{0}", wDesc.PropertyName)),
                    ReturnType = "void",
                    Visibility = ElementVisibilityClassic.Private
                };
                in_class.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);

                string fieldName = null; // NameHelper.NameToUnderscoreSeparatedName(wDesc.PropertyDefinition.Names) + "_id";
                PFTBackReferencedTable backRefTable = wDesc.PropertyDefinition.FunctionalType as PFTBackReferencedTable;
                if (backRefTable != null)
                {
                    fieldName = NameHelper.NameToUnderscoreSeparatedName(backRefTable.RelationshipReference.OwnerPropertyDefinition.Names);
                }
                else
                {
                    PFTTablePart tablePart = wDesc.PropertyDefinition.FunctionalType as PFTTablePart;
                    if (tablePart != null)
                    {
                        fieldName = NameHelper.NameToUnderscoreSeparatedName(tablePart.RelationshipTable.PropertyDefinitionInTable.Names);
                    }
                    else
                    {
                        throw new ApplicationException(string.Format("Unsupported PropertyFunctionalType {0} for list (PropertyDefinition Id {1}).", wDesc.PropertyDefinition.FunctionalType.GetType().Name, wDesc.PropertyDefinition.Id));
                    }
                }
                fieldName += "_id";
                method.BodyStrings.Add(string.Format("FilterSimple fs = new FilterSimple(\"COALESCE(\\\"{0}\\\", '\" + Guid.NewGuid().ToString() + \"') = '\" + (_o.Id.HasValue ? _o.Id.Value : Guid.Empty).ToString() + \"'\", null);", fieldName));
                method.BodyStrings.Add(string.Empty);
                method.BodyStrings.Add(string.Format("using (Form frm = UiRegistry.Instance.Uil{0}.CreateList(false, fs))", wDesc.PropertyClass));
                method.BodyStrings.Add("    frm.ShowDialog(this);");
                method.BodyStrings.Add(string.Format("{0} o = ({0})_o;", dotClassName));
                method.BodyStrings.Add(string.Format("{0}.SetValue(o.{1});", wDesc.WidgetName, PropertyNameToCountMethod(wDesc.PropertyName)));
            }
        }
        static void GenerateMethodsForCardProps(CSClass in_class, List<CSharpHelper.PropertyWidgetDescriptor> in_cardProps)
        {
            string dotClassName = in_cardProps.Count != 0 ?
                NameHelper.NamesToHungarianName(in_cardProps[0].PropertyDefinition.OwnerDefinition.Names) :
                string.Empty;

            foreach (CSharpHelper.PropertyWidgetDescriptor wDesc in in_cardProps)
            {
                CSMethod method = new CSMethod()
                {
                    Class = in_class,
                    DocComment = new XmlComment(string.Format("Просмотр подробностей свойства {0}", NameHelper.GetStringSuitableToXmlText(wDesc.WidgetCaption))),
                    Name = string.Format(string.Format("Card{0}", wDesc.PropertyName)),
                    ReturnType = "void",
                    Visibility = ElementVisibilityClassic.Private
                };
                in_class.Methods.Add(CSharpHelper.GenerateMethodKey(method), method);

                method.BodyStrings.Add(string.Format("{0} o = ({0})_o;", dotClassName));
                method.BodyStrings.Add(string.Empty);
                method.BodyStrings.Add(string.Format("if (o.{0} == null)", wDesc.PropertyName));
                method.BodyStrings.Add("{");
                method.BodyStrings.Add("    MessageBox.Show(this, c_noWayNoObjectSelected, c_captionFault, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);");
                method.BodyStrings.Add("    return;");
                method.BodyStrings.Add("}");
                method.BodyStrings.Add(string.Empty);
                method.BodyStrings.Add(string.Format("DbMappedDOT refObject = (DbMappedDOT)o.{0}.Clone();", wDesc.PropertyName));
                method.BodyStrings.Add(string.Format("using (IFRM_Card frm = UiRegistry.Instance.Uil{0}.CreateCard(refObject))", wDesc.PropertyClass));
                method.BodyStrings.Add("{");
                method.BodyStrings.Add("    ((Form)frm).ShowDialog(this);");
                method.BodyStrings.Add("    if (frm.Changed)");
                method.BodyStrings.Add("    {");
                //method.BodyStrings.Add(string.Format("        o.{0} = ({1})refObject;", wDesc.PropertyName, wDesc.PropertyClass));
                method.BodyStrings.Add("        o.ResetCachedRefProperties();");
                method.BodyStrings.Add(string.Format("        {0}.UpdateValue(refObject);", wDesc.WidgetName));
                method.BodyStrings.Add("    }");
                method.BodyStrings.Add("}");
            }
        }
        #endregion

        #region Генерация компонента *.Designer карточки элемента.
        static void GenerateWidgetConstructions(IList<string> in_predefinedCode, List<CSharpHelper.PropertyWidgetDescriptor> in_wDescs)
        {
            foreach (CSharpHelper.PropertyWidgetDescriptor wDesc in in_wDescs)
                in_predefinedCode.Add(string.Format("            this.{0} = new {1}();", wDesc.WidgetName, wDesc.WidgetType));
        }
        static int SetupWidgetParameters(IList<CSharpHelper.PropertyWidgetDescriptor> in_wDescs, IList<string> in_predefinedCode)
        {
            const int c_widgetHeight = 37;
            const int c_widgetVertGap = 6;
            const int c_multilineRows = 6;

            int widgetTop = 3;
            int tabIndex = 20;

            foreach (CSharpHelper.PropertyWidgetDescriptor wDesc in in_wDescs)
            {
                bool isBoolProp = wDesc.WidgetName.IndexOf("CHK_") == 0;
                string canSelectString = wDesc.CanSelect ? "true" : "false";
                string captionPropName = isBoolProp ? "Text" : "Caption";
                bool isMultiline = wDesc.PropertyDefinition.FunctionalType is PFTText;
                int widgetHeight = isMultiline ?
                    c_multilineRows * 20 + 17 :
                    c_widgetHeight;

                in_predefinedCode.Add("            // ");
                in_predefinedCode.Add(string.Format("            // {0}", wDesc.WidgetName));
                in_predefinedCode.Add("            // ");
                in_predefinedCode.Add(string.Format("            this.{0}.{1} = \"{2}\";", wDesc.WidgetName, captionPropName, NameHelper.GetStringSuitableToCSharp(wDesc.WidgetCaption)));
                if (wDesc.WidgetName.IndexOf("SEL_") == 0)
                {
                    in_predefinedCode.Add(string.Format("            this.{0}.EnabledCard = {1};", wDesc.WidgetName, canSelectString));
                    in_predefinedCode.Add(string.Format("            this.{0}.EnabledClear = {1};", wDesc.WidgetName, canSelectString));
                    in_predefinedCode.Add(string.Format("            this.{0}.EnabledSelect = true;", wDesc.WidgetName));
                    in_predefinedCode.Add(string.Format("            this.{0}.EnabledQuickSelect = {1};", wDesc.WidgetName, canSelectString));
                }
                in_predefinedCode.Add(string.Format("            this.{0}.Location = new System.Drawing.Point({1}, {2});", wDesc.WidgetName, isBoolProp ? c_checkBoxOffset : 0, widgetTop));
                in_predefinedCode.Add(string.Format("            this.{0}.Name = \"{0}\";", wDesc.WidgetName));
                if (isMultiline)
                {
                    in_predefinedCode.Add(string.Format("            this.{0}.Multiline = true;", wDesc.WidgetName));
                    in_predefinedCode.Add(string.Format("            this.{0}.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;", wDesc.WidgetName));
                }
                if (wDesc.PropertyDefinition.FunctionalType is PFTMoney)
                {
                    PFTMoney pftMoney = (PFTMoney)wDesc.PropertyDefinition.FunctionalType;
                    in_predefinedCode.Add(string.Format("            this.{0}.TreatAsDecimal = true;", wDesc.WidgetName));
                    in_predefinedCode.Add(string.Format("            this.{0}.DecimalPositions = {1};", wDesc.WidgetName, pftMoney.DecimalPositions));
                }
                in_predefinedCode.Add(string.Format("            this.{0}.Size = new System.Drawing.Size({1}, {2});", wDesc.WidgetName, isMultiline ? c_textWindgetWidth : (isBoolProp ? (c_widgetWidth - c_checkBoxOffset) : c_widgetWidth), widgetHeight));
                in_predefinedCode.Add(string.Format("            this.{0}.TabIndex = {1};", wDesc.WidgetName, tabIndex));
                if (wDesc.WidgetName.IndexOf("SEL_") == 0)
                {
                    in_predefinedCode.Add(string.Format("            this.{0}.UseCard = {1};", wDesc.WidgetName, canSelectString));
                    in_predefinedCode.Add(string.Format("            this.{0}.UseClear = {1};", wDesc.WidgetName, canSelectString));
                    in_predefinedCode.Add(string.Format("            this.{0}.UseSelect = true;", wDesc.WidgetName));
                    in_predefinedCode.Add(string.Format("            this.{0}.UseQuickSelect = {1};", wDesc.WidgetName, canSelectString));
                }
                if (!isBoolProp)
                    in_predefinedCode.Add(string.Format("            this.{0}.Value = \"\";", wDesc.WidgetName));

                widgetTop += widgetHeight + c_widgetVertGap;
                tabIndex++;
            }

            return widgetTop;
        }
        #endregion

        public ElementsPackage(GuiPackage in_parentPackage)
            : base(in_parentPackage, "Elements")
        {
            MetaModel.MetaModel mm = ParentPackage.ParentPackage.ParentPackage.DomainModel;
            DBSchemaMetaModelJson dbMM = ParentPackage.ParentPackage.ParentPackage.DBbSchemaModel;

            // Для каждого определения ТОД создаем компонент с соответствеющим классом.
            ICollection<DOTDefinition> dotDefs = mm.AllDOTDefinitions.Values;
            foreach (DOTDefinition dotDef in dotDefs)
            {
                string name = CSharpHelper.GenerateDOTClassName(dotDef);
                string formName = "DOP" + name;

                #region Основной компонент карточки.
                CSComponentWMainClass panComponent = new CSComponentWMainClass()
                {
                    Package = this,
                    Name = formName + ".cs",
                    DOTDefinition = dotDef,
                    Namespace = string.Format("{0}.Gui.Elements", ParentPackage.ParentPackage.RootNamespace)
                };
                _components.Add(panComponent.Name, panComponent);
                _mainComponents.Add(panComponent.Name, panComponent);
                panComponent.SystemUsings.Add("System");
                panComponent.SystemUsings.Add("System.Collections.Generic");
                panComponent.SystemUsings.Add("System.Windows.Forms");
                panComponent.UserUsings.Add("orm.Db");
                panComponent.UserUsings.Add("orm.Gui");
                panComponent.UserUsings.Add(string.Format("{0}.Model.DOT", ParentPackage.ParentPackage.RootNamespace));
                panComponent.UserUsings.Add(string.Format("{0}.Model.Storage", ParentPackage.ParentPackage.RootNamespace));

                CSClass panClass = new CSClass()
                {
                    Component = panComponent,
                    DocComment = new XmlComment("Панель (карточка) редактирования объекта " + NameHelper.GetLocalNameUpperCase(dotDef.Names)),
                    Name = formName,
                    InheritsFrom = "DOEditPanel",
                    Partial = true
                };
                panComponent.Classes.Add(panClass.Name, panClass);
                panComponent.MainClass = panClass;

                List<CSharpHelper.PropertyWidgetDescriptor>
                    clearProps = new List<CSharpHelper.PropertyWidgetDescriptor>(),
                    selectProps = new List<CSharpHelper.PropertyWidgetDescriptor>(),
                    cardProps = new List<CSharpHelper.PropertyWidgetDescriptor>(),
                    listProps = new List<CSharpHelper.PropertyWidgetDescriptor>();

                // 1. Создание конструктора.
                List<CSharpHelper.PropertyWidgetDescriptor> wDescs = GenerateConstructor(panClass, dotDef, clearProps, selectProps, cardProps, listProps);

                // 2. Создание метода SyncFromDOT.
                GenerateMethodSyncFromDOT(panClass, dotDef, wDescs);

                // 3. Создание метода SyncToDOT.
                GenerateMethodSyncToDOT(panClass, dotDef, wDescs);

                // 4. Создание методов для работы со ссылочными свойствами.
                GenerateMethodsForClearProps(panClass, clearProps);
                GenerateMethodsForSelectProps(panClass, selectProps);
                GenerateMethodsForQuickSelectProps(panClass, selectProps);
                GenerateMethodsForQuickSelectPropsMIClick(panClass, selectProps);
                GenerateMethodsForListProps(panClass, listProps);
                GenerateMethodsForCardProps(panClass, cardProps);
                #endregion

                #region Компонент *.Designer.
                ComponentWPredefinedCode designerComponent = new ComponentWPredefinedCode()
                {
                    Package = this,
                    EmitUtf8Bom = true,
                    LastLineWNewLine = true,
                    Name = string.Format("{0}.Designer.cs", formName)
                };
                _components.Add(designerComponent.Name, designerComponent);
                _designerComponents.Add(designerComponent.Name, designerComponent);

                designerComponent.PredefinedCode.Add(string.Format("namespace {0}.Gui.Elements", ParentPackage.ParentPackage.RootNamespace));
                designerComponent.PredefinedCode.Add("{");
                designerComponent.PredefinedCode.Add(string.Format("    partial class {0}", formName));
                designerComponent.PredefinedCode.Add("    {");
                designerComponent.PredefinedCode.Add("        /// <summary> ");
                designerComponent.PredefinedCode.Add("        /// Required designer variable.");
                designerComponent.PredefinedCode.Add("        /// </summary>");
                designerComponent.PredefinedCode.Add("        private System.ComponentModel.IContainer components = null;");
                designerComponent.PredefinedCode.Add(string.Empty);
                designerComponent.PredefinedCode.Add("        /// <summary> ");
                designerComponent.PredefinedCode.Add("        /// Clean up any resources being used.");
                designerComponent.PredefinedCode.Add("        /// </summary>");
                designerComponent.PredefinedCode.Add("        /// <param name=\"disposing\">true if managed resources should be disposed; otherwise, false.</param>");
                designerComponent.PredefinedCode.Add("        protected override void Dispose(bool disposing)");
                designerComponent.PredefinedCode.Add("        {");
                designerComponent.PredefinedCode.Add("            if (disposing && (components != null))");
                designerComponent.PredefinedCode.Add("            {");
                designerComponent.PredefinedCode.Add("                components.Dispose();");
                designerComponent.PredefinedCode.Add("            }");
                designerComponent.PredefinedCode.Add("            base.Dispose(disposing);");
                designerComponent.PredefinedCode.Add("        }");
                designerComponent.PredefinedCode.Add(string.Empty);
                designerComponent.PredefinedCode.Add("        #region Component Designer generated code");
                designerComponent.PredefinedCode.Add(string.Empty);
                designerComponent.PredefinedCode.Add("        /// <summary> ");
                designerComponent.PredefinedCode.Add("        /// Required method for Designer support - do not modify ");
                designerComponent.PredefinedCode.Add("        /// the contents of this method with the code editor.");
                designerComponent.PredefinedCode.Add("        /// </summary>");
                designerComponent.PredefinedCode.Add("        private void InitializeComponent()");
                designerComponent.PredefinedCode.Add("        {");

                // 1. Создание объектов виджетов свойств.
                GenerateWidgetConstructions(designerComponent.PredefinedCode, wDescs);

                designerComponent.PredefinedCode.Add("            this.SuspendLayout();");

                // 2. Настройка параметров свойств.
                int formHeight = SetupWidgetParameters(wDescs, designerComponent.PredefinedCode) + 42;

                designerComponent.PredefinedCode.Add("            // ");
                designerComponent.PredefinedCode.Add(string.Format("            // {0}", formName));
                designerComponent.PredefinedCode.Add("            // ");
                designerComponent.PredefinedCode.Add("            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);");
                designerComponent.PredefinedCode.Add("            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;");

                // 3. Добавление виджетов к форме.
                foreach (CSharpHelper.PropertyWidgetDescriptor wDesc in wDescs)
                    designerComponent.PredefinedCode.Add(string.Format("            this.Controls.Add(this.{0});", wDesc.WidgetName));

                designerComponent.PredefinedCode.Add(string.Format("            this.Name = \"{0}\";", formName));
                designerComponent.PredefinedCode.Add(string.Format("            this.Size = new System.Drawing.Size({0}, {1});", c_panelWidth, formHeight));
                designerComponent.PredefinedCode.Add("            this.ResumeLayout(false);");
                designerComponent.PredefinedCode.Add(string.Empty);
                designerComponent.PredefinedCode.Add("        }");
                designerComponent.PredefinedCode.Add(string.Empty);
                designerComponent.PredefinedCode.Add("        #endregion");
                designerComponent.PredefinedCode.Add(string.Empty);

                // 4. Объявление полей класса для виджетов.
                foreach (CSharpHelper.PropertyWidgetDescriptor wDesc in wDescs)
                    designerComponent.PredefinedCode.Add(string.Format("        private {0} {1};", wDesc.WidgetType, wDesc.WidgetName));

                designerComponent.PredefinedCode.Add("    }");
                designerComponent.PredefinedCode.Add("}");
                #endregion

                #region Компонент ресурса карточки
                CardResourceComponent resxComponent = new CardResourceComponent(this, formName + ".resx");
                _components.Add(resxComponent.Name, resxComponent);
                _resxComponents.Add(resxComponent.Name, resxComponent);
                #endregion
            }
        }

        public new GuiPackage ParentPackage { get { return (GuiPackage)_parentPackage; } }
        public IDictionary<string, CSComponentWMainClass> MainComponents { get { return _mainComponents; } }
        public IDictionary<string, ComponentWPredefinedCode> DesignerComponents { get { return _designerComponents; } }
        public IDictionary<string, ComponentWPredefinedCode> ResxComponents { get { return _resxComponents; } }
    };
}
