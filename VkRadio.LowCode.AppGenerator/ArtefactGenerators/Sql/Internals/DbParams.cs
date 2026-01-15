using System.Xml.Linq;

namespace VkRadio.LowCode.AppGenerator.ArtefactGenerators.Sql.Internals;

public struct DbParams
{
    public string? Host;
    public string? DbName;
    public bool OsSecurityUseCurrentUser;
    public string? User;
    public string? Password;

    public DbParams(string? host, string? dbName, bool osSecurityUseCurrentUser, string? user, string? password)
    {
        Host = host;
        DbName = dbName;
        OsSecurityUseCurrentUser = osSecurityUseCurrentUser;
        User = user;
        Password = password;
    }

    public static DbParams ReadFromXElement(XElement containingXel)
    {
        var xel = containingXel.Element("Host");
        var host = xel is not null
            ? xel.Value
            : null;

        xel = containingXel.Element("DbName");
        var dbName = xel is not null
            ? xel.Value
            : null;

        xel = containingXel.Element("OsSecurityUseCurrentUser");
        var osSecurity = false;
        string?
            user = null,
            password = null;

        if (xel is not null && bool.Parse(xel.Value))
        {
            osSecurity = true;
        }
        else
        {
            xel = containingXel.Element("User");
            user = xel is not null ? xel.Value : null;
            xel = containingXel.Element("Password");
            password = xel is not null ? xel.Value : null;
        }

        return new DbParams(host, dbName, osSecurity, user, password);
    }
}
