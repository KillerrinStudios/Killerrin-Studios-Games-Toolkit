using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace KillerrinStudiosToolkit
{
    public static class Extensions
    {
        public static string ConvertToAPIConpliantString(this string _text, char charToParse = ' ', char replacementChar = '-')
        {
            string text = _text;
            text.ToLower();
            char[] txtarr = text.ToCharArray();
            text = "";
            foreach (char c in txtarr) {
                if (c == charToParse) { text += replacementChar; }
                else { text += c; }
            }

            return text;
        }

        public static string PrintException(this Exception ex, string headerMessage = "")
        {
            return DebugTools.PrintOutException(headerMessage, ex);
        }

        public static string AddSpacesToSentence(this string text, bool preserveAcronyms = true)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++) {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
    }
}
