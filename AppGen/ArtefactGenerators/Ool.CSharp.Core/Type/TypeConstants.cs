namespace VkRadio.LowCode.AppGen.ArtefactGenerators.Ool.CSharp.Core.Type;

public static class TypeConstants
{
    public static class Value
    {
        public static string C_INT;

        public static string C_UINT;

        public static string C_FLOAT;

        public static string C_DOUBLE;

        public static string C_BYTE;

        public static string C_SHORT;

        public static string C_USHORT;

        public static string C_CHAR;

        public static string C_BOOL;

        public static string C_DATETIME;

        public static string C_GUID;

        public static string C_DECIMAL;

        static Value()
        {
            C_INT = "int";

            C_UINT = "uint";

            C_FLOAT = "float";

            C_DOUBLE = "double";

            C_BYTE = "byte";

            C_SHORT = "short";

            C_USHORT = "ushort";

            C_CHAR = "char";

            C_BOOL = "bool";

            C_DATETIME = "DateTime";

            C_GUID = "Guid";

            C_DECIMAL = "decimal";
        }
    }

    public static class Reference
    {
        public static string C_STRING;

        public static string C_OBJECT;

        static Reference()
        {
            C_STRING = "string";

            C_OBJECT = "object";
        }
    }
}
