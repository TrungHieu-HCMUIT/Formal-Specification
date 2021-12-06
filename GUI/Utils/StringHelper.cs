using MenuStripGUI.Handler;
using MenuStripGUI.Model;
using System;

namespace MenuStripGUI.Utils
{
    public class StringHelper
    {
        public static string ClearRedundantText(string text)
        {
            text = text.Replace("\r\n", String.Empty)
                .Replace("\t", String.Empty)
                .Replace("\n", String.Empty)
                .Replace(" ", String.Empty);
            return text;
        }

        public static string ClearParenthese(string text)
        {
            text = text.Replace(" ", String.Empty);
            text = text.Replace("(", String.Empty);
            text = text.Replace(")", String.Empty);
            return text;
        }
    }
}
