using System.Xml.Linq;

namespace ArtefactGenerationProject.ArtefactGenerator.Sql
{
    public struct DbParams
    {
        public string Host;
        public string DbName;
        public bool OsSecurityUseCurrentUser;
        public string User;
        public string Password;

        public DbParams(string in_host, string in_dbName, bool in_osSecurityUseCurrentUser, string in_user, string in_password)
        {
            Host = in_host;
            DbName = in_dbName;
            OsSecurityUseCurrentUser = in_osSecurityUseCurrentUser;
            User = in_user;
            Password = in_password;
        }

        public static DbParams ReadFromXElement(XElement in_xel)
        {
            XElement xel = in_xel.Element("Host");
            string host = xel != null ? xel.Value : null;
            xel = in_xel.Element("DbName");
            string dbName = xel != null ? xel.Value : null;
            xel = in_xel.Element("OsSecurityUseCurrentUser");
            bool osSecurity = false;
            string
                user = null,
                password = null;
            if (xel != null && bool.Parse(xel.Value))
            {
                osSecurity = true;
            }
            else
            {
                xel = in_xel.Element("User");
                user = xel != null ? xel.Value : null;
                xel = in_xel.Element("Password");
                password = xel != null ? xel.Value : null;
            }

            return new DbParams(host, dbName, osSecurity, user, password);
        }
    };
}
