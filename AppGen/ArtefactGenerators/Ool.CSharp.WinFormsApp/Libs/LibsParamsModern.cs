using System.Xml.Linq;

namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.WinFormsApp.Libs;

public abstract class LibsParamsModern : LibsParams
{
    public const string C_PROJ_NAME_WIN_FORMS = "Gui.WinForms";

    public const string C_PROJ_NAME_ORM = "Orm";

    public const string C_PROJ_NAME_ORM_MS_SQL = "Orm.MsSql";

    public const string C_PROJ_NAME_UTILS = "Utils";

    protected LibsParamsModern(XElement initFromTargetXElement, ArtefactGeneratorCSharpClassic generator)
        : base(initFromTargetXElement, generator)
    {
    }
}
