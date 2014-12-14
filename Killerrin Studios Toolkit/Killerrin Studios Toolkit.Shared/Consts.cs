using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace KillerrinStudiosToolkit
{
    public class Consts
    {
        public static bool isApplicationClosing = false;

        public static string ConvertToAPIConpliantString(string _text, char charToParse = ' ', char replacementChar = '-')
        {
            Debug.WriteLine("ConvertToAPIConpliantString()");
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
    }
}
