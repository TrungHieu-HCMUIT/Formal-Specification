using MenuStripGUI.Handler;
using MenuStripGUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuStripGUI.Utils
{
    class FormalLanguageHelper
    {
        public static FormalLanguageType GetFormalLanguageType(string pre, string post)
        {
            if (ContainsExplicitTypeKeywords(pre) || ContainsExplicitTypeKeywords(post))
            {
                return FormalLanguageType.TYPE_2;
            }
            return FormalLanguageType.TYPE_1;
        }

        private static bool ContainsExplicitTypeKeywords(string line)
        {
            String stringObj = line;
            return (stringObj.Contains("VM") || stringObj.Contains("TT")) && stringObj.Contains("TH");
        }

        //Map formal language type to primitiveType
        public static string MapFormalLanguageTypeToPrimitiveType(string type)
        {
            switch (type)
            {
                case FormalLanguageDataType.NATURAL:
                    return "int";
                case FormalLanguageDataType.ARRAY_OF_NATURAL:
                    return "int[]";
                case FormalLanguageDataType.INTEGER:
                    return "int";
                case FormalLanguageDataType.RATIONAL:
                    return "double";
                case FormalLanguageDataType.REAL:
                    return "double";
                case FormalLanguageDataType.ARRAY_OF_REAL:
                    return "double[]";
                case FormalLanguageDataType.BOOLEAN:
                    return "bool";
                case FormalLanguageDataType.CHAR_ARRAY:
                    return "string";
                default:
                    Console.WriteLine("Loi kieu du lieu khong ton tai");
                    return "0";
            }
        }
    }
}
